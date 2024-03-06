using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace FIA_Grupp2
{	
	//Could be used for 
	public enum SlotChoice
	{
		Player,
		AI,
		None
	}

	public sealed partial class LobbyPage : Page
	{
		//Used to store choosable teamicons
		private BitmapImage[] teamicons;

		//Index to keep track of choosen teamicon
		private int slot1IconIndex;
		private int slot2IconIndex;
		private int slot3IconIndex;
		private int slot4IconIndex;

		public LobbyPage()
		{
			this.InitializeComponent();

			// Initialize the array of BitmapImages
			teamicons = new BitmapImage[]
			{
				new BitmapImage(new Uri("ms-appx:///Assets/TeamIcons/cow.jpg")),
				new BitmapImage(new Uri("ms-appx:///Assets/TeamIcons/pig.jpg")),
				new BitmapImage(new Uri("ms-appx:///Assets/TeamIcons/chicken.jpg")),
				new BitmapImage(new Uri("ms-appx:///Assets/TeamIcons/sheep.jpg"))
			};

			//Starting iconindex dor each slot
			slot1IconIndex = 0;
			slot2IconIndex = 1;
			slot3IconIndex = 2;
			slot4IconIndex = 3;

		}

		private void slot1_user_selection_button_click_event(object sender, RoutedEventArgs e)
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
						slot1_rectangle.Opacity = 1;
						break;

					case "AI":

						button.Content = "None";
						slot1_username.Text = "---";
						slot1_button_teamicon.IsEnabled = false;
						slot1_rectangle.Opacity = 0.5;
						break;

					case "None":

						button.Content = "Player";
						slot1_username.Text = "Player1";
						slot1_button_teamicon.IsEnabled = true;
						slot1_rectangle.Opacity = 1;
						break;

					default:
						break;
				}
			}
		}

		private void slot2_user_selection_button_click_event(object sender, RoutedEventArgs e)
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
						slot2_rectangle.Opacity = 1;
						break;

					case "AI":

						button.Content = "None";
						slot2_username.Text = "---";
						slot2_button_teamicon.IsEnabled = false;
						slot2_rectangle.Opacity = 0.5;
						break;

					case "None":

						button.Content = "Player";
						slot2_username.Text = "Player2";
						slot2_button_teamicon.IsEnabled = true;
						slot2_rectangle.Opacity = 1;
						break;

					default:
						break;
				}
			}
		}

		private void slot3_user_selection_button_click_event(object sender, RoutedEventArgs e)
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
						slot3_rectangle.Opacity = 1;
						break;

					case "AI":

						button.Content = "None";
						slot3_username.Text = "---";
						slot3_button_teamicon.IsEnabled = false;
						slot3_rectangle.Opacity = 0.5;
						break;

					case "None":

						button.Content = "Player";
						slot3_username.Text = "Player3";
						slot3_button_teamicon.IsEnabled = true;
						slot3_rectangle.Opacity = 1;
						break;

					default:
						break;
				}
			}
		}

		private void slot4_user_selection_button_click_event(object sender, RoutedEventArgs e)
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
						slot4_rectangle.Opacity = 1;
						break;

					case "AI":

						button.Content = "None";
						slot4_username.Text = "---";
						slot4_button_teamicon.IsEnabled = false;
						slot4_rectangle.Opacity = 0.5;
						break;

					case "None":

						button.Content = "Player";
						slot4_username.Text = "Player4";
						slot4_button_teamicon.IsEnabled = true;
						slot4_rectangle.Opacity = 1;
						break;

					default:
						break;
				}
			}
		}

		private void slot1_button_teamicon_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				//Goes through the icons in teamicon array. When slot1IconIndex == teamicons.Length the remainder will be 0, reseting the slot1IconIndex back to 0.
				slot1IconIndex = (slot1IconIndex + 1) % teamicons.Length;
				slot1_teamicon.Source = teamicons[slot1IconIndex];
			}
		}

		private void slot2_button_teamicon_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				slot2IconIndex = (slot2IconIndex + 1) % teamicons.Length;
				slot2_teamicon.Source = teamicons[slot2IconIndex];
			}
		}

		private void slot3_button_teamicon_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				slot3IconIndex = (slot3IconIndex + 1) % teamicons.Length;
				slot3_teamicon.Source = teamicons[slot3IconIndex];
			}
		}

		private void slot4_button_teamicon_click_event(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				slot4IconIndex = (slot4IconIndex + 1) % teamicons.Length;
				slot4_teamicon.Source = teamicons[slot4IconIndex];
			}
		}
        private AudioPlayer introAudio;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            introAudio = e.Parameter as AudioPlayer;
        }



        private void back_button_click_event(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(MainPage));
		}

		private void options_button_click_event(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(GameSessionOptionsPage));
		}

		private void choose_level_button_click_event(object sender, RoutedEventArgs e)
		{
			introAudio.StopPlayback();
			this.Frame.Navigate(typeof(GamePage));
		}
	}
}
