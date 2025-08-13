using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class LatticeNoise : IFilter
    {
        private static readonly Vector2i s_LocalGroupSize = new(32, 32);

        private static ComputeShader? s_LatticeNoiseShader;
        private static readonly int s_FrequencyLocation = 0;
        private static readonly int s_LacunarityLocation = 1;
        private static readonly int s_OctavesLocation = 2;
        private static readonly int s_PersistenceLocation = 3;
        private static readonly int s_AmplitudeLocation = 4;
        private static readonly int s_EvaluatorLocation = 5;
        private static readonly int s_InterpolatorLocation = 6;
        private static readonly int s_PreTreatmentLocation = 7;
        private static readonly int s_PostTreatmentLocation = 8;
        private static readonly int s_SeedLocation = 9;
        private static readonly int s_ChannelLocation = 10;

        public enum Evaluator
        {
            Cosine = 0,
            CosineInverse = 1,
            Curl = 2,
            Curvature = 3,
            Divergence = 4,
            DoublePlane = 5,
            Gradient = 6,
            Hill = 7,
            HillAndSlope = 8,
            Hyperbolic = 9,
            HyperbolicPlanes = 10,
            HyperbolicPlanesDisplaced = 11,
            MonkeySaddle = 12,
            Parabolic = 13,
            ParabolicComposed = 14,
            ParabolicDisplaced = 15,
            ParabolicInverse = 16,
            Rejection= 17,
            TriangularEdge = 18,
            VerticalEdge = 19,
            VerticalEdgeDisplaced = 20,
            VerticalEdgeInverse = 21,
            VerticalEdgeInverseDisplaced = 22
        }

        public enum Interpolator
        {
            Cosine = 0,
            Epanechnikov = 1,
            Hermite = 2,
            HermiteDisplaced = 3,
            HermiteQuintic = 4,
            HermiteSigmoid = 5,
            Linear = 6,
            Pyramid = 7,
            Quartic = 8,
            Sigmoid = 9,
            Tricube = 10,
            Triweight = 11,
        }

        public enum Treatment
        {
            None = 0,
            Billow = 1,
            Ridge = 2,
            SemiBillow = 3,
            SemiRidge = 4
        }

        public class Settings
        {
            public Vector3 Frequency { get; set; } = new(1, 1, 1);
            public Vector3 Lacunarity { get; set; } = new(2, 2, 2);
            public int Octaves { get; set; } = 6;
            public float Persistence { get; set; } = 0.6f;
            public float Amplitude { get; set; } = 1;
            public Evaluator Evaluator { get; set; } = Evaluator.Gradient;
            public Interpolator Interpolator { get; set; } = Interpolator.HermiteQuintic;
            public Treatment PreTreatment { get; set; } = Treatment.None;
            public Treatment PostTreatment { get; set; } = Treatment.None;
            public int Seed { get; set; }

            public Settings() { }
        }

        private readonly Settings _settings;

        private LatticeNoise(Settings settings)
        {
            _settings = settings;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_LatticeNoiseShader ??=
                ComputeShader.FromFile("Resources/ImageProcessing/Filters/lattice_noise.comp", s_LocalGroupSize);

            s_LatticeNoiseShader.SetVector3(s_FrequencyLocation, _settings.Frequency);
            s_LatticeNoiseShader.SetVector3(s_LacunarityLocation, _settings.Lacunarity);
            s_LatticeNoiseShader.SetInt32(s_OctavesLocation, _settings.Octaves);
            s_LatticeNoiseShader.SetFloat(s_PersistenceLocation, _settings.Persistence);
            s_LatticeNoiseShader.SetFloat(s_AmplitudeLocation, _settings.Amplitude);
            s_LatticeNoiseShader.SetInt32(s_EvaluatorLocation, (int)_settings.Evaluator);
            s_LatticeNoiseShader.SetInt32(s_InterpolatorLocation, (int)_settings.Interpolator);
            s_LatticeNoiseShader.SetInt32(s_PreTreatmentLocation, (int)_settings.PreTreatment);
            s_LatticeNoiseShader.SetInt32(s_PostTreatmentLocation, (int)_settings.PostTreatment);
            s_LatticeNoiseShader.SetInt32(s_SeedLocation, _settings.Seed);

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
            private Settings _settings = new();

            public Builder SetSeed(int seed)
            {
                _settings.Seed = seed;
                return this;
            }

            public Builder SetFrequency(Vector3 frequency)
            {
                _settings.Frequency = frequency;
                return this;
            }

            public Builder SetLacunarity(Vector3 lacunarity)
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

            public Builder SetAmplitude(float amplitude)
            {
                _settings.Amplitude = amplitude;
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
                return new LatticeNoise(_settings);
            }
        }
    }
}
