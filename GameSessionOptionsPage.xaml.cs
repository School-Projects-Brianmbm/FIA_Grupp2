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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FIA_Grupp2
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class GameSessionOptionsPage : Page
	{
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

				string turnTimeHours = sessionOptionsData.TurnTimeHours.ToString();
				string turnTimeMinutes = sessionOptionsData.TurnTimeMinutes.ToString();
				string turnTimeSeconds = sessionOptionsData.TurnTimeSeconds.ToString();
				TurnTimeHours.Text = int.Parse(turnTimeHours) < 10 ? "0" + turnTimeHours.ToString() : turnTimeHours.ToString();
				TurnTimeMinutes.Text = int.Parse(turnTimeMinutes) < 10 ? "0" + turnTimeMinutes.ToString() : turnTimeMinutes.ToString();
				TurnTimeSeconds.Text = int.Parse(turnTimeSeconds) < 10 ? "0" + turnTimeSeconds.ToString() : turnTimeSeconds.ToString();
			}
		}

		private void Back_button_click_event(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(LobbyPage));
		}

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

			this.Frame.Navigate(typeof(LobbyPage));
		}

		private void TextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
		{
			args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
		}

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

		private void GameTurnMinutes_LostFocus(object sender, RoutedEventArgs args)
		{

			if (int.Parse(TurnTimeMinutes.Text) >= 15)
			{
				TurnTimeMinutes.Text = "15";
			}
			else if (int.Parse(TurnTimeMinutes.Text) < 10)
			{
				TurnTimeMinutes.Text = "0" + TurnTimeMinutes.Text;
			}
		}

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
