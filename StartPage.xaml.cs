﻿using System;
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 


    public sealed partial class StartPage : Page
    {
        public Playlist introAudio;

        public StartPage()
        {
            this.InitializeComponent();
            startStoryBoard.Begin();
            introAudio = new Playlist();
            StartMusic();
            
        }

        private async void StartMusic()
        {
            await introAudio.InitializePlaylist("Assets\\Sound\\Menu");
            introAudio.StartPlayback();
        }

        private void SplashButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), introAudio);
        }
    }
}
