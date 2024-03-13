using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FIA_Grupp2
{
    public class Dice
    {
        /// <summary>
        /// Represents a dice that can be rolled to generate random numbers.
        /// </summary>
        private DispatcherTimer _diceTimer = new DispatcherTimer();
        private int _amountOfSpinsLeft = 0;
        private int _realDiceNumber;

        private object _sender;

        private double _diceSpinTickRate = 100.0;   //How fast will each tick be, in milliseconds, i.e. after 100 milliseconds a tick event is called.


        /// <summary>
        /// Event triggered when the dice finishes spinning.
        /// </summary>
        public event EventHandler DiceFinished;

        /// <summary>
        /// Returns the number that the dice fell on.
        /// </summary>
        public int DiceNumber
        {
            get { return _realDiceNumber; }
        }

        /// <summary>
        /// Initializes a new instance of the Dice class.
        /// </summary>
        /// <param name="gamePage">The game page where the dice is used.</param>
        public Dice(GamePage gamePage, Button diceButton)
        {
            _diceTimer = new DispatcherTimer();
            _diceTimer.Tick += Timer_Tick;
            _diceTimer.Interval = TimeSpan.FromMilliseconds(_diceSpinTickRate);
            _sender = diceButton;
            this.DiceFinished += gamePage.DiceFinishedSpinning;  //Subscribe a function from gamepage
        }
        /// <summary>
        /// Initiates the dice spinning animation.
        /// </summary>
        public void SpinDice(object sender, RoutedEventArgs e)
        {
            _sender = sender;
            PlayRandomSequence(0.5f);
        }

        public void SpinDice()
        {
            // _sender = sender;
            PlayRandomSequence(0.5f);
        }

        private void Timer_Tick(object sender, object e)
        {
            if (_amountOfSpinsLeft <= 1)
            {
                _diceTimer.Stop();
                _realDiceNumber = GetRandomDiceNumber();

                ChangeDiceIcon($"ms-appx:///Assets/Dice_images/{GetImageFromDiceNumber(_realDiceNumber)}.png");

                OnDiceFinished();   //Fire the event, when the dice has finished spinning.
            }
            else
            {
                _amountOfSpinsLeft--;
            }
        }

        /// <summary>
        /// Event invocation when the dice finishes spinning.
        /// </summary>
        private void OnDiceFinished()
        {
            DiceFinished?.Invoke(null, EventArgs.Empty);    //Invoke the functions that's the been subscribed, when the dice has finished spinning.
        }

        /// <summary>
        /// Gets the image path corresponding to the dice number.
        /// </summary>
        /// <param name="number">The dice number.</param>
        /// <returns>The image path.</returns>
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

        /// <summary>
        /// Generates a random dice number.
        /// </summary>
        /// <returns>A random dice number.</returns>
        private int GetRandomDiceNumber()
        {
            Random r = new Random();
            return r.Next(1, 7);
            //return r.Next(1, 7);
        }

        /// <summary>
        /// Initiates a random sequence of dice spinning animations.
        /// </summary>
        private void PlayRandomSequence(float seconds)
        {
            _amountOfSpinsLeft = (int)(seconds / ((float)(_diceTimer.Interval.Milliseconds) / 250f));
            _diceTimer.Start();
            ChangeDiceIcon($"ms-appx:///Assets/Dice_images/random_spin_1.gif");
        }

        /// <summary>
        /// Changes the dice icon to the specified image path.
        /// </summary>
        /// <param name="path">The image path.</param>
        private void ChangeDiceIcon(string path)
        {
            try
            {
                BitmapImage newDiceImage = new BitmapImage(new Uri(path));

                if (_sender != null)
                {
                    // Find the Image control inside the Button that has been pressed.
                    //Image imageControl = diceButton;
                    Image imageControl = (Image)((Button)_sender).Content;

                    // Update the Source property of the Button image control 
                    imageControl.Source = newDiceImage;
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Upon a new turn, this will reset the icon to signal that the dice can be thrown again
        /// </summary>
        public void NewTurn()
        {
            ChangeDiceIcon($"ms-appx:///Assets/Dice_images/white_dice.png");
        }
    }
}