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
	/// Page to show the rules of the game.
	/// </summary>
	public sealed partial class RulesPage : Page
	{
		public RulesPage()
		{
			this.InitializeComponent();
		}

		private Playlist introAudio;
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			introAudio = e.Parameter as Playlist;
		}

		private void Back_button_click_event(object sender, RoutedEventArgs e)
		{
            SoundEffect.PlayTrack(SoundEffect.ClickPath);
            this.Frame.Navigate(typeof(MainPage), introAudio);
		}
	}
}
