using NAudio.Wave;

namespace Cardamom.Audio
{
    public class AutoDisposeSampleProvider : ISampleProvider
    {
        public WaveFormat WaveFormat => _sampleProvider.WaveFormat;

        private readonly ISampleProvider _sampleProvider;
        private bool _disposed;

        public AutoDisposeSampleProvider(ISampleProvider sampleProvider)
        {
            _sampleProvider = sampleProvider;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (_disposed)
            {
                return 0;
            }
            int read = _sampleProvider.Read(buffer, offset, count);
            if (read == 0)
            {
                if (_sampleProvider is IDisposable d)
                {
                    d.Dispose();
                }
                _disposed = true;
            }
            return read;
        }
    }
}
