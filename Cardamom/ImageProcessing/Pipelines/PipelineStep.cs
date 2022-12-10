using Cardamom.ImageProcessing.Filters;

namespace Cardamom.ImageProcessing.Pipelines
{
    public class PipelineStep : IKeyed
    {
        public string? Key { get; set; }

        private readonly Type _type;
        private readonly Channel _channel;
        private readonly Dictionary<string, string> _inputs;
        private readonly Dictionary<string, IParameterValue> _parameters;

        public PipelineStep(
            string key,
            Type type,
            Channel channel, 
            Dictionary<string, string> inputs, 
            Dictionary<string, IParameterValue> parameters)
        {
            Key = key;
            _type = type;
            _channel = channel;
            _inputs = inputs;
            _parameters = parameters;
        }

        public Dictionary<string, string> GetInputs()
        {
            return _inputs;
        }

        public Canvas Run(Dictionary<string, Canvas> inputs, ICanvasProvider canvasProvider)
        {
            var builder = (IFilter.IFilterBuilder)Activator.CreateInstance(_type)!;
            foreach (var param in _parameters)
            {
                _type.GetMethod("Set" + param.Key)!.Invoke(builder, new object?[] { param.Value.Get() });
            }
            var filter = builder.Build();
            var outCanvas = filter.InPlace ? inputs.First().Value : canvasProvider.Get();
            filter.Apply(outCanvas, _channel, inputs);
            return outCanvas;
        }

        public class Builder : IKeyed
        {
            public string? Key { get; set; }
            public Type? Type { get; set; }
            public Channel Channel { get; set; } = Channel.ALL;
            public Dictionary<string, string> Inputs { get; set; } = new();
            public Dictionary<string, IParameterValue> Parameters { get; set; } = new();

            public Builder SetKey(string key)
            {
                Key = key;
                return this;
            }

            public Builder SetType(Type type)
            {
                Type = type;
                return this;
            }
            
            public Builder SetChannel(Channel channel)
            {
                Channel = channel;
                return this;
            }

            public Builder SetInput(string inputName, string stepKey)
            {
                Inputs.Add(inputName, stepKey);
                return this;
            }

            public Builder SetParameter(string parameterName,  IParameterValue value) 
            {
                Parameters.Add(parameterName, value);
                return this;
            }

            public PipelineStep Build()
            {
                return new PipelineStep(Key!, Type, Channel, Inputs, Parameters);
            }
        }
    }
}
