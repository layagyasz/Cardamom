namespace Cardamom.Audio
{
    public record class Playlist
    {
        public List<AudioTrack> Tracks { get; set; } = new();
    }
}
