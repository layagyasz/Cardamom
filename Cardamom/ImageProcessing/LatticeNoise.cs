using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing
{
    public class LatticeNoise
    {
        private static Shader? LATTICE_NOISE_SHADER;

        public struct Settings
        {
            public float Frequency { get; set; } = .002f;
            public float Lacunarity { get; set; } = 2;
            public int Octaves { get; set; } = 6;
            public float Persistence { get; set; } = 0.6f;
            public float Bias { get; set; } = 0.5f;
            public float Factor { get; set; } = 1;

            public Settings() { }
        }

        private readonly int[] _hashLookup;
        private readonly Vector3[] _vectors;
        private readonly Settings _settings;

        private LatticeNoise(int[] hashLookup, Vector3[] vectors, Settings settings)
        {
            _hashLookup = hashLookup;
            _vectors = vectors;
            _settings = settings;
        }

        public void Render(Texture texture, Vector2 offset, Color4 color)
        {
            LATTICE_NOISE_SHADER ??= new Shader.Builder().SetCompute("Resources/lattice_noise.comp").Build();

            for (int i=0; i<_hashLookup.Length; i++)
            {
                LATTICE_NOISE_SHADER.SetInt32($"hash_lookup[{i}]", _hashLookup[i]);
                LATTICE_NOISE_SHADER.SetVector3($"vector[{i}]", _vectors[i]);
            }
            LATTICE_NOISE_SHADER.SetFloat("frequency", _settings.Frequency);
            LATTICE_NOISE_SHADER.SetFloat("lacunarity", _settings.Lacunarity);
            LATTICE_NOISE_SHADER.SetInt32("octaves", _settings.Octaves);
            LATTICE_NOISE_SHADER.SetFloat("persistence", _settings.Persistence);
            LATTICE_NOISE_SHADER.SetFloat("bias", _settings.Bias);
            LATTICE_NOISE_SHADER.SetFloat("factor", _settings.Factor);
            LATTICE_NOISE_SHADER.SetVector2("offset", offset);
            LATTICE_NOISE_SHADER.SetColor("color", color);

            texture.BindImage();
            LATTICE_NOISE_SHADER.DoCompute(texture.Size);
            Texture.UnbindImage();
        }

        public class Builder
        {
            private int _hashSpace = 128;
            private Random? _generator;
            private Settings _settings = new();

            public Builder SetHashSpace(int hashSpace)
            {
                _hashSpace = hashSpace;
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

            public Builder SetFactor(float factor)
            {
                _settings.Factor = factor;
                return this;
            }

            public LatticeNoise Build()
            {
                var lookup = new int[_hashSpace];
                var vectors = new Vector3[_hashSpace];
                Random generator = _generator!;
                for (int i = 0; i < _hashSpace; ++i)
                {
                    lookup[i] = i;
                }
                for (int i=0; i<_hashSpace;++i)
                {
                    // Shuffle lookup
                    var index = generator.Next(0, _hashSpace);
                    (lookup[index], lookup[i]) = (lookup[i], lookup[index]);

                    // Generate random unit vector
                    var z = 2 * generator.NextSingle() - 1;
                    var theta = 2 * Math.PI * generator.NextSingle();
                    vectors[i] = 
                        new(
                            (float)(Math.Sqrt(1 - z * z) * Math.Cos(theta)),
                            (float)(Math.Sqrt(1 - z * z) * Math.Sin(theta)),
                            z);
                }
                return new(lookup, vectors, _settings);
            }
        }
    }
}
