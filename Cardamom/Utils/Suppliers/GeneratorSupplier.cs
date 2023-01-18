using Cardamom.Utils.Generators;

namespace Cardamom.Utils.Suppliers
{
    public class GeneratorSupplier<T> : ISupplier<T>
    {
        public IGenerator<T>? Generator { get; set; }

        private T? _value;

        public GeneratorSupplier() { }

        public GeneratorSupplier(IGenerator<T> generator)
        {
            Generator = generator;
        }

        public T Get()
        {
            return _value!;
        }

        public void Regenerate(Random random)
        {
            _value = Generator!.Generate(random);
        }
    }
}
