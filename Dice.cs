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

namespace FIA_Grupp2
{
    public class Dice
    {
        private DispatcherTimer _diceTimer = new DispatcherTimer();
        private int _amountOfSpinsLeft = 0;
        private int _realDiceNumber;

        private object _sender;

        private double _diceSpinTickRate = 100.0;   //How fast will each tick be, in milliseconds, i.e. after 100 milliseconds a tick event is called.

        public void SpinDice(object sender, RoutedEventArgs e)
        {
            _diceTimer = new DispatcherTimer();
            _diceTimer.Tick += Timer_Tick;
            _diceTimer.Interval = TimeSpan.FromMilliseconds(_diceSpinTickRate);

            _realDiceNumber = GetRandomDiceNumber();
            _sender = sender;

            PlayRandomSequence(4.0f);
        }

        private void Timer_Tick(object sender, object e)
        {
            if (_amountOfSpinsLeft <= 1)
            {
                _diceTimer.Stop();

                ChangeDiceIcon($"ms-appx:///Assets/Dice_images/{GetImageFromDiceNumber(_realDiceNumber)}.png");
            }
            else
            {
                _amountOfSpinsLeft--;
            }
        }

        private string GetImageFromDiceNumber(int number)
        {
            switch (number)
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
            _amountOfSpinsLeft = (int)(seconds / ((float)(_diceTimer.Interval.Milliseconds) / 1000f));
            _diceTimer.Start();
            ChangeDiceIcon($"ms-appx:///Assets/Dice_images/random_spin_1.gif");
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
