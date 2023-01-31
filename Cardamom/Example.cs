using Cardamom.Graphics;
using Cardamom.Graphics.Camera;
using Cardamom.ImageProcessing;
using Cardamom.ImageProcessing.Filters;
using Cardamom.ImageProcessing.Pipelines;
using Cardamom.ImageProcessing.Pipelines.Nodes;
using Cardamom.Mathematics.Coordinates;
using Cardamom.Mathematics.Coordinates.Projections;
using Cardamom.Mathematics.Geometry;
using Cardamom.Ui;
using Cardamom.Ui.Controller;
using Cardamom.Ui.Controller.Element;
using Cardamom.Ui.Elements;
using Cardamom.Utils.Suppliers;
using Cardamom.Window;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom
{
    public static class Example
    {
        public static void Main()
        {
            var window = new RenderWindow("Cardamom - Example", new Vector2i(800, 600));

            int resolution = 2048;
            var canvases = new CachingCanvasProvider(new((int)resolution, (int)resolution), Color4.Black);
            var random = new Random();
            var seed = ConstantSupplier<int>.Create(random.Next());
            var noiseFrequency = ConstantSupplier<float>.Create(2f);
            var noiseEvaluator = ConstantSupplier<LatticeNoise.Evaluator>.Create(LatticeNoise.Evaluator.Gradient);
            var noiseInterpolator =
            ConstantSupplier<LatticeNoise.Interpolator>.Create(LatticeNoise.Interpolator.HermiteQuintic);
            var noisePreTreatment = ConstantSupplier<LatticeNoise.Treatment>.Create(LatticeNoise.Treatment.None);
            var noisePostTreatment = ConstantSupplier<LatticeNoise.Treatment>.Create(LatticeNoise.Treatment.None);
            var pipeline =
                new Pipeline.Builder()
                    .AddNode(new GeneratorNode.Builder().SetKey("new"))
                    .AddNode(
                        new GradientNode.Builder()
                            .SetKey("gradient")
                            .SetChannel(Channel.Red | Channel.Green)
                            .SetInput("input", "new")
                            .SetParameters(
                                new GradientNode.Parameters()
                                { 
                                    Scale = ConstantSupplier<Vector2>.Create(
                                        new Vector2(1f / resolution, 1f / resolution)),
                                    Gradient = ConstantSupplier<Matrix4x2>.Create(
                                        new Matrix4x2(new(1, 0), new(0, 1), new(), new()))
                                }))
                    .AddNode(
                        new SpherizeNode.Builder()
                            .SetKey("spherize")
                            .SetChannel(Channel.All)
                            .SetInput("input", "gradient"))
                    .AddNode(
                        new LatticeNoiseNode.Builder()
                            .SetKey("lattice-noise")
                            .SetChannel(Channel.Color)
                            .SetInput("input", "spherize")
                            .SetParameters(
                                new()
                                {
                                    Seed = seed,
                                    Frequency = noiseFrequency,
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
                            .SetInput("input", "denormalize"))
                    .AddOutput("denormalize")
                    .AddOutput("sobel")
                    .Build();
            var output = pipeline.Run(canvases);

            var ui = new UiWindow(window);
            ui.Bind(new MouseListener());
            ui.Bind(
                new KeyboardListener(SimpleKeyMapper.Us, new Keys[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down }));
            var resources = GameResources.Builder.ReadFrom("Example/GraphicsResources.json").Build();
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
                        "example-row-class", elements, new NoOpElementController<UiSerialContainer>()));
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
                    vertices[6 * i + j] = new(c, Color4.White, texCoords, c.Normalized(), texCoords, texCoords);
                }
            }
            var sphereModel =
                new InteractiveModel<VertexLit3>(
                    new Model<VertexLit3>(
                        new(vertices, PrimitiveType.Triangles), 
                        resources.GetShader("shader-simple-light"), 
                        new(
                            Texture.Create(new(resolution, resolution), Color4.White), 
                            output[1].GetTexture(),
                            Texture.Create(new(resolution, resolution), new(1f, 128f, 0, 0.25f)))), 
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
                new BasicScene(
                    new Vector3(800, 600, 0),
                    sceneController,
                    camera,
                    new List<IRenderable>() { sphereModel });

            var screen = 
                new SceneScreen(
                    new NoOpController<Screen>(),
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
