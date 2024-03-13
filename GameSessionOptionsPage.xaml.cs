using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FIA_Grupp2
{
    /// <summary>
    /// Page to show game session options, including game time and turn time.
    /// </summary>
	public sealed partial class GameSessionOptionsPage : Page
	{
        /// <summary>
        /// Initializes a new instance of the GameSessionOptionsPage class.
        /// </summary>
        public GameSessionOptionsPage()
		{
			this.InitializeComponent();

			//Get saved game session options data
			string serializedData = (string)ApplicationData.Current.LocalSettings.Values["SessionOptionsData"];
			if (!string.IsNullOrEmpty(serializedData))
			{
				GameSessionOptions sessionOptionsData = JsonConvert.DeserializeObject<GameSessionOptions>(serializedData);

				string gameTimeHours = sessionOptionsData.GameTimeHours.ToString();
				string gameTimeMinutes = sessionOptionsData.GameTimeMinutes.ToString();
				string gameTimeSeconds = sessionOptionsData.GameTimeSeconds.ToString();
				GameTimeHours.Text = int.Parse(gameTimeHours) < 10 ? "0" + gameTimeHours.ToString() : gameTimeHours.ToString();
				GameTimeMinutes.Text = int.Parse(gameTimeMinutes) < 10 ? "0" + gameTimeMinutes.ToString() : gameTimeMinutes.ToString();
				GameTimeSeconds.Text = int.Parse(gameTimeSeconds) < 10 ? "0" + gameTimeSeconds.ToString() : gameTimeSeconds.ToString();

				string turnTimeHours = "00";
				//string turnTimeHours = sessionOptionsData.TurnTimeHours.ToString();
				string turnTimeMinutes = sessionOptionsData.TurnTimeMinutes.ToString();
				string turnTimeSeconds = sessionOptionsData.TurnTimeSeconds.ToString();
				TurnTimeHours.Text = int.Parse(turnTimeHours) < 10 ? "0" + turnTimeHours.ToString() : turnTimeHours.ToString();
				TurnTimeMinutes.Text = int.Parse(turnTimeMinutes) < 10 ? "0" + turnTimeMinutes.ToString() : turnTimeMinutes.ToString();
				TurnTimeSeconds.Text = int.Parse(turnTimeSeconds) < 10 ? "0" + turnTimeSeconds.ToString() : turnTimeSeconds.ToString();
			}
		}


        /// <summary>
        /// Event handler for the back button click event.
        /// </summary>
        private void Back_button_click_event(object sender, RoutedEventArgs e)
		{
            SoundEffect.PlayTrack(SoundEffect.ClickPath);

            this.Frame.Navigate(typeof(LobbyPage), introAudio);
		}

        /// <summary>
        /// Event handler for the save button click event. Saves chosen game session options later accessed in GamePage.
        /// </summary>
        private void Save_button_click_event(object sender, RoutedEventArgs e)
		{
			GameSessionOptions sessionOptionsData = new GameSessionOptions();
			sessionOptionsData.GameTimeHours = int.Parse(GameTimeHours.Text);
			sessionOptionsData.GameTimeMinutes = int.Parse(GameTimeMinutes.Text);
			sessionOptionsData.GameTimeSeconds = int.Parse(GameTimeSeconds.Text);

			sessionOptionsData.TurnTimeHours = int.Parse(TurnTimeHours.Text);
			sessionOptionsData.TurnTimeMinutes = int.Parse(TurnTimeMinutes.Text);
			sessionOptionsData.TurnTimeSeconds = int.Parse(TurnTimeSeconds.Text);

			ApplicationData.Current.LocalSettings.Values["SessionOptionsData"] = JsonConvert.SerializeObject(sessionOptionsData);

            SoundEffect.PlayTrack(SoundEffect.ClickPath);

            this.Frame.Navigate(typeof(LobbyPage), introAudio);
		}

        /// <summary>
        /// Event handler for changing the text in the TextBox.
        /// </summary>
        private void TextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
		{
			args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
		}

        /// <summary>
        /// Event handler to set the maximum value of the game time hours.
        /// </summary>
        private void GameTimeHours_LostFocus(object sender, RoutedEventArgs args)
		{	

			if (int.Parse(GameTimeHours.Text) >= 10) 
			{
				GameTimeHours.Text = "10";
			}
            else if (int.Parse(GameTimeHours.Text) < 10)
            {
				GameTimeHours.Text = "0" + GameTimeHours.Text;
			}
        }

        /// <summary>
        /// Event handler to set the maximum value of the game time minutes.
        /// </summary>
        private void GameTurnMinutes_LostFocus(object sender, RoutedEventArgs args)
		{
			if (int.Parse(TurnTimeMinutes.Text) > 15)
			{
				TurnTimeMinutes.Text = "15";
			}
			else if (int.Parse(TurnTimeMinutes.Text) < 10)
			{
				TurnTimeMinutes.Text = "0" + TurnTimeMinutes.Text;
			}

		}


        private Playlist introAudio;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            introAudio = e.Parameter as Playlist;
        }

        /// <summary>
        /// Event handler to set the maximum value of the turn time minutes and seconds.
        /// </summary>
        private void GameTimeMinutesSecondsAndTurnTimeSeconds_LostFocus(object sender, RoutedEventArgs args)
		{
			TextBox textbox = sender as TextBox;
			if (int.Parse(textbox.Text) >= 99)
			{
				textbox.Text = "99";
			}
			else if (int.Parse(textbox.Text) < 10)
			{
				textbox.Text = "0" + textbox.Text;
			}
		}
	}
}
