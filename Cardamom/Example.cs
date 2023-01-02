using Cardamom.Collections;
using Cardamom.Graphics;
using Cardamom.Graphics.Camera;
using Cardamom.Graphics.Ui;
using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements;
using Cardamom.ImageProcessing;
using Cardamom.ImageProcessing.Filters;
using Cardamom.ImageProcessing.Pipelines;
using Cardamom.ImageProcessing.Pipelines.Nodes;
using Cardamom.Mathematics;
using Cardamom.Mathematics.Coordinates;
using Cardamom.Mathematics.Coordinates.Projections;
using Cardamom.Mathematics.Geometry;
using Cardamom.Window;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

namespace Cardamom
{
    public static class Example
    {
        public static void Main()
        {
            var window = new RenderWindow("Cardamom - Example", new Vector2i(800, 600));

            float resolution = 512;
            var random = new Random();
            var seed = ConstantValue.Create(random.Next());
            var noiseFrequency = ConstantValue.Create(0.01f);
            var noiseScale = 
                ConstantValue.Create(new Vector3(MathHelper.TwoPi / resolution, MathHelper.Pi / resolution, 256));
            var noiseSurface = ConstantValue.Create(LatticeNoise.Surface.Sphere);
            var noiseEvaluator = ConstantValue.Create(LatticeNoise.Evaluator.VerticalEdgeInverse);
            var noiseInterpolator = ConstantValue.Create(LatticeNoise.Interpolator.Linear);
            var noisePreTreatment = ConstantValue.Create(LatticeNoise.Treatment.SemiRig);
            var noisePostTreatment = ConstantValue.Create(LatticeNoise.Treatment.Billow);
            var pipeline =
                new Pipeline.Builder()
                    .AddNode(new GeneratorNode.Builder().SetKey("new"))
                    .AddNode(
                        new LatticeNoiseNode.Builder()
                            .SetKey("lattice-noise")
                            .SetChannel(Channel.Red | Channel.Green | Channel.Blue)
                            .SetInput("input", "new")
                            .SetParameters(
                                new() 
                                { 
                                    Seed = seed,
                                    Frequency = noiseFrequency,
                                    Scale = noiseScale,
                                    Surface = noiseSurface,
                                    Evaluator = noiseEvaluator,
                                    Interpolator = noiseInterpolator,
                                    PreTreatment = noisePreTreatment,
                                    PostTreatment = noisePostTreatment
                                }))
                    .AddNode(
                        new DenormalizeNode.Builder()
                            .SetKey("denormalize")
                            .SetChannel(Channel.Red | Channel.Green | Channel.Blue)
                            .SetInput("input", "lattice-noise"))
                    .AddOutput("lattice-noise")
                    .Build();
            var canvases = new CachingCanvasProvider(new((int)resolution, (int)resolution), Color4.Black);
            var output = pipeline.Run(canvases);

            var ui = new UiWindow(window);
            ui.Bind(new MouseListener());
            ui.Bind(
                new KeyboardListener(SimpleKeyMapper.Us, new Keys[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down }));
            var resources = GraphicsResources.Builder.ReadFrom("Example/GraphicsResources.json").Build();
            var uiElementFactory = new UiElementFactory(resources);
            var pane = uiElementFactory.CreatePane("example-base-class").Item1;
            var options = new List<IUiElement>();
            for (int i = 0; i < 20; ++i)
            {
                options.Add(
                    uiElementFactory.CreateSelectOption("example-select-option-class", i, $"Button #{i}").Item1);
            }

            var select = 
                uiElementFactory.CreateSelect<int>("example-select-class", "example-select-drop-box-class", options);
            select.Item2.ValueChanged += (s, e) => Console.WriteLine(e);
            pane.Add(select.Item1);

            var text = uiElementFactory.CreateTextInput("example-row-class", new(0, select.Item1.Size.Y, 0));
            text.Item2.ValueChanged += (s, e) => Console.WriteLine(e);
            pane.Add(text.Item1);

            var uvSphereSolid = Solid<Spherical3>.GenerateSphericalUvSphere(1, 64);
            VertexArray vertices = new(PrimitiveType.Triangles, 6 * uvSphereSolid.Faces.Length);
            var projection = new CylindricalProjection.Spherical();
            for (int i=0; i<uvSphereSolid.Faces.Length; ++i)
            {
                for (int j=0; j < uvSphereSolid.Faces[i].Vertices.Length; ++j)
                {
                    var vert = uvSphereSolid.Faces[i].Vertices[j];
                    var texCoords = resolution * new Vector2(0.5f, 1) * projection.Project(vert);
                    vertices[6 * i + j] = new(vert.AsCartesian(), Color4.White, texCoords);
                }
            }
            var sphereModel =
                new InteractiveModel(
                    new(vertices, resources.GetShader("shader-default"), output![0].GetTexture()), 
                    new Sphere(new(), 1),
                    new DebugController());
            sphereModel.Controller.Clicked += (s, e) => Console.WriteLine(e.Position.Length);

            List<HyperVector> field = new();
            KdTree<HyperVector>.Builder kdTreeBuilder = new();
            kdTreeBuilder.SetCardinality(3);
            for (int i=0; i<1000; ++i)
            {
                var point = GeneratePoint(random);
                field.Add(point);
                kdTreeBuilder.Add(point, point);
            }
            var kdTree = kdTreeBuilder.Build();

            Stopwatch kdTime = new();
            kdTime.Start();
            for (int i = 0; i < 100000; ++i)
            {
                var point = GeneratePoint(random);
                kdTree.GetClosest(point);
            }
            kdTime.Stop();
            Console.WriteLine(kdTime.ElapsedMilliseconds);

            Stopwatch normalTime = new();
            normalTime.Start();
            for (int i = 0; i < 100000; ++i)
            {
                var point = GeneratePoint(random);
                var fromNormal = GetClosest(point, field);
            }
            normalTime.Stop();
            Console.WriteLine(normalTime.ElapsedMilliseconds);

            var camera = new SubjectiveCamera3d(1.5f, 1000, new(), 2);
            var sceneController =
                new PassthroughController(
                    new SubjectiveCamera3dController(camera)
                    {
                        KeySensitivity = 0.0005f,
                        MouseWheelSensitivity = 0.1f,
                        PitchRange = new(-MathHelper.PiOver2 + 0.01f, MathHelper.PiOver2 - 0.01f),
                        DistanceRange = new(1.1f, 10)
                    });
            var scene =
                new Scene(
                    new Vector3(800, 600, 0),
                    sceneController,
                    camera,
                    new List<IRenderable>() { sphereModel });

            var screen = 
                new SceneScreen(
                    new Mathematics.Geometry.Rectangle(new(), new(800, 600)),
                    new SceneScreenController(),
                    new List<UiGroupLayer>()
                    {
                        UiElementFactory.CreatePaneLayer(new List<IRenderable>() { pane }).Item1
                    },
                    scene);
            screen.Initialize();
            ui.UiRoot = screen;
            ui.Start();
        }

        private static HyperVector GeneratePoint(Random random)
        {
            return new HyperVector(1000 * random.NextSingle(), 1000 * random.NextSingle(), 1000 * random.NextSingle());
        }

        private static HyperVector GetClosest(HyperVector point, IEnumerable<HyperVector> field)
        {
            return field.ArgMin(x => HyperVector.DistanceSquared(x, point));
        }
    }
}
