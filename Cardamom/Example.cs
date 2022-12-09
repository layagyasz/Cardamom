using Cardamom.Collections;
using Cardamom.Graphics;
using Cardamom.Graphics.Ui;
using Cardamom.Graphics.Ui.Controller;
using Cardamom.Graphics.Ui.Elements;
using Cardamom.Json;
using Cardamom.Trackers;
using Cardamom.Window;
using OpenTK.Mathematics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cardamom
{
    public static class Example
    {
        class ExampleCollectionData
        {
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public enum ExampleEnum
            {
                EXAMPLE_0,
                EXAMPLE_1,
                EXAMPLE_2
            }

            public class ExampleKeyed : IKeyed
            {
                public string? Key { get; set; }

                public override string ToString()
                {
                    return string.Format($"[ExampleKeyed: Key={Key}]");
                }
            }

            public List<ExampleKeyed>? Objects { get; set; }
            public EnumMap<ExampleEnum, int>? EnumMap { get; set; }
            public EnumSet<ExampleEnum>? EnumSet { get; set; }
            public MultiCount<int>? MultiCount { get; set; }
            public WeightedVector<ExampleKeyed>? WeightedVector { get; set; }
        }

        public static void Main()
        {
            JsonSerializerOptions jsonOptions = new()
            {
                ReferenceHandler = new KeyedReferenceHandler(new Dictionary<string, IKeyed>()),
            };
            var exampleCollectionData = 
                JsonSerializer.Deserialize<ExampleCollectionData>(
                    File.ReadAllText("Example/ExampleCollectionData.json"), jsonOptions);
            foreach (var entry in exampleCollectionData!.Objects!)
            {
                Console.WriteLine(entry);
            }
            foreach (var entry in exampleCollectionData!.EnumMap!)
            {
                Console.WriteLine(entry);
            }
            foreach (var entry in exampleCollectionData!.EnumSet!)
            {
                Console.WriteLine(entry);
            }
            foreach (var entry in exampleCollectionData!.MultiCount!)
            {
                Console.WriteLine(entry);
            }
            foreach (var entry in exampleCollectionData!.WeightedVector!)
            {
                Console.WriteLine(entry);
            }

            var window = new RenderWindow("Cardamom - Example", new Vector2i(800, 600));
            var ui = new UiWindow(window);
            var uiElementFactory =
                new UiElementFactory(
                    new ClassLibrary.Builder()
                                    .ReadTextures("Example/Textures.json")
                                    .ReadFonts("Example/Fonts.json")
                                    .ReadShaders("Example/Shaders.json")
                                    .ReadClasses("Example", "Style.json")
                                    .Build(),
                    SimpleKeyMapper.US);
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

            var text = uiElementFactory.CreateTextInput("example-row-class", new(0, select.Item1.Size.Y));
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
