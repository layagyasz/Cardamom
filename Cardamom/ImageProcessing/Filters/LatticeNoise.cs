using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    public class LatticeNoise : IFilter
    {
        private static Shader? LATTICE_NOISE_SHADER;

        public struct Settings
        {
            public float Frequency { get; set; } = .002f;
            public float Lacunarity { get; set; } = 2;
            public int Octaves { get; set; } = 6;
            public float Persistence { get; set; } = 0.6f;
            public float Bias { get; set; } = 0.5f;
            public float Amplitude { get; set; } = 1;
            public Vector2 Offset { get; set; }

            public Settings() { }
        }

        public bool InPlace => true;

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
            Precondition.Check(inputs.Count == 1 && inputs.First().Value == output);

            LATTICE_NOISE_SHADER ??= new Shader.Builder().SetCompute("Resources/lattice_noise.comp").Build();

            for (int i = 0; i < _hashLookup.Length; i++)
            {
                LATTICE_NOISE_SHADER.SetInt32($"hash_lookup[{i}]", _hashLookup[i]);
            }
            for (int i = 0; i < _kernel.Length; ++i)
            {
                LATTICE_NOISE_SHADER.SetVector3($"kernel[{i}]", _kernel[i]);
            }
            LATTICE_NOISE_SHADER.SetFloat("frequency", _settings.Frequency);
            LATTICE_NOISE_SHADER.SetFloat("lacunarity", _settings.Lacunarity);
            LATTICE_NOISE_SHADER.SetInt32("octaves", _settings.Octaves);
            LATTICE_NOISE_SHADER.SetFloat("persistence", _settings.Persistence);
            LATTICE_NOISE_SHADER.SetFloat("bias", _settings.Bias);
            LATTICE_NOISE_SHADER.SetFloat("amplitude", _settings.Amplitude);
            LATTICE_NOISE_SHADER.SetVector2("offset", _settings.Offset);

            LATTICE_NOISE_SHADER.SetInt32("channel", (int)channel);
            LATTICE_NOISE_SHADER.DoCompute(output.GetTexture());
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

            public Builder SetOffset(Vector2 offset)
            {
                _settings.Offset = offset;
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
