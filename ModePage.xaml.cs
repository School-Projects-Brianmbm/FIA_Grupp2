using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FIA_Grupp2
{
    /// <summary>
    /// Page to choos online or offline mode. Online is currently not supported.
    /// </summary>
    public sealed partial class ModePage : Page
    {
        public ModePage()
        {
            this.InitializeComponent();
        }
        private Playlist introAudio;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            introAudio = e.Parameter as Playlist;
        }

        private void OfflineButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LobbyPage), introAudio);
        }
    }
}
