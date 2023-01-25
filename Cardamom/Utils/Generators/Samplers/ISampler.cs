using System.Text.Json.Serialization;

namespace Cardamom.Utils.Generators.Samplers
{
    [JsonDerivedType(typeof(ConstantSampler), "Constant")]
    [JsonDerivedType(typeof(ExponentialSampler), "Exponential")]
    [JsonDerivedType(typeof(GammaSampler), "Gamma")]
    [JsonDerivedType(typeof(NormalSampler), "Normal")]
    [JsonDerivedType(typeof(ParetoSampler), "Pareto")]
    [JsonDerivedType(typeof(ReciprocalSampler), "Reciprocal")]
    [JsonDerivedType(typeof(UniformSampler), "Uniform")]
    public interface ISampler : IGenerator<float> { }
}
