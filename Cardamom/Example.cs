﻿using Cardamom.Graphics;
using Cardamom.Graphics.Camera;
using Cardamom.Graphics.Ui;
using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements;
using Cardamom.ImageProcessing;
using Cardamom.ImageProcessing.Filters;
using Cardamom.ImageProcessing.Pipelines;
using Cardamom.ImageProcessing.Pipelines.Nodes;
using Cardamom.Mathematics;
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

            var random = new Random();
            var rSeed = ConstantValue.Create(0);
            var gSeed = ConstantValue.Create(0);
            var bSeed = ConstantValue.Create(0);
            var adjustment = ConstantValue.Create(new Vector4(0,0,0,0));
            var pipeline =
                new Pipeline.Builder()
                    .AddNode(new GeneratorNode.Builder().SetKey("new"))
                    .AddNode(
                        new LatticeNoiseNode.Builder()
                            .SetKey("lattice-noise-r")
                            .SetChannel(Channel.RED)
                            .SetInput("input", "new")
                            .SetParameters(new() { Seed = rSeed }))
                    .AddNode(
                        new LatticeNoiseNode.Builder()
                            .SetKey("lattice-noise-b")
                            .SetChannel(Channel.BLUE)
                            .SetInput("input", "lattice-noise-r")
                            .SetParameters(new() { Seed = bSeed }))
                    .AddNode(
                        new LatticeNoiseNode.Builder()
                            .SetKey("lattice-noise-g")
                            .SetChannel(Channel.GREEN)
                            .SetInput("input", "lattice-noise-b")
                            .SetParameters(new() { Seed = gSeed }))
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
                                    Periodicity = ConstantValue.Create(new Vector2(0, 0.0122f)),
                                    Turbulence = ConstantValue.Create(new Vector2(178, 178))
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
                                    Periodicity = ConstantValue.Create(new Vector2(0, .0244f)),
                                    Turbulence = ConstantValue.Create(new Vector2(256, 256))
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
                    .Build();
            var resolution = 512;
            var canvases = new CachingCanvasProvider(new(resolution, resolution), Color4.Black);
            rSeed.Value = random.Next();
            gSeed.Value = random.Next();
            bSeed.Value = random.Next();

            var output = pipeline.Run(canvases);
            output[0].GetTexture().CopyToImage().SaveToFile("example-out.png");

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

            var icosphereSolid = Solid.GenerateIcosphere(1, 12);
            VertexArray vertices = new(PrimitiveType.Triangles, 3 * icosphereSolid.Faces.Length);
            for (int i=0; i<icosphereSolid.Faces.Length; ++i)
            {
                for (int j=0; j<3; ++j)
                {
                    var vert = icosphereSolid.Faces[i].Vertices[j];
                    float p = (float)(Math.Sqrt(vert.X * vert.X + vert.Y * vert.Y) * Math.Atan2(vert.Y, vert.X));
                    float z = vert.Z;
                    vertices[3 * i + j] =
                        new(
                            vert,
                            Color4.White, 
                            new((float)(resolution * ((p + Math.PI) / Math.Tau)), resolution * 0.5f * (z + 1)));
                }
            }
            var cubeModel = new Model(vertices, resources.GetShader("shader-default"), output[0].GetTexture());

            var camera = new SubjectiveCamera3d(1.5f, 1000, new(), new(), 2);
            camera.SetPitch(-MathHelper.PiOver2);
            var sceneController =
                new PassthroughController(
                    new SubjectiveCamera3dController(camera)
                    {
                        KeySensitivity = 0.0005f,
                        MouseWheelSensitivity = 0.1f,
                        PitchRange = new(-MathHelper.Pi, 0),
                        DistanceRange = new(2, 10)
                    });
            var scene =
                new Scene(
                    new Vector3(800, 600, 0),
                    sceneController,
                    camera,
                    new List<IRenderable>() { cubeModel });

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
