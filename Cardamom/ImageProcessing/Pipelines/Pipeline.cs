namespace Cardamom.ImageProcessing.Pipelines
{
    public class Pipeline
    {
        private class Node
        {
            internal PipelineStep Step { get; }
            internal List<Edge> Incoming { get; set; } = new();
            internal Edge? Outgoing { get; set; }

            internal Node(PipelineStep step)
            {
                Step = step;
            }

            internal Canvas Run(ICanvasProvider canvasProvider)
            {
                var inputs = new Dictionary<string, Canvas>();
                foreach (var edge in Incoming)
                {
                    if (edge.Source == null)
                    {
                        inputs.Add(edge.InputName, canvasProvider.Get());
                    }
                    else
                    {
                        inputs.Add(edge.InputName, edge.Source.Run(canvasProvider));
                    }
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
            internal Node? Source { get; }
            internal string InputName { get; }

            internal Edge(Node? source, string inputName)
            {
                Source = source;
                InputName = inputName;
            }
        }

        private Node _root;

        private Pipeline(Node root)
        {
            _root = root;
        }

        public Canvas Run(ICanvasProvider canvasProvider)
        {
            return _root.Run(canvasProvider);
        }

        public class Builder
        {
            public List<PipelineStep.Builder> Steps { get; set; } = new();
            public string Output { get; set; } = string.Empty;

            public Builder SetOutput(string stepKey)
            {
                Output = stepKey;
                return this;
            }

            public Builder AddStep(PipelineStep.Builder step)
            {
                Steps.Add(step);
                return this;
            }

            public Pipeline Build()
            {
                var steps = Steps.Select(x => x.Build()).ToList();
                var nodes = steps.Select(x => new Node(x)).ToDictionary(x => x.Step.Key!, x => x);
                var output = nodes[Output];
                foreach (var node in nodes.Values)
                {
                    foreach (var edge in node.Step.GetInputs())
                    {
                        if (edge.Value == "$new")
                        {
                            node.Incoming.Add(new Edge(null, edge.Key));
                        }
                        else
                        {
                            var source = nodes[edge.Value];
                            var e = new Edge(source, edge.Key);
                            node.Incoming.Add(e);
                            Precondition.IsNull(source.Outgoing);
                            source.Outgoing = e;
                        }
                    }
                }
                return new Pipeline(output);
            }
        }
    }
}
