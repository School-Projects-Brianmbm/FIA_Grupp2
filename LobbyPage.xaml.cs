using Newtonsoft.Json;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace FIA_Grupp2
{	

	public sealed partial class LobbyPage : Page
	{
		//Used to store lobby options
		LobbyOptions lobbyOptionsData;

		//Used to store choosable teamicons
		private BitmapImage[] teamicons;

		//Index to keep track of choosen teamicon
		private int slot1IconIndex;
		private int slot2IconIndex;
		private int slot3IconIndex;
		private int slot4IconIndex;

		private bool isIconIndex0Available = true;
		private bool isIconIndex1Available = true;
		private bool isIconIndex2Available = true;
		private bool isIconIndex3Available = true;

		public LobbyPage()
		{
			this.InitializeComponent();

			lobbyOptionsData = new LobbyOptions();
			
			// Initialize the array of BitmapImages
			teamicons = new BitmapImage[]
			{
				new BitmapImage(new Uri("ms-appx:///Assets/TeamIcons/cow.jpg")),
				new BitmapImage(new Uri("ms-appx:///Assets/TeamIcons/pig.jpg")),
				new BitmapImage(new Uri("ms-appx:///Assets/TeamIcons/chicken.jpg")),
				new BitmapImage(new Uri("ms-appx:///Assets/TeamIcons/sheep.jpg"))
			};

			//Starting iconindex for each slot
			slot1IconIndex = 0;
			slot2IconIndex = 1;
			slot3IconIndex = 2;
			slot4IconIndex = 3;

			//Initiate to default option settings:
			lobbyOptionsData.slot1Usertype = "Player";
			lobbyOptionsData.slot2Usertype = "Player";
			lobbyOptionsData.slot3Usertype = "None";
			lobbyOptionsData.slot4Usertype = "None";

			lobbyOptionsData.slot1Username = "Player1";
			lobbyOptionsData.slot2Username = "Player2";
			lobbyOptionsData.slot3Username = "---";
			lobbyOptionsData.slot4Username = "---";

			lobbyOptionsData.slot1Team = "cow";
			lobbyOptionsData.slot2Team = "pig";
			lobbyOptionsData.slot3Team = "chicken";
			lobbyOptionsData.slot4Team = "sheep";


		}

		private void Slot1_user_selection_button_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null && button.Content != null)
			{
				string selectedChoice = button.Content.ToString();

				switch (selectedChoice)
				{
					case "Player":

						button.Content = "AI";
						slot1_username.Text = "AI";
						slot1_button_teamicon.IsEnabled = true;
						slot1_username.IsEnabled = true;
						slot1_rectangle.Opacity = 1;
						lobbyOptionsData.slot1Usertype = "AI";
						break;

					case "AI":

						button.Content = "None";
						slot1_username.Text = "---";
						slot1_button_teamicon.IsEnabled = false;
						slot1_username.IsEnabled = false;
						slot1_rectangle.Opacity = 0.5;
						lobbyOptionsData.slot1Usertype = "None";
						break;

					case "None":

						button.Content = "Player";
						slot1_username.Text = "Player1";
						slot1_button_teamicon.IsEnabled = true;
						slot1_username.IsEnabled = true;
						slot1_rectangle.Opacity = 1;
						lobbyOptionsData.slot1Usertype = "Player";
						break;

					default:
						break;
				}
			}
		}

		private void Slot2_user_selection_button_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null && button.Content != null)
			{
				string selectedChoice = button.Content.ToString();

				switch (selectedChoice)
				{
					case "Player":

						button.Content = "AI";
						slot2_username.Text = "AI";
						slot2_button_teamicon.IsEnabled = true;
						slot2_username.IsEnabled = true;
						slot2_rectangle.Opacity = 1;
						lobbyOptionsData.slot2Usertype = "AI";
						break;

					case "AI":

						button.Content = "None";
						slot2_username.Text = "---";
						slot2_button_teamicon.IsEnabled = false;
						slot2_username.IsEnabled = false;
						slot2_rectangle.Opacity = 0.5;
						lobbyOptionsData.slot2Usertype = "None";
						break;

					case "None":

						button.Content = "Player";
						slot2_username.Text = "Player2";
						slot2_button_teamicon.IsEnabled = true;
						slot2_username.IsEnabled = true;
						slot2_rectangle.Opacity = 1;
						lobbyOptionsData.slot2Usertype = "Player";
						break;

					default:
						break;
				}
			}
		}

		private void Slot3_user_selection_button_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null && button.Content != null)
			{
				string selectedChoice = button.Content.ToString();

				switch (selectedChoice)
				{
					case "Player":

						button.Content = "AI";
						slot3_username.Text = "AI";
						slot3_button_teamicon.IsEnabled = true;
						slot3_username.IsEnabled = true;
						slot3_rectangle.Opacity = 1;
						lobbyOptionsData.slot3Usertype = "AI";
						break;

					case "AI":

						button.Content = "None";
						slot3_username.Text = "---";
						slot3_button_teamicon.IsEnabled = false;
						slot3_username.IsEnabled = false;
						slot3_rectangle.Opacity = 0.5;
						lobbyOptionsData.slot3Usertype = "None";
						break;

					case "None":

						button.Content = "Player";
						slot3_username.Text = "Player3";
						slot3_button_teamicon.IsEnabled = true;
						slot3_username.IsEnabled = true;
						slot3_rectangle.Opacity = 1;
						lobbyOptionsData.slot3Usertype = "Player";
						break;

					default:
						break;
				}
			}
		}

		private void Slot4_user_selection_button_click_event(object sender, RoutedEventArgs e)
		{

			Button button = sender as Button;
			if (button != null && button.Content != null)
			{
				string selectedChoice = button.Content.ToString();

				switch (selectedChoice)
				{
					case "Player":

						button.Content = "AI";
						slot4_username.Text = "AI";
						slot4_button_teamicon.IsEnabled = true;
						slot4_username.IsEnabled = true;
						slot4_rectangle.Opacity = 1;
						lobbyOptionsData.slot4Usertype = "AI";
						break;

					case "AI":

						button.Content = "None";
						slot4_username.Text = "---";
						slot4_button_teamicon.IsEnabled = false;
						slot4_username.IsEnabled = false;
						slot4_rectangle.Opacity = 0.5;
						lobbyOptionsData.slot4Usertype = "None";
						break;

					case "None":

						button.Content = "Player";
						slot4_username.Text = "Player4";
						slot4_button_teamicon.IsEnabled = true;
						slot4_username.IsEnabled = true;
						slot4_rectangle.Opacity = 1;
						lobbyOptionsData.slot4Usertype = "Player";
						break;

					default:
						break;
				}
			}
		}

		private void Slot1_username_LostFocus(object sender, RoutedEventArgs args)
		{
			lobbyOptionsData.slot1Username = slot1_username.Text;
		}

		private void Slot2_username_LostFocus(object sender, RoutedEventArgs args)
		{
			lobbyOptionsData.slot2Username = slot2_username.Text;
		}

		private void Slot3_username_LostFocus(object sender, RoutedEventArgs args)
		{
			lobbyOptionsData.slot3Username = slot3_username.Text;
		}

		private void Slot4_username_LostFocus(object sender, RoutedEventArgs args)
		{
			lobbyOptionsData.slot4Username = slot4_username.Text;
		}

		private void Slot1_button_teamicon_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				//Goes through the icons in teamicon array. When slot1IconIndex == teamicons.Length the remainder will be 0, reseting the slot1IconIndex back to 0.
				slot1IconIndex = (slot1IconIndex + 1) % teamicons.Length;
				slot1_teamicon.Source = teamicons[slot1IconIndex];
				if (slot1IconIndex == 0) {
					lobbyOptionsData.slot1Team = "cow";
				}
				else if (slot1IconIndex == 1)
				{
					lobbyOptionsData.slot1Team = "pig";
				}
				else if (slot1IconIndex == 2)
				{
					lobbyOptionsData.slot1Team = "chicken";
				}
				else if (slot1IconIndex == 3)
				{
					lobbyOptionsData.slot1Team = "sheep";
				}
			}
		}

		private void Slot2_button_teamicon_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				slot2IconIndex = (slot2IconIndex + 1) % teamicons.Length;
				slot2_teamicon.Source = teamicons[slot2IconIndex];
				if (slot2IconIndex == 0)
				{
					lobbyOptionsData.slot2Team = "cow";
				}
				else if (slot2IconIndex == 1)
				{
					lobbyOptionsData.slot2Team = "pig";
				}
				else if (slot2IconIndex == 2)
				{
					lobbyOptionsData.slot2Team = "chicken";
				}
				else if (slot2IconIndex == 3)
				{
					lobbyOptionsData.slot2Team = "sheep";
				}
			}
		}

		private void Slot3_button_teamicon_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				slot3IconIndex = (slot3IconIndex + 1) % teamicons.Length;
				slot3_teamicon.Source = teamicons[slot3IconIndex];
				if (slot3IconIndex == 0)
				{
					lobbyOptionsData.slot3Team = "cow";
				}
				else if (slot3IconIndex == 1)
				{
					lobbyOptionsData.slot3Team = "pig";
				}
				else if (slot3IconIndex == 2)
				{
					lobbyOptionsData.slot3Team = "chicken";
				}
				else if (slot3IconIndex == 3)
				{
					lobbyOptionsData.slot3Team = "sheep";
				}
			}
		}

		private void Slot4_button_teamicon_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				slot4IconIndex = (slot4IconIndex + 1) % teamicons.Length;
				slot4_teamicon.Source = teamicons[slot4IconIndex];
				if (slot4IconIndex == 0)
				{
					lobbyOptionsData.slot4Team = "cow";
				}
				else if (slot4IconIndex == 1)
				{
					lobbyOptionsData.slot4Team = "pig";
				}
				else if (slot4IconIndex == 2)
				{
					lobbyOptionsData.slot4Team = "chicken";
				}
				else if (slot4IconIndex == 3)
				{
					lobbyOptionsData.slot4Team = "sheep";
				}
			}
		}
        private Playlist introAudio;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            introAudio = e.Parameter as Playlist;
        }



        private void Back_button_click_event(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(MainPage));
		}

		private void Options_button_click_event(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(GameSessionOptionsPage));
		}

		private void Choose_level_button_click_event(object sender, RoutedEventArgs e)
		{	
			//Save the lobby options:
			//LobbyOptions lobbyOptionsData = new LobbyOptions();

			//lobbyOptionsData.slot1Usertype = slot1_user_button.Content.ToString();
			//lobbyOptionsData.slot1Username = slot1_username.Text.ToString();
			//lobbyOptionsData.slot1Team = slot1_user_button.Content.ToString();

			ApplicationData.Current.LocalSettings.Values["LobbyOptionsData"] = JsonConvert.SerializeObject(lobbyOptionsData);
			introAudio.StopPlayback();
			this.Frame.Navigate(typeof(GamePage));
		}

	}
}
