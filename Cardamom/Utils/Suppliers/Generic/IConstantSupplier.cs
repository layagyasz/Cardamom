namespace Cardamom.Utils.Suppliers.Generic
{
    public interface IConstantSupplier : ISupplier 
    {
        void Set(object value);
    }
}
