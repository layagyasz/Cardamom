using Cardamom.Graphics;
using Cardamom.Graphics.Ui;
using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements;
using Cardamom.ImageProcessing;
using Cardamom.ImageProcessing.Filters;
using Cardamom.ImageProcessing.Pipelines;
using Cardamom.ImageProcessing.Pipelines.Nodes;
using Cardamom.Window;
using OpenTK.Mathematics;
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
            var pipelineTime = new Stopwatch();
            var canvases = new CachingCanvasProvider(new(512, 512), Color4.Black);
            for (int i = 0; i < 10000; ++i)
            {
                if (i % 100 == 0)
                {
                    Console.WriteLine($"### {i}");
                    Console.WriteLine(pipelineTime.ElapsedMilliseconds);
                }
                rSeed.Value = random.Next();
                gSeed.Value = random.Next();
                bSeed.Value = random.Next();

                pipelineTime.Start();
                var output = pipeline.Run(canvases);
                pipelineTime.Stop();

                if (i == 0)
                {
                    output[0].GetTexture().CopyToImage();
                    output[1].GetTexture().CopyToImage();
                }

                canvases.Return(output[0]);
                canvases.Return(output[1]);
            }
            Console.WriteLine(pipelineTime.ElapsedMilliseconds);

            var ui = new UiWindow(window);
            var uiElementFactory =
                new UiElementFactory(
                    GraphicsResources.Builder.ReadFrom("Example/GraphicsResources.json").Build(), SimpleKeyMapper.US);
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

            var screen = 
                new Screen(
                    new Planar.Rectangle(new(), new(800, 600)), 
                    new SecondaryController<Screen>(),
                    new List<UiLayer>()
                    {
                        UiElementFactory.CreatePaneLayer(new List<IRenderable>() { pane }).Item1
                    });
            screen.Initialize();
            ui.UiRoot = screen;
            ui.Start();
        }
    }
}
