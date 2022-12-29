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

            var uvSphereSolid = Solid.GenerateUvSphere(1, 64);
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
                    float z = (float)Math.Atan2(vert.Z, Math.Sqrt(vert.X * vert.X + vert.Y * vert.Y));
                    vertices[6 * i + j] =
                        new(
                            vert,
                            Color4.White, 
                            new(
                                (float)(resolution * ((theta + Math.PI) / Math.Tau)), 
                                (float)(resolution * ((z + MathHelper.PiOver2) / Math.PI))));
                }
            }
            var sphereModel = new Model(vertices, resources.GetShader("shader-default"), output![0].GetTexture());

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
