using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers.Generic;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class GenericPipelineNode : IPipelineNode
    {
        public string Key { get; set; }
        public Channel Channel { get; }
        public bool External => false;
        public bool Inline { get; }

        private readonly Type _type;
        private readonly Type _builderType;
        private readonly Dictionary<string, string> _inputs;
        private readonly string? _output;
        private readonly Dictionary<string, ISupplier> _parameters;

        private GenericPipelineNode(
            string key,
            Channel channel,
            Type type,
            Dictionary<string, string> inputs,
            string? output,
            Dictionary<string, ISupplier> parameters)
        {
            Key = key;
            Channel = channel;
            _type = type;
            _inputs = inputs;
            _output = output;
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

        public string? GetOutput()
        {
            return _output;
        }

        public Canvas Run(Canvas? output, Dictionary<string, Canvas> inputs)
        {
            var builder = (IFilter.IFilterBuilder)Activator.CreateInstance(_builderType)!;
            foreach (var param in _parameters)
            {
                _type.GetMethod("Set" + param.Key)!.Invoke(builder, new object?[] { param.Value.Get<object>() });
            }
            builder.Build().Apply(output!, Channel, inputs);
            return output!;
        }

        public class Builder : IPipelineNode.IBuilder
        {
            public string Key { get; set; } = string.Empty;
            public Type? Type { get; set; }
            public Channel Channel { get; set; } = Channel.All;
            public Dictionary<string, string> Inputs { get; set; } = new();
            public string? Output { get; set; }
            public Dictionary<string, ISupplier> Parameters { get; set; } = new();

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

            public Builder SetOutput(string stepKey)
            {
                Output = stepKey;
                return this;
            }

            public Builder SetParameter(string parameterName, ISupplier value)
            {
                Parameters.Add(parameterName, value);
                return this;
            }

            public IPipelineNode Build()
            {
                return new GenericPipelineNode(Key!, Channel, Type!, Inputs, Output, Parameters);
            }
        }
    }
}
