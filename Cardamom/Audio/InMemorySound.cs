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
            using var reader = new AudioFileReader(path);
            var data = new List<float>((int)(0.25f * reader.Length));
            var readBuffer = new float[reader.WaveFormat.SampleRate * reader.WaveFormat.Channels];
            int samplesRead;
            while ((samplesRead = reader.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                data.AddRange(readBuffer.Take(samplesRead));
            }
            return new(reader.WaveFormat, data.ToArray());
        }

        public ISampleProvider GetSampleProvider()
        {
            return new InMemorySoundSampleProvider(WaveFormat, _data);
        }
    }
}
