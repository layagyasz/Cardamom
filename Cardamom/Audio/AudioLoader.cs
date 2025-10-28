using NAudio.Vorbis;
using NAudio.Wave;

namespace Cardamom.Audio
{
    public static class AudioLoader
    {
        public static ISampleProvider GetReader(string path)
        {
            if (path.EndsWith(".ogg"))
            {
                return new VorbisWaveReader(path);
            }
            return new AudioFileReader(path);
        }
    }
}
