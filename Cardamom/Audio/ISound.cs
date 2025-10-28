using NAudio.Wave;

namespace Cardamom.Audio
{
    public interface ISound
    {
        ISampleProvider GetSampleProvider();
    }
}
