using NAudio.Wave;

namespace Cardamom.Audio
{
    public class FileStreamSound : ISound
    {
        private readonly string _path;

        private FileStreamSound(string path)
        {
            _path = path;
        }

        public static FileStreamSound FromFile(string path)
        {
            return new FileStreamSound(path);
        }

        public ISampleProvider GetSampleProvider()
        {
            return new AutoDisposeSampleProvider(AudioLoader.GetReader(_path));
        }
    }
}
