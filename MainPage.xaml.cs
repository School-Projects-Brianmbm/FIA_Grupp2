using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
//TEST
//Buttons enabled even if not functioning otherwise not visible
namespace FIA_Grupp2
{
    /// <summary>
    /// Page for the starting menu of game. 
    /// </summary>
    public sealed partial class MainPage : Page
    {


        public MainPage()
        {
            this.InitializeComponent();

        }

        private Playlist introAudio;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            introAudio = e.Parameter as Playlist;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ModePage), introAudio);

        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

		private void RulesButton_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(RulesPage), introAudio);
		}
    }
}
