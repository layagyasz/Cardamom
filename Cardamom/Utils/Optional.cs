namespace Cardamom.Utils
{
    public sealed class Optional<T>
    {
        public bool HasValue { get; }

        private readonly T? _value;

        private Optional(T? value, bool hasValue)
        {
            _value = value;
            HasValue = hasValue;
        }

        public static Optional<T> Of(T? value)
        {
            return new(value, /* hasValue= */ true);
        }

        public static Optional<T> Empty()
        {
            return new(/* value= */ default, /* hasValue= */ false);
        }

        public override bool Equals(object? other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (other is Optional<T> o)
            {
                return Equals(_value, o._value) && Equals(HasValue, o.HasValue);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value, HasValue);
        }

        public Optional<K> Map<K>(Func<T?, K?> mapFn)
        {
            return HasValue ? Optional<K>.Of(mapFn(_value)) : Optional<K>.Empty();
        }

        public T? OrElse(T? value)
        {
            return HasValue ? _value : value;
        }

        public T? OrElseDefault()
        {
            return OrElse(default);
        }
    }
}
