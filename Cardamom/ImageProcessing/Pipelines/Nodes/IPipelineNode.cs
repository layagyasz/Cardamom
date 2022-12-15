namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public interface IPipelineNode : IKeyed
    {
        bool Inline { get; }

        Dictionary<string, string> GetInputs();
        void Run(Canvas output, Dictionary<string, Canvas> inputs);

        public interface IBuilder : IKeyed
        {
            IPipelineNode Build();
        }
    }
}
