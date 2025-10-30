using Cardamom.Json.Audio;
using System.Text.Json.Serialization;

namespace Cardamom.Audio
{
    public record class AudioTrack
    {
        public string? Name { get; set; }

        [JsonConverter(typeof(FileStreamSoundConverter))]
        public ISound? Track { get; set; }
    }
}
