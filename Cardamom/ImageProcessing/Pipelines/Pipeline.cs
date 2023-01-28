using Cardamom.ImageProcessing.Pipelines.Nodes;
using OpenTK.Graphics.OpenGL4;

namespace Cardamom.ImageProcessing.Pipelines
{
    public class Pipeline
    {
        private class Node
        {
            internal IPipelineNode Step { get; }
            internal List<Edge> Incoming { get; set; } = new();
            internal List<Edge> Outgoing { get; set; } = new();
            internal Edge? IncomingOutput { get; set; }
            public int IsOutput { get; private set; }

            private Canvas? _cachedOutput;
            private int _outstanding;

            internal Node(IPipelineNode step)
            {
                Step = step;
            }

            internal void MarkOutput()
            {
                IsOutput++;
            }

            internal bool IsDone()
            {
                return _outstanding == 0;
            }

            internal void Release()
            {
                _outstanding--;
            }

            internal void Return(ICanvasProvider canvasProvider)
            {
                if (_cachedOutput != null && IsOutput == 0 && !Step.External)
                {
                    canvasProvider.Return(_cachedOutput);
                }
                _cachedOutput = null;
            }

            internal Canvas Run(ICanvasProvider canvasProvider)
            {
                Canvas? result;
                if (_cachedOutput != null)
                {
                    result = _cachedOutput;
                }
                else
                {
                    var inputs = new Dictionary<string, Canvas>();
                    foreach (var edge in Incoming)
                    {
                        var o = edge.Source.Run(canvasProvider);
                        inputs.Add(edge.InputName, o);
                    }
                    if (IncomingOutput != null)
                    {
                        result = IncomingOutput.Source.Run(canvasProvider);
                    }
                    else if (!Step.External)
                    {
                        result = canvasProvider.Get();
                    }
                    else
                    {
                        result = null; 
                    }
                    result = Step.Run(result, inputs);
                    _cachedOutput = result;
                    foreach (var edge in Incoming)
                    {
                        edge.Source.Release();
                        if (edge.Source.IsDone())
                        {
                            edge.Source.Return(canvasProvider);
                        }
                    }
                    _outstanding = Outgoing.Count + IsOutput;
                }
                return result;
            }
        }

        private class Edge
        {
            internal Node Source { get; }
            internal Node Sink { get; }
            internal string InputName { get; }

            internal Edge(Node source, Node sink, string inputName)
            {
                Source = source;
                Sink = sink;
                InputName = inputName;
            }
        }

        private readonly List<InputNode> _inputs;
        private readonly List<Node> _roots;

        private Pipeline(List<InputNode> inputs, List<Node> roots)
        {
            _inputs = inputs;
            _roots = roots;
        }

        public Canvas[] Run(ICanvasProvider canvasProvider, params Canvas[] input)
        {
            Precondition.Check(input.Length == _inputs.Count);
            for (int i=0; i <input.Length; ++i)
            {
                _inputs[i].SetCanvas(input[i]);
            }
            var outs = new Canvas[_roots.Count];
            for (int i=0; i<_roots.Count;++i)
            {
                outs[i] = _roots[i].Run(canvasProvider);
                _roots[i].Return(canvasProvider);
            }
            GL.Finish();
            return outs;
        }

        public class Builder
        {
            public List<IPipelineNode.IBuilder> Steps { get; set; } = new();
            public List<string> Outputs { get; set; } = new();

            public Builder AddOutput(string stepKey)
            {
                Outputs.Add(stepKey);
                return this;
            }

            public Builder AddNode(IPipelineNode.IBuilder step)
            {
                Steps.Add(step);
                return this;
            }

            public Pipeline Build()
            {
                var steps = Steps.Select(x => x.Build()).ToList();
                var nodes = steps.Select(x => new Node(x)).ToDictionary(x => x.Step.Key!, x => x);
                var inputs =
                    nodes.Values
                        .Where(x => x.Step is InputNode)
                        .Select(x => (InputNode)x.Step)
                        .OrderBy(x => x.Index)
                        .ToList();
                var output = Outputs.Select(x => nodes[x]).ToList();
                foreach (var node in nodes.Values)
                {
                    if (output.Contains(node))
                    {
                        node.MarkOutput();
                    }
                    foreach (var edge in node.Step.GetInputs())
                    {
                        var source = nodes[edge.Value];
                        Edge e = new(source, node, edge.Key);
                        node.Incoming.Add(e);
                        source.Outgoing.Add(e);
                    }
                    if (node.Step.GetOutput() != null)
                    {
                        var source = nodes[node.Step.GetOutput()!];
                        Edge e = new(source, node, "output");
                        node.IncomingOutput = e;
                        source.MarkOutput();
                    }
                }
                return new Pipeline(inputs, output);
            }

            public Builder Clone()
            {
                return new Builder()
                {
                    Steps = Steps.ToList(),
                    Outputs = Outputs.ToList()
                };
            }
        }
    }
}
