using Cardamom.ImageProcessing.Pipelines.Nodes;

namespace Cardamom.ImageProcessing.Pipelines
{
    public class Pipeline
    {
        private class Node
        {
            internal IPipelineNode Step { get; }
            internal List<Edge> Incoming { get; set; } = new();
            internal List<Edge> Outgoing { get; set; } = new();

            internal Node(IPipelineNode step)
            {
                Step = step;
            }

            internal Canvas Run(ICanvasProvider canvasProvider)
            {
                var inputs = new Dictionary<string, Canvas>();
                foreach (var edge in Incoming)
                {
                    inputs.Add(edge.InputName, edge.Source.Run(canvasProvider));
                }
                var outCanvas = Step.Run(inputs, canvasProvider);
                foreach (var canvas in inputs.Values)
                {
                    if (canvas != outCanvas)
                    {
                        canvasProvider.Return(canvas);
                    }
                }
                return outCanvas;
            }
        }

        private class Edge
        {
            internal Node Source { get; }
            internal string InputName { get; }

            internal Edge(Node source, string inputName)
            {
                Source = source;
                InputName = inputName;
            }
        }

        private List<Node> _roots;

        private Pipeline(List<Node> roots)
        {
            _roots = roots;
        }

        public Canvas[] Run(ICanvasProvider canvasProvider)
        {
            var outs = new Canvas[_roots.Count];
            for (int i=0; i<_roots.Count;++i)
            {
                outs[i] = _roots[i].Run(canvasProvider);
            }
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
                var output = Outputs.Select(x => nodes[x]).ToList();
                foreach (var node in nodes.Values)
                {
                    foreach (var edge in node.Step.GetInputs())
                    {
                        var source = nodes[edge.Value];
                        var e = new Edge(source, edge.Key);
                        node.Incoming.Add(e);
                        source.Outgoing.Add(e);
                    }
                }
                return new Pipeline(output);
            }
        }
    }
}
