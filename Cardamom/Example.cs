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

            float resolution = 2048;
            var canvases = new CachingCanvasProvider(new((int)resolution, (int)resolution), Color4.Black);

            var testPipeline =
                new Pipeline.Builder()
                    .AddNode(new GeneratorNode.Builder().SetKey("new"))
                    .AddNode(new GeneratorNode.Builder().SetKey("new-2"))
                    .AddNode(
                        new GradientNode.Builder()
                            .SetKey("gradient-1")
                            .SetChannel(Channel.Color)
                            .SetInput("input", "new")
                            .SetParameters(new GradientNode.Parameters()
                            {
                                Scale = ConstantValue.Create(new Vector2(1f / resolution, 1f / resolution))
                            }))
                    .AddNode(
                        new GradientNode.Builder()
                            .SetKey("gradient-2")
                            .SetChannel(Channel.Color)
                            .SetInput("input", "new-2")
                            .SetParameters(new GradientNode.Parameters()
                            {
                                Scale = ConstantValue.Create(new Vector2(1f / resolution, 1f / resolution)),
                                Factor = ConstantValue.Create(new Matrix4x2(new(1, 0), new(0, 1), new(), new()))
                            }))
                    .AddNode(
                        new CombineNode.Builder()
                            .SetKey("combine")
                            .SetChannel(Channel.Color)
                            .SetInput("left", "gradient-1")
                            .SetInput("right", "gradient-2")
                            .SetParameters(
                                new CombineNode.Parameters()
                                { 
                                    LeftFactor = ConstantValue.Create(new Vector4(1, 1, 1, 0)),
                                    RightFactor = ConstantValue.Create(new Vector4(-1, -1, -1, 0)),
                                }))
                    .AddOutput("combine")
                    .AddOutput("gradient-1")
                    .Build();
            var testOutput = testPipeline.Run(canvases);
            testOutput[0].GetTexture().CopyToImage().SaveToFile("test-combine.png");
            testOutput[1].GetTexture().CopyToImage().SaveToFile("test-gradient.png");

            var random = new Random();
            var seed = ConstantValue.Create(random.Next());
            var noiseFrequency = ConstantValue.Create(0.01f);
            var noiseScale = 
                ConstantValue.Create(new Vector3(MathHelper.TwoPi / resolution, MathHelper.Pi / resolution, 256));
            var noiseSurface = ConstantValue.Create(LatticeNoise.Surface.Sphere);
            var noiseEvaluator = ConstantValue.Create(LatticeNoise.Evaluator.Gradient);
            var noiseInterpolator = ConstantValue.Create(LatticeNoise.Interpolator.HermiteQuintic);
            var noisePreTreatment = ConstantValue.Create(LatticeNoise.Treatment.None);
            var noisePostTreatment = ConstantValue.Create(LatticeNoise.Treatment.None);
            var pipeline =
                new Pipeline.Builder()
                    .AddNode(new GeneratorNode.Builder().SetKey("new"))
                    .AddNode(
                        new LatticeNoiseNode.Builder()
                            .SetKey("lattice-noise")
                            .SetChannel(Channel.Color)
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
                            .SetChannel(Channel.Color)
                            .SetInput("input", "lattice-noise"))
                    .AddNode(
                        new SobelNode.Builder()
                            .SetKey("sobel")
                            .SetChannel(Channel.Red)
                            .SetInput("input", "lattice-noise"))
                    .AddOutput("lattice-noise")
                    .AddOutput("sobel")
                    .Build();
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

            var text = uiElementFactory.CreateTextInput("example-text-input-class", new(0, select.Item1.Size.Y, 0));
            text.Item2.ValueChanged += (s, e) => Console.WriteLine(e);
            pane.Add(text.Item1);

            var rows = new List<IUiElement>();
            for (int i=0; i<5; ++i)
            {
                var elements = new List<IUiElement>();
                for (int j=0; j<5; ++j)
                {
                    elements.Add(
                        uiElementFactory.CreateTextButton(
                            "example-cell-class", string.Format($"{(char)(i + 65)}-{j}")).Item1);
                }
                rows.Add(
                    uiElementFactory.CreateTableRow(
                        "example-row-class", elements, new NoOpController<UiSerialContainer>()));
            }
            var table = 
                uiElementFactory.CreateTable(
                    "example-table-class", 
                    rows, 
                    0, 
                    new(0, text.Item1.Position.Y + text.Item1.Size.Y, 0));
            pane.Add(table.Item1);

            var uvSphereSolid = Solid<Spherical3>.GenerateSphericalUvSphere(1, 64);
            VertexLit3[] vertices = new VertexLit3[6 * uvSphereSolid.Faces.Length];
            var projection = new CylindricalProjection.Spherical();
            for (int i=0; i<uvSphereSolid.Faces.Length; ++i)
            {
                for (int j=0; j < uvSphereSolid.Faces[i].Vertices.Length; ++j)
                {
                    var vert = uvSphereSolid.Faces[i].Vertices[j];
                    var texCoords = resolution * new Vector2(0.5f, 1) * projection.Project(vert);
                    var c = vert.AsCartesian();
                    vertices[6 * i + j] = new(c, Color4.White, texCoords, c.Normalized(), texCoords);
                }
            }
            var sphereModel =
                new InteractiveModel<VertexLit3>(
                    new Model<VertexLit3>(
                        vertices, 
                        PrimitiveType.Triangles, 
                        resources.GetShader("shader-simple-light"), 
                        output![0].GetTexture(),
                        output![1].GetTexture()), 
                    new Sphere(new(), 1),
                    new DebugController());
            sphereModel.Controller.Clicked += (s, e) => Console.WriteLine(e.Position.Length);

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
    }
}
