using NAudio.Wave;

namespace Cardamom.Audio
{
    public class InMemorySound : ISound
    {
        public WaveFormat WaveFormat { get; }

        private readonly float[] _data;

        private InMemorySound(WaveFormat waveFormat, float[] data)
        {
            WaveFormat = waveFormat;
            _data = data;
        }

        public static InMemorySound FromFile(string path)
        {
            var reader = AudioLoader.GetReader(path);
            var data = new List<float>();
            var readBuffer = new float[reader.WaveFormat.SampleRate * reader.WaveFormat.Channels];
            int samplesRead;
            while ((samplesRead = reader.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                data.AddRange(readBuffer.Take(samplesRead));
            }
            var sound = new InMemorySound(reader.WaveFormat, data.ToArray());
            if (reader is IDisposable d)
            {
                d.Dispose();
            }
            return sound;
        }

        public ISampleProvider GetSampleProvider()
        {
            return new InMemorySoundSampleProvider(WaveFormat, _data);
        }
    }
}
