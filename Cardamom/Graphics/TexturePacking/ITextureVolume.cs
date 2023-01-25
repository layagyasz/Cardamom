using System.Text.Json.Serialization;

namespace Cardamom.Graphics.TexturePacking
{
    public interface ITextureVolume
    {
        IEnumerable<TextureSegment> GetSegments();
        IEnumerable<Texture> GetTextures();
        TextureSegment Add(string Key, Texture texture);
        TextureSegment Add(string Key, Bitmap bitmap);
        TextureSegment Get(string Key);

        [JsonDerivedType(typeof(StaticTexturePage.Builder), "Static")]
        [JsonDerivedType(typeof(DynamicVariableSizeTextureVolume.Builder), "DynamicVariableSize")]
        public interface IBuilder
        {
            ITextureVolume Build();
        }
    }
}
