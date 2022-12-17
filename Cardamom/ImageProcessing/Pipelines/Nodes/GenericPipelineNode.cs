using Cardamom.ImageProcessing.Filters;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class GenericPipelineNode : IPipelineNode
    {
        public string Key { get; set; }
        public Channel Channel { get; }
        public bool Inline { get; }

        private readonly Type _type;
        private readonly Type _builderType;
        private readonly Dictionary<string, string> _inputs;
        private readonly Dictionary<string, IParameterValue> _parameters;

        private GenericPipelineNode(
            string key,
            Channel channel,
            Type type,
            Dictionary<string, string> inputs,
            Dictionary<string, IParameterValue> parameters)
        {
            Key = key;
            Channel = channel;
            _type = type;
            _inputs = inputs;
            _parameters = parameters;

            var builderAttr =
                (FilterBuilderAttribute)type.GetCustomAttributes(/* inherit= */ false)
                    .First(x => x.GetType() == typeof(FilterBuilderAttribute));
            _builderType = builderAttr.Type;

            Inline = 
                type.GetCustomAttributes(/* inherit= */ false).Any(x => x.GetType() == typeof(FilterInlineAttribute));
        }

        public Dictionary<string, string> GetInputs()
        {
            return _inputs;
        }

        public void Run(Canvas output, Dictionary<string, Canvas> inputs)
        {
            var builder = (IFilter.IFilterBuilder)Activator.CreateInstance(_builderType)!;
            foreach (var param in _parameters)
            {
                _type.GetMethod("Set" + param.Key)!.Invoke(builder, new object?[] { param.Value.Get() });
            }
            builder.Build().Apply(output, Channel, inputs);
        }

        public class Builder : IPipelineNode.IBuilder
        {
            public string Key { get; set; } = string.Empty;
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

            public Builder SetParameter(string parameterName, IParameterValue value)
            {
                Parameters.Add(parameterName, value);
                return this;
            }

            public IPipelineNode Build()
            {
                return new GenericPipelineNode(Key!, Channel, Type!, Inputs, Parameters);
            }
        }
    }
}
