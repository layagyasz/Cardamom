using System.Text.Json.Serialization;

namespace Cardamom.Utils.Generators.Samplers
{
    [JsonDerivedType(typeof(ConstantSampler), "constant")]
    [JsonDerivedType(typeof(ExponentialSampler), "exponential")]
    [JsonDerivedType(typeof(GammaSampler), "gamma")]
    [JsonDerivedType(typeof(NormalSampler), "normal")]
    [JsonDerivedType(typeof(ParetoSampler), "pareto")]
    [JsonDerivedType(typeof(ReciprocalSampler), "reciprocal")]
    [JsonDerivedType(typeof(UniformSampler), "uniform")]
    public interface ISampler : IGenerator<float> { }
}
