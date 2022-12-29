namespace Cardamom.Mathematics.Coordinates.Projections
{
    public interface IProjection<THighDimension, TLowDimension>
    {
        public TLowDimension Project(THighDimension coordinate);
        public THighDimension Wrap(TLowDimension coordinate);
    }
}
