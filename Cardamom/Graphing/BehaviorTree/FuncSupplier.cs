namespace Cardamom.Graphing.BehaviorTree
{
    public class FuncSupplier<T> : ISupplier<T>
    {
        public Func<T> SupplierFn { get; }

        public FuncSupplier(Func<T> supplierFn) 
        {  
            SupplierFn = supplierFn; 
        }

        public T Get()
        {
            return SupplierFn();
        }
    }
}
