using NAudio.Wave;

namespace Cardamom.Audio
{
    public class SoundtrackPlayer
    {
        public enum PlayMode
        {
            Sequence,
            Shuffle
        }

        public AudioPlayer AudioPlayer { get; }
        public Playlist Playlist { get; }
        public PlayMode Mode { get; }
        public WaveFormat WaveFormat => _currentSampleProvider!.WaveFormat;
        public AudioTrack? CurrentTrack { get; private set; }

        private readonly Random _random = new();

        private ISampleProvider? _currentSampleProvider;
        private Queue<AudioTrack> _order = new();

        public SoundtrackPlayer(AudioPlayer audioPlayer, Playlist playlist, PlayMode mode)
        {
            AudioPlayer = audioPlayer;
            Playlist = playlist;
            Mode = mode;
        }

        public void Initialize()
        {
            AudioPlayer.SoundFinished += HandleTrackFinished;
            Skip();
        }

        public void Skip()
        {
            if (!_order.Any())
            {
                QueueTracks();
            }
            // TODO: Remove current track
            SetTrack(_order.Dequeue());
        }

        public void SetTrack(AudioTrack track)
        {
            CurrentTrack = track;
            _currentSampleProvider = track.Track!.GetSampleProvider();
            AudioPlayer.Play(_currentSampleProvider);
        }

        private void QueueTracks()
        {
            if (Mode == PlayMode.Shuffle)
            {
                _order = new(Collections.Extensions.ShuffleCopy(Playlist.Tracks, _random));
            }
            else
            {
                _order = new(Playlist.Tracks);
            }
        }

        private void HandleTrackFinished(object? sender, ISampleProvider e)
        {
            if (e == _currentSampleProvider)
            {
                Skip();
            }
        }
    }
}
