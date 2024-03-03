using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FIA_Grupp2
{
    public sealed partial class GamePage : Page
    {
        int MouseX, MouseY;
        GameBoardGrid gameGrid;

        private Dice _dice;

		private DispatcherTimer gameTimer;
		private DispatcherTimer turnTimer;
		private TimeSpan remainingGameTime;
		private TimeSpan remainingTurnTime;

		public GamePage()
        {
            InitializeComponent();
            Window.Current.CoreWindow.PointerMoved += CoreWindow_PointerMoved;
            layoutRoot.PointerWheelChanged += new PointerEventHandler(PointerWheelChanged);
            Loaded += MainPage_Loaded;

            //Get saved game session options data
			string serializedData = (string)ApplicationData.Current.LocalSettings.Values["SessionOptionsData"];
			if (!string.IsNullOrEmpty(serializedData))
			{
				GameSessionOptions sessionOptionsData = JsonConvert.DeserializeObject<GameSessionOptions>(serializedData);
				int gameHours = int.Parse(sessionOptionsData.GameTimeHours.ToString());
				int gameMinutes = int.Parse(sessionOptionsData.GameTimeMinutes.ToString());
				int gameSeconds = int.Parse(sessionOptionsData.GameTimeSeconds.ToString());
				int turnHours = int.Parse(sessionOptionsData.TurnTimeHours.ToString());
				int turnMinutes = int.Parse(sessionOptionsData.TurnTimeMinutes.ToString());
				int turnSeconds = int.Parse(sessionOptionsData.TurnTimeSeconds.ToString());

				remainingGameTime = new TimeSpan(gameHours, gameMinutes, gameSeconds);
				remainingTurnTime = new TimeSpan(turnHours, turnMinutes, turnSeconds);

				gameTimer = new DispatcherTimer();
				gameTimer.Interval = TimeSpan.FromSeconds(1);
				gameTimer.Tick += GameTimerTick;
				gameTimer.Start();

				turnTimer = new DispatcherTimer();
				turnTimer.Interval = TimeSpan.FromSeconds(1);
				turnTimer.Tick += TurnTimerTick;
				turnTimer.Start();
			}
		}

        private void DiceClicked(object sender, RoutedEventArgs e)
        {
            _dice.SpinDice(sender, e);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.Write("BAM MainPage Loaded");
            gameGrid = new GameBoardGrid(gameCanvas);
            gameGrid.CreateArrayOfPoints();
            gameGrid.CreateArrayOfDots();

            gameGrid.CalculateActualPositions();
            gameGrid.CalculateOrigoY();
            gameGrid.SetEllipsesPositions();

            Debug.Write(gameGrid.GetActualPositionOf(10, 10) + "\n");

            _dice = new Dice();
        }



        private void CoreWindow_PointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            DebugTextUpdate(args);
        }

        private void DebugTextUpdate(PointerEventArgs args)
        {
            // Get the mouse pointer position relative to your app window
            var x = args.CurrentPoint.Position.X;
            var y = args.CurrentPoint.Position.Y;
            int roundedX = (int)Math.Round(x);
            int roundedY = (int)Math.Round(y);
            debugtext.Text = $"Mouse Position: X={roundedX}, Y={roundedY}, {gameGrid.Squish}";
            MouseX = roundedX;
            MouseY = roundedY;
        }

        private void DebugTextUpdateModifier()
        {
            debugtext.Text = $"Mouse Position: X={MouseX}, Y={MouseY}, {gameGrid.Squish}";
        }


        private new void PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            // Get the delta value to determine whether the wheel scrolls up or down
            var delta = e.GetCurrentPoint(layoutRoot).Properties.MouseWheelDelta;

            if (delta > 0)
            {
                gameGrid.Squish--;
            }
            else if (delta < 0)
            {
                gameGrid.Squish++;
            }

            gameGrid.CalculateColumnDist();
            gameCanvas.Children.Clear();

            gameGrid.CreateArrayOfPoints();
            gameGrid.CreateArrayOfDots();

            gameGrid.CalculateOrigoY();
            gameGrid.CalculateActualPositions();
            gameGrid.SetEllipsesPositions();

            DebugTextUpdateModifier();

            Debug.Write(gameGrid.GetActualPositionOf(10, 10) + "\n");
        }

		private void GameTimerTick(object sender, object e)
		{
			if (remainingGameTime.TotalSeconds > 0)
			{
				remainingGameTime = remainingGameTime.Subtract(TimeSpan.FromSeconds(1));
				UpdateGameTimerText();
			}
			else
			{
				gameTimer.Stop();
				// TODO Stop the game as the gametimer has run out.
			}
		}

		private void TurnTimerTick(object sender, object e)
		{
			if (remainingTurnTime.TotalSeconds > 0)
			{
				remainingTurnTime = remainingTurnTime.Subtract(TimeSpan.FromSeconds(1));
				UpdateTurnTimerText();
			}
			else
			{
				gameTimer.Stop();
				// TODO Stop the game as the gametimer has run out.
			}
		}

		private void UpdateGameTimerText()
		{
			gameTimerText.Text = $"{remainingGameTime.Hours:D2}:{remainingGameTime.Minutes:D2}:{remainingGameTime.Seconds:D2}";
		}
		private void UpdateTurnTimerText()
		{
			turnTimerText.Text = $"{remainingTurnTime.Hours:D2}:{remainingTurnTime.Minutes:D2}:{remainingTurnTime.Seconds:D2}";
		}
	}
}
