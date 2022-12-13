using Cardamom.Graphics;
using Cardamom.Graphics.Ui;
using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements;
using Cardamom.ImageProcessing;
using Cardamom.ImageProcessing.Filters;
using Cardamom.ImageProcessing.Pipelines;
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
            var pipeline =
                new Pipeline.Builder()
                    .SetOutput("lattice-noise-g")
                    .AddStep(
                        new PipelineStep.Builder()
                            .SetKey("lattice-noise-r")
                            .SetChannel(Channel.RED)
                            .SetType(typeof(LatticeNoise.Builder))
                            .SetInput("input", "$new")
                            .SetParameter("Seed", rSeed))
                    .AddStep(
                        new PipelineStep.Builder()
                            .SetKey("wave-form-r")
                            .SetChannel(Channel.RED)
                            .SetType(typeof(WaveForm.Builder))
                            .SetInput("input", "lattice-noise-r")
                            .SetParameter("WaveType", ConstantValue.Create(WaveForm.WaveType.COSINE))
                            .SetParameter("Amplitude", ConstantValue.Create(-0.5f))
                            .SetParameter("Periodicity", ConstantValue.Create(new Vector2(0, 0.0122f)))
                            .SetParameter("Turbulence", ConstantValue.Create(new Vector2(512, 512))))
                    .AddStep(
                        new PipelineStep.Builder()
                                .SetKey("lattice-noise-b")
                                .SetChannel(Channel.BLUE)
                                .SetType(typeof(LatticeNoise.Builder))
                                .SetInput("input", "wave-form-r")
                                .SetParameter("Seed", bSeed))
                    .AddStep(
                        new PipelineStep.Builder()
                            .SetKey("wave-form-b")
                            .SetChannel(Channel.BLUE)
                            .SetType(typeof(WaveForm.Builder))
                            .SetInput("input", "lattice-noise-b")
                            .SetParameter("WaveType", ConstantValue.Create(WaveForm.WaveType.COSINE))
                            .SetParameter("Amplitude", ConstantValue.Create(-0.5f))
                            .SetParameter("Periodicity", ConstantValue.Create(new Vector2(0, 0.0244f)))
                            .SetParameter("Turbulence", ConstantValue.Create(new Vector2(768, 768))))
                    .AddStep(
                        new PipelineStep.Builder()
                            .SetKey("lattice-noise-g")
                            .SetChannel(Channel.GREEN)
                            .SetType(typeof(LatticeNoise.Builder))
                            .SetInput("input", "wave-form-b")
                            .SetParameter("Seed", gSeed))
                    .Build();
            var pipelineTime = new Stopwatch();
            var canvases = new CachingCanvasProvider(new(512, 512), Color4.Black);
            for (int i = 0; i < 0; ++i)
            {
                rSeed.Value = random.Next();
                gSeed.Value = random.Next();
                bSeed.Value = random.Next();

                pipelineTime.Start();
                var output = pipeline.Run(canvases);
                pipelineTime.Stop();

                output.GetTexture().CopyToImage().SaveToFile($"example-out-{i}.png");
                canvases.Return(output);
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
