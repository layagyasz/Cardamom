using System.Text.Json.Serialization;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public interface IPipelineNode : IKeyed
    {
        bool External { get; }
        bool Inline { get; }

        Dictionary<string, string> GetInputs();
        Canvas Run(Canvas? output, Dictionary<string, Canvas> inputs);

        [JsonDerivedType(typeof(AdjustNode.Builder), "Adjust")]
        [JsonDerivedType(typeof(ClassifyNode.Builder), "Classify")]
        [JsonDerivedType(typeof(CombineNode.Builder), "Combine")]
        [JsonDerivedType(typeof(CylinderizeNode.Builder), "Cylinderize")]
        [JsonDerivedType(typeof(DenormalizeNode.Builder), "Denormalize")]
        [JsonDerivedType(typeof(GeneratorNode.Builder), "Generator")]
        [JsonDerivedType(typeof(GradientNode.Builder), "Gradient")]
        [JsonDerivedType(typeof(InputNode.Builder), "Input")]
        [JsonDerivedType(typeof(LatticeNoiseNode.Builder), "LatticeNoise")]
        [JsonDerivedType(typeof(SobelNode.Builder), "Sobel")]
        [JsonDerivedType(typeof(SpherizeNode.Builder), "Spherize")]
        [JsonDerivedType(typeof(WaveFormNode.Builder), "WaveForm")]
        [JsonDerivedType(typeof(WhiteNoiseNode.Builder), "WhiteNoise")]
        public interface IBuilder : IKeyed
        {
            IPipelineNode Build();
        }
    }
}
