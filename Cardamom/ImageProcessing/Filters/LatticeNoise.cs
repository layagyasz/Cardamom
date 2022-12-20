using Cardamom.Graphics;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class LatticeNoise : IFilter
    {
        private static Shader? s_LatticeNoiseShader;
        private static readonly int s_FrequencyLocation = 0;
        private static readonly int s_LacunarityLocation = 1;
        private static readonly int s_OctavesLocation = 2;
        private static readonly int s_PersistenceLocation = 3;
        private static readonly int s_BiasLocation = 4;
        private static readonly int s_AmplitudeLocation = 5;
        private static readonly int s_OffsetLocation = 6;
        private static readonly int s_SurfaceLocation = 7;
        private static readonly int s_ScaleLocation = 8;
        private static readonly int s_EvaluatorLocation = 9;
        private static readonly int s_InterpolatorLocation = 10;
        private static readonly int s_PreTreatmentLocation = 11;
        private static readonly int s_PostTreatmentLocation = 12;
        private static readonly int s_ChannelLocation = 13;
        private static readonly int s_HashLookupLocation = 14;
        private static readonly int s_KernelLocation = 270;

        public enum Surface
        {
            PLANE = 0,
            CYLINDER = 1,
            SPHERE = 2
        }

        public enum Evaluator
        {
            COSINE = 0,
            COSINE_INVERSE = 1,
            CURL = 2,
            CURVATURE = 3,
            DIVERGENCE = 4,
            DOUBLE_PLANE = 5,
            GRADIENT = 6,
            HILL = 7,
            HILL_AND_SLOPE = 8,
            HYPERBOLIC = 9,
            HYPERBOLIC_PLANES = 10,
            HYPERBOLIC_PLANES_DISPLACED = 11,
            MONKEY_SADDLE = 12,
            PARABOLIC = 13,
            PARABOLIC_COMPOSED = 14,
            PARABOLIC_DISPLACED = 15,
            PARABOLIC_INVERSE = 16,
            REJECTION= 17,
            TRIANGULAR_EDGE = 18,
            VERTICAL_EDGE = 19,
            VERTICAL_EDGE_DISPLACED = 20,
            VERTICAL_EDGE_INVERSE = 21,
            VERTICAL_EDGE_INVERSE_DISPLACED = 22
        }

        public enum Interpolator
        {
            COSINE = 0,
            EPANECHNIKOV = 1,
            HERMITE = 2,
            HERMITE_DISPLACED = 3,
            HERMITE_QUINTIC = 4,
            HERMITE_SIGMOID = 5,
            LINEAR = 6,
            PYRAMID = 7,
            QUARTIC = 8,
            SIGMOID = 9,
            TRICUBE = 10,
            TRIWEIGHT = 11,
        }

        public enum Treatment
        {
            NONE = 0,
            BILLOW = 1,
            RIG = 2,
            SEMIRIG = 3
        }

        public struct Settings
        {
            public float Frequency { get; set; } = .002f;
            public float Lacunarity { get; set; } = 2;
            public int Octaves { get; set; } = 6;
            public float Persistence { get; set; } = 0.6f;
            public float Bias { get; set; } = 0.5f;
            public float Amplitude { get; set; } = 1;
            public Vector3 Offset { get; set; }
            public Surface Surface { get; set; } = Surface.PLANE;
            public Vector3 Scale { get; set; } = new(1, 1, 1);
            public Evaluator Evaluator { get; set; } = Evaluator.GRADIENT;
            public Interpolator Interpolator { get; set; } = Interpolator.LINEAR;
            public Treatment PreTreatment { get; set; } = Treatment.NONE;
            public Treatment PostTreatment { get; set; } = Treatment.NONE;

            public Settings() { }
        }

        private readonly int[] _hashLookup;
        private readonly Vector3[] _kernel;
        private readonly Settings _settings;

        private LatticeNoise(int[] hashLookup, Vector3[] kernel, Settings settings)
        {
            _hashLookup = hashLookup;
            _kernel = kernel;
            _settings = settings;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_LatticeNoiseShader ??= new Shader.Builder().SetCompute("Resources/lattice_noise.comp").Build();

            s_LatticeNoiseShader.SetFloat(s_FrequencyLocation, _settings.Frequency);
            s_LatticeNoiseShader.SetFloat(s_LacunarityLocation, _settings.Lacunarity);
            s_LatticeNoiseShader.SetInt32(s_OctavesLocation, _settings.Octaves);
            s_LatticeNoiseShader.SetFloat(s_PersistenceLocation, _settings.Persistence);
            s_LatticeNoiseShader.SetFloat(s_BiasLocation, _settings.Bias);
            s_LatticeNoiseShader.SetFloat(s_AmplitudeLocation, _settings.Amplitude);
            s_LatticeNoiseShader.SetVector3(s_OffsetLocation, _settings.Offset);
            s_LatticeNoiseShader.SetInt32(s_SurfaceLocation, (int)_settings.Surface);
            s_LatticeNoiseShader.SetVector3(s_ScaleLocation, _settings.Scale);
            s_LatticeNoiseShader.SetInt32(s_EvaluatorLocation, (int)_settings.Evaluator);
            s_LatticeNoiseShader.SetInt32(s_InterpolatorLocation, (int)_settings.Interpolator);
            s_LatticeNoiseShader.SetInt32(s_PreTreatmentLocation, (int)_settings.PreTreatment);
            s_LatticeNoiseShader.SetInt32(s_PostTreatmentLocation, (int)_settings.PostTreatment);
            s_LatticeNoiseShader.SetInt32Array(s_HashLookupLocation, _hashLookup);
            s_LatticeNoiseShader.SetVector3Array(s_KernelLocation, _kernel);

            s_LatticeNoiseShader.SetInt32(s_ChannelLocation, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_LatticeNoiseShader.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private int _hashSpace = 256;
            private int _kernelSize = 64;
            private Random? _generator;
            private Settings _settings = new();

            public Builder SetHashSpace(int hashSpace)
            {
                _hashSpace = hashSpace;
                return this;
            }

            public Builder SetKernelSize(int kernelSize)
            {
                _kernelSize = kernelSize;
                return this;
            }

            public Builder SetSeed(int seed)
            {
                _generator = new Random(seed);
                return this;
            }

            public Builder SetGenerator(Random random)
            {
                _generator = random;
                return this;
            }

            public Builder SetFrequency(float frequency)
            {
                _settings.Frequency = frequency;
                return this;
            }

            public Builder SetLacunarity(float lacunarity)
            {
                _settings.Lacunarity = lacunarity;
                return this;
            }

            public Builder SetOctaves(int octaves)
            {
                _settings.Octaves = octaves;
                return this;
            }

            public Builder SetPersistence(float persistence)
            {
                _settings.Persistence = persistence;
                return this;
            }

            public Builder SetBias(float bias)
            {
                _settings.Bias = bias;
                return this;
            }

            public Builder SetAmplitude(float amplitude)
            {
                _settings.Amplitude = amplitude;
                return this;
            }

            public Builder SetOffset(Vector3 offset)
            {
                _settings.Offset = offset;
                return this;
            }

            public Builder SetSurface(Surface surface)
            {
                _settings.Surface = surface;
                return this;
            }

            public Builder SetScale(Vector3 scale)
            {
                _settings.Scale = scale;
                return this;
            }

            public Builder SetEvaluator(Evaluator evaluator)
            {
                _settings.Evaluator = evaluator;
                return this;
            }

            public Builder SetInterpolator(Interpolator interpolator)
            {
                _settings.Interpolator = interpolator;
                return this;
            }

            public Builder SetPreTreatment(Treatment treatment)
            {
                _settings.PreTreatment = treatment;
                return this;
            }

            public Builder SetPostTreatment(Treatment treatment)
            {
                _settings.PostTreatment = treatment;
                return this;
            }

            public IFilter Build()
            {
                var lookup = new int[_hashSpace];
                var kernel = new Vector3[_kernelSize];
                Random generator = _generator!;
                for (int i = 0; i < _hashSpace; ++i)
                {
                    lookup[i] = i;
                }
                for (int i = 0; i < _hashSpace; ++i)
                {
                    var index = generator.Next(0, _hashSpace);
                    (lookup[index], lookup[i]) = (lookup[i], lookup[index]);
                }
                for (int i = 0; i < _kernelSize; ++i)
                {
                    var z = 2 * generator.NextSingle() - 1;
                    var theta = 2 * Math.PI * generator.NextSingle();
                    kernel[i] =
                        new(
                            (float)(Math.Sqrt(1 - z * z) * Math.Cos(theta)),
                            (float)(Math.Sqrt(1 - z * z) * Math.Sin(theta)),
                            z);
                }
                return new LatticeNoise(lookup, kernel, _settings);
            }
        }
    }
}
