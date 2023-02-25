using System.Text.Json.Serialization;

namespace Cardamom.Graphics.TexturePacking
{
    public interface ITextureVolume : IDisposable
    {
        IEnumerable<TextureSegment> GetSegments();
        IEnumerable<Texture> GetTextures();
        TextureSegment Add(string Key, Texture texture);
        TextureSegment Add(string Key, Bitmap bitmap);
        TextureSegment Get(string Key);

        [JsonDerivedType(typeof(StaticTexturePage.Builder), "Static")]
        [JsonDerivedType(typeof(DynamicTextureVolume.VariableSizeBuilder), "DynamicVariableSize")]
        [JsonDerivedType(typeof(DynamicTextureVolume.StaticSizeBuilder), "DynamicStaticSize")]
        public interface IBuilder
        {
            ITextureVolume Build();
        }
    }
}
