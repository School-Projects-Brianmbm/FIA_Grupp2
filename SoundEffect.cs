using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace FIA_Grupp2
{
    // <summary>
    /// Represents a class that handles playback of soundeffects
    /// </summary>
    public static class SoundEffect
    {
        private static MediaPlayer _mediaPlayer = new MediaPlayer();
        public static string ClickPath = "Assets\\Sound\\SoundEffects\\select-sound-121244.mp3";
        public static string DicePath = "Assets\\Sound\\SoundEffects\\gamemisc_dice-roll-on-wood_jaku5-37414.mp3";

        public static string CowPath = "Assets\\Sound\\SoundEffects\\mooing-cow-122255.mp3";
        public static string ChickenPath = "Assets\\Sound\\SoundEffects\\chicken-sound.mp3";
        public static string PigPath = "Assets\\Sound\\SoundEffects\\pig-grunt-100272.mp3";
        public static string SheepPath = "Assets\\Sound\\SoundEffects\\sheep-122256.mp3";

        public static string VictoriusPath = "Assets\\Sound\\SoundEffects\\game-victorius-144751.mp3";

        public static async void PlayTrack(string trackPath)
        {
            /*
             StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\" + folderName);
            _playList = await folder.GetFilesAsync();*/

            if(GamePage.Instance != null)
            {
                SetVolume(GamePage.Instance.MusicVolume / 100.0);
            }
            else
            {
                //TODO: Since the only place we are setting the volume is in the game page at the moment, in the future maybe there will be a settings view with a volume slider that will control the sound in the game instead.
                SetVolume(1.0);
            }

            StorageFile file = await StorageFile.GetFileFromPathAsync(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\" + trackPath);
            _mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
            _mediaPlayer.Play();
        }

        public static void SetVolume(double volume)
        {
            if (volume >= 0.0 && volume <= 1.0)
            {
                _mediaPlayer.Volume = volume;
            }
        }
    }
}
