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

        [JsonDerivedType(typeof(StaticTexturePage.Builder), "static")]
        [JsonDerivedType(typeof(DynamicVariableSizeTextureVolume.Builder), "dynamic-variable-size")]
        public interface IBuilder
        {
            ITextureVolume Build();
        }
    }
}
