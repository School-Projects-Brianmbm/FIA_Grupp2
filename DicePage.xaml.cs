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
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FIA_Grupp2
{

    public sealed partial class DicePage : Page
    {
        private DispatcherTimer _diceTimer = new DispatcherTimer();
        private int _amountOfSpins = 0;
        private int _realDiceNumber;

        object _sender;

        public DicePage()
        {
            this.InitializeComponent();

            _diceTimer.Tick += Timer_Tick;
            _diceTimer.Interval = TimeSpan.FromMilliseconds(300);
        }

        private void Dice_Click(object sender, RoutedEventArgs e)
        {
            _realDiceNumber = GetRandomDiceNumber();
            _sender = sender;

            PlayRandomSequence(4.0f);
        }

        private void Timer_Tick(object sender, object e)
        {
            if(_amountOfSpins <= 1)
            {
                _diceTimer.Stop();

                ChangeDiceIcon($"ms-appx:///Assets/Dice_images/{GetImageFromDiceNumber(_realDiceNumber)}.png");
            }
            else
            {
                _amountOfSpins--;

                int diceNumber = GetRandomDiceNumber();

                ChangeDiceIcon($"ms-appx:///Assets/Dice_images/{GetImageFromDiceNumber(diceNumber)}.png");
            }
        }

        private string GetImageFromDiceNumber(int number)
        {
            switch(number)
            {
                case 1:
                    return "dice_one";
                case 2:
                    return "dice_two";
                case 3:
                    return "dice_three";
                case 4:
                    return "dice_four";
                case 5:
                    return "dice_five";
                case 6:
                    return "dice_six";
            }

            return null;
        }

        private int GetRandomDiceNumber()
        {
            Random r = new Random();
            return r.Next(1, 7);
        }

        private void PlayRandomSequence(float seconds)
        {
            _amountOfSpins = (int)(seconds / ((float)(_diceTimer.Interval.Milliseconds) / 1000f));
            _diceTimer.Start();
        }

        private void ChangeDiceIcon(string path)
        {
            BitmapImage newDiceImage = new BitmapImage(new Uri(path));

            // Find the Image control inside the Button that has been pressed.
            Image imageControl = (Image)((Button)_sender).Content;

            // Update the Source property of the Button image control 
            imageControl.Source = newDiceImage;
        }
    }
}
