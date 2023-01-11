using System.Text.Json.Serialization;

namespace Cardamom.Randoms
{
    [JsonDerivedType(typeof(ConstantSampler), "constant")]
    [JsonDerivedType(typeof(ExponentialSampler), "exponential")]
    [JsonDerivedType(typeof(GammaSampler), "gamma")]
    [JsonDerivedType(typeof(NormalSampler), "normal")]
    [JsonDerivedType(typeof(ParetoSampler), "pareto")]
    [JsonDerivedType(typeof(ReciprocalSampler), "reciprocal")]
    [JsonDerivedType(typeof(UniformSampler), "uniform")]
    public interface ISampler : IGenerator<float>, Generic.IGenerator 
    { 
        object Generic.IGenerator.Generate(Random random)
        {
            return ((IGenerator<float>)this).Generate(random);
        }
    }
}
