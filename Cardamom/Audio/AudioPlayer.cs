using NAudio.Wave.SampleProviders;
using NAudio.Wave;

namespace Cardamom.Audio
{
    public class AudioPlayer : ManagedResource
    {
        public EventHandler<ISampleProvider>? SoundFinished { get; set; }

        private readonly IWavePlayer _device;
        private readonly MixingSampleProvider _mixer;

        public AudioPlayer(int sampleRate = 44100, int channelCount = 2)
        {
            _device = new WaveOutEvent();
            _mixer = new(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            _mixer.ReadFully = true;
            _mixer.MixerInputEnded += HandleMixerInputEnded;
            _device.Init(_mixer);
            _device.Play();
        }

        private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
        {
            if (input.WaveFormat.Channels == _mixer.WaveFormat.Channels)
            {
                return input;
            }
            if (input.WaveFormat.Channels == 1 && _mixer.WaveFormat.Channels == 2)
            {
                return new MonoToStereoSampleProvider(input);
            }
            throw new NotImplementedException("Not yet implemented this channel count conversion");
        }

        public void Play(ISampleProvider sampleProvider)
        {
            _mixer.AddMixerInput(ConvertToRightChannelCount(sampleProvider));
        }

        protected override void DisposeImpl()
        {
            _device.Dispose();
        }

        private void HandleMixerInputEnded(object? sender, SampleProviderEventArgs e)
        {
            SoundFinished?.Invoke(this, e.SampleProvider);
        }
    }
}
