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
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	/// 

	public enum SlotChoice
	{
		Player,
		AI,
		None
	}

	public sealed partial class LobbyPage : Page
	{
		public LobbyPage()
		{
			this.InitializeComponent();

			SlotChoice slot1 = SlotChoice.Player;
			SlotChoice slot2 = SlotChoice.Player;
			SlotChoice slot3 = SlotChoice.None;
			SlotChoice slot4 = SlotChoice.None;

			txtblock.Text = "Pla";
		}

		private void Click_Choose_level(object sender, RoutedEventArgs e)
		{

        }

		private void Slot_1_user(object sender, RoutedEventArgs e)
		{

			Button button = sender as Button;
			if (button != null && button.Content != null)
			{
				string selectedChoice = button.Content.ToString();

				switch (selectedChoice)
				{
					case "Player":

						button.Content = "AI";
						txtblock.Text = "AI";
						break;

					case "AI":

						button.Content = "None";
						txtblock.Text = "---";
						break;

					case "None":

						button.Content = "Player";
						txtblock.Text = "Player1";
						break;

					default:
						break;
				}
			}
		}

		private void ComboBox_slot1User_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			
			ComboBox comboBox = sender as ComboBox;
			if (comboBox != null && comboBox.SelectedItem != null)
			{
				string selectedChoice = (comboBox.SelectedItem as ComboBoxItem).Content.ToString();
				//txtblock.Text = "hej";

				switch (selectedChoice)
				{
					case "Player":

						//txtblock.Text = "player!!";
						break;

					case "AI":
						
						
						break;

					case "None":
						
						
						break;

					default:
						break;
				}
			}
		}
	}
}
