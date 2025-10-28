using NAudio.Wave;

namespace Cardamom.Audio
{
    public class InMemorySoundSampleProvider : ISampleProvider
    {
        public WaveFormat WaveFormat { get; }

        private readonly float[] _data;
        private long _position;

        public InMemorySoundSampleProvider(WaveFormat waveFormat, float[] data)
        {
            WaveFormat = waveFormat;
            _data = data;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = _data.LongLength - _position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(_data, _position, buffer, offset, samplesToCopy);
            _position += samplesToCopy;
            return (int)samplesToCopy;
        }
    }
}
