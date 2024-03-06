using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Composition;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FIA_Grupp2
{
    public sealed partial class GamePage : Page
    {
        int MouseX, MouseY;

        static GameBoardGrid gameGrid;

        Position goalPosition = new Position(5, 5);

        static Position[] globalCoarse = new Position[44]
        {
            new Position(10, 6),new Position(9, 6),new Position(8, 6),new Position(7, 6),new Position(6, 6),new Position(6, 7),new Position(6, 8),new Position(6, 9),
            new Position(6, 10),new Position(5, 10),new Position(4, 10),new Position(4, 9),new Position(4, 8),new Position(4, 7),new Position(4, 6),new Position(3, 6),
            new Position(2, 6),new Position(1, 6),new Position(0, 6),new Position(0, 5),new Position(0, 4),new Position(1, 4),new Position(2, 4),new Position(3, 4),
            new Position(4, 4),new Position(4, 3),new Position(4, 2),new Position(4, 1),new Position(4, 0),new Position(5, 0),new Position(6, 0),new Position(6, 1),
            new Position(6, 2),new Position(6, 3),new Position(6, 4),new Position(7, 4),new Position(8, 4),new Position(9, 4),new Position(10, 4),new Position(10, 5),
            new Position(9, 5),new Position(8, 5),new Position(7, 5),new Position(6, 5)
        };

        Team cows, sheeps;

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
            string gameSessionOptionsData = (string)ApplicationData.Current.LocalSettings.Values["SessionOptionsData"];
            if (!string.IsNullOrEmpty(gameSessionOptionsData))
            {
                GameSessionOptions sessionOptionsData = JsonConvert.DeserializeObject<GameSessionOptions>(gameSessionOptionsData);
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

            //Get saved lobby options data
            string lobbyData = (string)ApplicationData.Current.LocalSettings.Values["LobbyOptionsData"];
            if (!string.IsNullOrEmpty(lobbyData))
            {
                LobbyOptions lobbyOptionsData = JsonConvert.DeserializeObject<LobbyOptions>(lobbyData);
                //TODO: Initiate the settings from lobbydata
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
            gameGrid.SetEllipsesPositions(true, false, true);
            //gameGrid.SetEllipsesPositions(true,showInd: true);

            cows = new Cows(gameGrid, globalCoarse, new Position(9, 9), goalPosition);
            sheeps = new Sheeps(gameGrid, globalCoarse, new Position(1, 1), goalPosition);

            layoutRoot.Children.Add(cows.Pawn.PawnCanvas);
            layoutRoot.Children.Add(sheeps.Pawn.PawnCanvas);

            _dice = new Dice();
            Debug.Write("Length of coarse is: " + gameGrid.CountCourseLength());
            //Debug.Write(gameGrid.GetActualPositionOf(10, 10) + "\n");
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
            if (gameGrid != null)
            {
                debugtext.Text = $"Mouse Position: X={roundedX}, Y={roundedY}, {gameGrid.Squish}";
            }
            MouseX = roundedX;
            MouseY = roundedY;
        }


        private void DebugTextUpdateModifier()
        {
            debugtext.Text = $"Mouse Position: X={MouseX}, Y={MouseY}, {gameGrid.Squish}";
        }

        private new void PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            cows.Pawn.Step();
            sheeps.Pawn.Step();
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

            //Debug.Write(gameGrid.GetActualPositionOf(10, 10) + "\n");
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
