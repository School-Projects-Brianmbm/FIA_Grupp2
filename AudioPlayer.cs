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
    public class AudioPlayer
    {
        private MediaPlayer mediaplayer;
        private int currentTrackIndex = 0;
        private IReadOnlyList<StorageFile> playlist;


        public AudioPlayer()
        {
            mediaplayer = new MediaPlayer();
            mediaplayer.PlaybackSession.PlaybackStateChanged += PlaybackStateSession;
        }

        public async Task InitializePlaylist(string folderName)
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\" + folderName);
            playlist = await folder.GetFilesAsync();
        }

        public void StartPlayback()
        {
            if (playlist != null && playlist.Count > 0)
            {
                PlayTrack(currentTrackIndex);
            }
        }

        private void PlayTrack(int index)
        {
            if (index >= 0 && index < playlist.Count)
            {
                StorageFile file = playlist[index];
                mediaplayer.Source = MediaSource.CreateFromStorageFile(file);
                mediaplayer.Play();
            }

        }

        private void PlaybackStateSession(MediaPlaybackSession sender, object args)
        {
            Thread.Sleep(500);

            if (sender.PlaybackState == MediaPlaybackState.Paused)
            {
                currentTrackIndex = (currentTrackIndex + 1) % playlist.Count;
                PlayTrack(currentTrackIndex);
            }
        }

        public void StopPlayback()
        {
            mediaplayer.Source = null;
        }
    }

}
