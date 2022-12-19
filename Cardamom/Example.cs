using Cardamom.Graphics;
using Cardamom.Graphics.Camera;
using Cardamom.Graphics.Ui;
using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements;
using Cardamom.ImageProcessing;
using Cardamom.ImageProcessing.Filters;
using Cardamom.ImageProcessing.Pipelines;
using Cardamom.ImageProcessing.Pipelines.Nodes;
using Cardamom.Mathematics.Geometry;
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

            float resolution = 256;
            var random = new Random();
            var rSeed = ConstantValue.Create(0);
            var gSeed = ConstantValue.Create(0);
            var bSeed = ConstantValue.Create(0);
            var noiseFrequency = ConstantValue.Create(0.01f);
            var noiseScale = 
                ConstantValue.Create(new Vector3(MathHelper.TwoPi / resolution, MathHelper.Pi / resolution, 256));
            var noiseSurface = ConstantValue.Create(LatticeNoise.Surface.SPHERE);
            var adjustment = ConstantValue.Create(new Vector4(0,0,0,0));
            var pipeline =
                new Pipeline.Builder()
                    .AddNode(new GeneratorNode.Builder().SetKey("new"))
                    .AddNode(
                        new LatticeNoiseNode.Builder()
                            .SetKey("lattice-noise-r")
                            .SetChannel(Channel.RED)
                            .SetInput("input", "new")
                            .SetParameters(
                                new() 
                                { 
                                    Seed = rSeed,
                                    Frequency = noiseFrequency,
                                    Scale = noiseScale,
                                    Surface = noiseSurface
                                }))
                    .AddNode(
                        new LatticeNoiseNode.Builder()
                            .SetKey("lattice-noise-b")
                            .SetChannel(Channel.BLUE)
                            .SetInput("input", "lattice-noise-r")
                            .SetParameters(
                                new()
                                {
                                    Seed = bSeed,
                                    Frequency = noiseFrequency,
                                    Scale = noiseScale,
                                    Surface = noiseSurface
                                }))
                    .AddNode(
                        new LatticeNoiseNode.Builder()
                            .SetKey("lattice-noise-g")
                            .SetChannel(Channel.GREEN)
                            .SetInput("input", "lattice-noise-b")
                            .SetParameters(
                                new()
                                {
                                    Seed = gSeed,
                                    Frequency = noiseFrequency,
                                    Scale = noiseScale,
                                    Surface = noiseSurface
                                }))
                    .AddNode(
                        new DenormalizeNode.Builder()
                            .SetKey("denormalize")
                            .SetChannel(Channel.RED | Channel.GREEN | Channel.BLUE)
                            .SetInput("input", "lattice-noise-g"))
                    .AddNode(
                        new WaveFormNode.Builder()
                            .SetKey("wave-form-r")
                            .SetChannel(Channel.RED)
                            .SetInput("input", "denormalize")
                            .SetParameters(
                                new()
                                {
                                    WaveType = ConstantValue.Create(WaveForm.WaveType.COSINE),
                                    Amplitude = ConstantValue.Create(-0.5f),
                                    Periodicity = ConstantValue.Create(new Vector2(0, MathHelper.TwoPi)),
                                    Turbulence = ConstantValue.Create(new Vector2(0.172f, 0.172f)),
                                    Scale = ConstantValue.Create(new Vector2(1f / resolution, 1f / resolution))
                                }))
                    .AddNode(
                        new WaveFormNode.Builder()
                            .SetKey("wave-form-b")
                            .SetChannel(Channel.BLUE)
                            .SetInput("input", "wave-form-r")
                            .SetParameters(
                                new()
                                {
                                    WaveType = ConstantValue.Create(WaveForm.WaveType.COSINE),
                                    Amplitude = ConstantValue.Create(-0.5f),
                                    Periodicity = ConstantValue.Create(new Vector2(0, 2 * MathHelper.TwoPi)),
                                    Turbulence = ConstantValue.Create(new Vector2(0.25f, 0.25f)),
                                    Scale = ConstantValue.Create(new Vector2(1f / resolution, 1f / resolution))
                                }))
                    .AddNode(
                        new AdjustNode.Builder()
                            .SetKey("adjusted")
                            .SetChannel(Channel.ALL)
                            .SetInput("input", "wave-form-b")
                            .SetParameters(
                                new()
                                {
                                    Adjustment = adjustment
                                }))
                    .AddNode(
                        new ClassifyNode.Builder()
                            .SetKey("classified")
                            .SetChannel(Channel.ALL)
                            .SetInput("input", "adjusted")
                            .SetParameters(
                                new()
                                {
                                    Classifications =
                                        ConstantValue.Create(
                                            new List<Classify.Classification>()
                                            {
                                                new Classify.Classification()
                                                {
                                                    Color = Color4.White,
                                                    Conditions = new List<Classify.Condition>()
                                                    {
                                                        new Classify.Condition()
                                                        {
                                                            Channel = Channel.RED,
                                                            Maximum = 0.25f
                                                        }
                                                    }
                                                },
                                                new Classify.Classification()
                                                {
                                                    Color = Color4.Blue,
                                                    Conditions = new List<Classify.Condition>()
                                                    {
                                                        new Classify.Condition()
                                                        {
                                                            Channel = Channel.GREEN,
                                                            Maximum = 0.5f
                                                        }
                                                    }
                                                },
                                                new Classify.Classification()
                                                {
                                                    Color = Color4.Lime,
                                                }
                                            })
                                }))
                    .AddNode(
                        new SobelNode.Builder()
                            .SetKey("sobel")
                            .SetChannel(Channel.ALL)
                            .SetInput("input", "denormalize")
                            .SetParameters(new() { Channel = ConstantValue.Create(Channel.GREEN) }))
                    .AddOutput("classified")
                    .AddOutput("sobel")
                    .AddOutput("lattice-noise-r")
                    .Build();
            var canvases = new CachingCanvasProvider(new((int)resolution, (int)resolution), Color4.Black);
            rSeed.Value = random.Next();
            gSeed.Value = random.Next();
            bSeed.Value = random.Next();

            var output = pipeline.Run(canvases);
            output[0].GetTexture().CopyToImage().SaveToFile("example-out.png");
            output[2].GetTexture().CopyToImage().SaveToFile("example-out-single.png");

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

            var uvSphereSolid = Solid.GenerateUvSphere(1, 40);
            VertexArray vertices = new(PrimitiveType.Triangles, 6 * uvSphereSolid.Faces.Length);
            for (int i=0; i<uvSphereSolid.Faces.Length; ++i)
            {
                float leftTheta = 0;
                for (int j=0; j < uvSphereSolid.Faces[i].Vertices.Length; ++j)
                {
                    var vert = uvSphereSolid.Faces[i].Vertices[j];
                    float theta = (float)Math.Atan2(vert.Y, vert.X);
                    // Make sure the tex coords don't backtrack
                    if (j == 0)
                    {
                        leftTheta = theta;
                    }
                    // ... for the rectangles in main body
                    else if (uvSphereSolid.Faces[i].Vertices.Length == 6 && leftTheta - theta > 1)
                    {
                        theta += MathHelper.TwoPi;
                    }
                    // ... for the triangles at the poles
                    else if (uvSphereSolid.Faces[i].Vertices.Length == 3)
                    {
                        if (j == 1 && leftTheta - theta > 1)
                        {
                            theta += MathHelper.TwoPi;
                        }
                        if (j == 2)
                        {
                            theta = leftTheta;
                        }
                    }
                    float z = vert.Z;
                    vertices[6 * i + j] =
                        new(
                            vert,
                            Color4.White, 
                            new((float)(resolution * ((theta + Math.PI) / Math.Tau)), resolution * 0.5f * (z + 1)));
                }
            }
            var sphereModel = new Model(vertices, resources.GetShader("shader-default"), output[0].GetTexture());

            var camera = new SubjectiveCamera3d(1.5f, 1000, new(), new(), 2);
            camera.SetPitch(-MathHelper.PiOver2);
            var sceneController =
                new PassthroughController(
                    new SubjectiveCamera3dController(camera)
                    {
                        KeySensitivity = 0.0005f,
                        MouseWheelSensitivity = 0.1f,
                        PitchRange = new(-MathHelper.Pi, 0),
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
