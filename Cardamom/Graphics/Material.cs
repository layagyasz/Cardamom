namespace Cardamom.Graphics
{
    public class Material : ManagedResource
    {
        public Texture Diffuse { get; }
        public Texture Normal { get; }
        public Texture Lighting { get; }

        public Material(Texture diffuse, Texture normal, Texture lighting)
        {
            Diffuse = diffuse;
            Normal = normal;
            Lighting = lighting;
        }

        protected override void DisposeImpl()
        {
            Diffuse.Dispose(); 
            Normal.Dispose(); 
            Lighting.Dispose();
        }
    }
}
