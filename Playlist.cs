using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Media.Core;
using System.Diagnostics;
using System.Threading;

namespace FIA_Grupp2
{
    /// <summary>
    /// Represents a playlist that manages playback of media files.
    /// </summary>
    public class Playlist
    {
        private MediaPlayer _mediaPlayer;
        private int _currentIndex = 0;
        private IReadOnlyList<StorageFile> _playList;

        /// <summary>
        /// Initializes a new instance of the Playlist class.
        /// </summary>
        public Playlist()
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.PlaybackSession.PlaybackStateChanged += PlaybackStateSession;
        }

        /// <summary>
        /// Initializes the playlist with media files from the specified folder.
        /// </summary>
        public async Task InitializePlaylist(string folderName)
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\" + folderName);
            _playList = await folder.GetFilesAsync();
        }

        /// <summary>
        /// Starts playback of the playlist.
        /// </summary>
        public void StartPlayback()
        {
            if (_playList != null && _playList.Count > 0)
            {
                PlayTrack(_currentIndex);
            }
        }

        private void PlayTrack(int index)
        {
            if (index >= 0 && index < _playList.Count)
            {
                StorageFile file = _playList[index];
                _mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
                //_mediaPlayer.Play();
            }

        }

        /// <summary>
        /// Automatically plays the next track in the playlist.
        /// </summary>
        private void PlaybackStateSession(MediaPlaybackSession sender, object args)
        {
            Thread.Sleep(500);

            if (sender.PlaybackState == MediaPlaybackState.Paused)
            {
                _currentIndex = (_currentIndex + 1) % _playList.Count;
                PlayTrack(_currentIndex);
            }
        }

        /// <summary>
        /// Stops playback of the playlist.
        /// </summary>
        public void StopPlayback()
        {
            _mediaPlayer.Source = null;
        }
    }

}
