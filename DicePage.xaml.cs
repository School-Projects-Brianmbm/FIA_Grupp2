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
    //TODO: Document the code
    //TODO: Testa med en liten random animation tills den slutar med talet man fick.

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DicePage : Page
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        private int _amountOfCycles = 0;
        private int _realDiceNumber;

        object _sender;

        public DicePage()
        {
            this.InitializeComponent();

            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromMilliseconds(300);
        }

        private void Dice_Click(object sender, RoutedEventArgs e)
        {
            _realDiceNumber = GetRandomDiceNumber();
            Debug.WriteLine($"Got number : {_realDiceNumber}");
            _sender = sender;

            PlayRandomSequence(4.0f);
        }

        private void Timer_Tick(object sender, object e)
        {
            Debug.WriteLine($"Tick {_amountOfCycles}");
            if(_amountOfCycles <= 1)
            {
                _timer.Stop();

                BitmapImage newDiceImage = new BitmapImage(new Uri($"ms-appx:///Assets/Dice_images/{GetImageFromDiceNumber(_realDiceNumber)}.png"));

                // Find the Image control inside the Button
                Image imageControl = (Image)((Button)_sender).Content;

                // Update the Source property of the Image control
                imageControl.Source = newDiceImage;
            }
            else
            {
                _amountOfCycles--;

                int diceNumber = GetRandomDiceNumber();

                BitmapImage newDiceImage = new BitmapImage(new Uri($"ms-appx:///Assets/Dice_images/{GetImageFromDiceNumber(diceNumber)}.png"));

                // Find the Image control inside the Button
                Image imageControl = (Image)((Button)_sender).Content;

                // Update the Source property of the Image control
                imageControl.Source = newDiceImage;
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
            _amountOfCycles = (int)(seconds / ((float)(_timer.Interval.Milliseconds) / 1000f));
            _timer.Start();
        }
    }
}
