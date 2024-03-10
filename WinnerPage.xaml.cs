using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FIA_Grupp2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WinnerPage : Page
    {
        private Playlist gameAudio;
        private string name;

        public WinnerPage()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var winnerParameters = e.Parameter as Tuple<string, Playlist>;
            name = winnerParameters.Item1;
            gameAudio = winnerParameters.Item2;
            Debug.Write($" On Navigate Winner: {name}");
            SetBackgroundImage();
        }
        private void SetBackgroundImage()
        {
            Debug.Write($" Set Background Winner: {name}");
            switch (name.ToLower())
            {
                case "pigs":
                    splashImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Backgrounds/pigs_win.jpg"));
                    break;
                case "hens":
                    splashImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Backgrounds/chickens_win.png"));
                    break;
                case "cows":
                    splashImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Backgrounds/cows_win.jpg"));
                    break;
                case "sheeps":
                    splashImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Backgrounds/sheep_win.jpg"));
                    break;
                default:
                    break;
            }

        }

        private void MenuButtonClick(object sender, RoutedEventArgs e)
        {
            gameAudio.StopPlayback();
            this.Frame.Navigate(typeof(StartPage));
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
