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
using Windows.UI.Xaml.Documents;
using Windows.Devices.Pwm;
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;
using System.Collections.Generic;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FIA_Grupp2
{
    public sealed partial class GamePage : Page
    {
        public static GamePage Instance;

        int MouseX, MouseY;
        static int nrOfPlayers = 4;
        static int currentTeam = 0;
        static GameBoardGrid gameGrid;

        Position goalPosition = new Position(5, 5);

        static Position[] globalCoarse = new Position[44]
        {
            new Position(6, 10), new Position(6, 9), new Position(6, 8), new Position(6, 7), new Position(6, 6), new Position(7, 6), new Position(8, 6), new Position(9, 6),
            new Position(10, 6), new Position(10, 5), new Position(10, 4), new Position(9, 4), new Position(8, 4), new Position(7, 4), new Position(6, 4), new Position(6, 3),
            new Position(6, 2), new Position(6, 1), new Position(6, 0), new Position(5, 0), new Position(4, 0), new Position(4, 1), new Position(4, 2), new Position(4, 3),
            new Position(4, 4), new Position(3, 4), new Position(2, 4), new Position(1, 4), new Position(0, 4), new Position(0, 5), new Position(0, 6), new Position(1, 6),
            new Position(2, 6), new Position(3, 6), new Position(4, 6), new Position(4, 7), new Position(4, 8), new Position(4, 9), new Position(4, 10), new Position(5, 10),
            new Position(5, 9),new Position(5, 8),new Position(5, 7),new Position(5, 6)
        };

        Team[] teams = new Team[nrOfPlayers];
        // Team cows, hens, sheeps, pigs;

        private Dice _dice;

        private DispatcherTimer gameTimer;
        private DispatcherTimer turnTimer;
        private TimeSpan remainingGameTime;
        private TimeSpan remainingTurnTime;
        public Playlist gameAudio;
        
        public GamePage()
        {
            InitializeComponent();
            Instance = this;

            Window.Current.CoreWindow.PointerMoved += CoreWindow_PointerMoved;
            layoutRoot.PointerWheelChanged += new PointerEventHandler(PointerWheelChanged);
            Loaded += MainPage_Loaded;
            gameAudio = new Playlist();
            StartMusic();

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

        private async void StartMusic()
        {
            await gameAudio.InitializePlaylist("Assets\\Sound\\InGame");
            gameAudio.StartPlayback();
        }


        private void DiceClicked(object sender, RoutedEventArgs e)
        {
            _dice.SpinDice(sender, e);
        }

        public void DiceFinishedSpinning(object sender, EventArgs e)
        {
            diceButton.IsEnabled = false;
            // FÖR VAR PJÄS I TEAMS
            foreach (Pawn pwn in teams[currentTeam].pawns)
            {
                // OM INTE ETT OCH INTE SEX
                if (_dice.DiceNumber != 1 && _dice.DiceNumber != 6)
                {
                    // OCH ALLA ÄR I BOET
                    if (teams[currentTeam].GetPawnsOnTheBoard().Length <= 0)
                    {
                        AutoPass();
                        return; // Hoppa ut ur loopen, för ingen ska flyttas
                    }
                    // OM PJÄSEN ÄR I BOET
                    if (pwn.Steps == 0)
                    {
                        pwn.PawnCanvas.IsHitTestVisible = false;
                        continue;  // Hoppa direkt till att undersöka nästa pjäs
                    }
                    // OM NÅGON ÄR UR BOET LÅT DEN GÅ Såvida inte går utanför corase
                    else if (teams[currentTeam].GetPawnsOnTheBoard().Length > 0 && pwn.Steps + _dice.DiceNumber < globalCoarse.Length + 2)
                    {
                        pwn.TurnStepsLeft = _dice.DiceNumber;
                        pwn.PawnCanvas.IsHitTestVisible = true;
                    }
                    else
                    {
                        Debug.Write("FÖR MÅNGA STEG FÖR ATT GÅ JÄMNT I MÅL 1");
                        pwn.PawnCanvas.IsHitTestVisible = false;
                    }
                }
                // OM ETT ELLER SEX
                else if (_dice.DiceNumber == 1 || _dice.DiceNumber == 6)
                {
                    // OCH INGEN PÅ BORDET MEN NÅGON I BOET
                    if (teams[currentTeam].GetPawnsOnTheBoard().Length <= 0 &&
                        teams[currentTeam].GetPawnsInTheNest().Length >= 0)
                    {
                        //Debug.WriteLine("There is still pawns in the nest, and i can move one out");
                        // FIXME : Maybe we want to allow only one step if we get a 6 from the dice.
                        pwn.TurnStepsLeft = _dice.DiceNumber;
                        pwn.PawnCanvas.IsHitTestVisible = true;
                    }
                    // OM NÅGON PÅ BORDET LÅT DEN GÅ Såvida inte går utanför corase
                    else if (pwn.Steps + _dice.DiceNumber < globalCoarse.Length + 2)
                    {
                        pwn.TurnStepsLeft = _dice.DiceNumber;
                        pwn.PawnCanvas.IsHitTestVisible = true;
                    }
                    else
                    {
                        Debug.Write("FÖR MÅNGA STEG FÖR ATT GÅ JÄMNT I MÅL 2");
                        pwn.PawnCanvas.IsHitTestVisible = false;
                    }
                    // TODO KOLLA OM NÅGON ÄR I VÄGEN
                }
            }

            // TODO OM INGEN ÄR HIT TEST VISIBLE

            if (!AnyHitTestVisible())
            {
                AutoPass();
            }

            bool AnyHitTestVisible()
            {
                foreach (Pawn pwn in teams[currentTeam].pawns)
                {
                    if (pwn.PawnCanvas.IsHitTestVisible)
                    {
                        return true; 
                    }
                }
                return false;
            }

            void AutoPass()
            {
                Debug.WriteLine("No valid move avilable");
                // To the next team but through an delay
                NextTeamsTurnDelay();
            }

        }

        public async void NextTeamsTurnDelay(int ms = 500)
        {
            diceCross.Visibility = Visibility.Visible;
            await Task.Delay(ms);
            // Here i want a delay before the NextTeamsTurn Executes ?
            NextTeamsTurn();
            diceCross.Visibility = Visibility.Collapsed;
        }

        public void NextTeamsTurn()
        {
            diceButton.IsEnabled = true;
            // int aprioTeam = currentTeam;
            Debug.WriteLine("");
            // Note to embeed actuall functionality in a Debug.Write cant be good practice.
            CheckOtherTeamsPositions(teams[currentTeam]);

            //IsThereAPawnOnThisPosition(new Position(6, 10));

            currentTeam++;
            if (currentTeam > nrOfPlayers - 1)
            {
                currentTeam = 0;
            }
            DebugTextUpdateModifier();

            ChangeActiveTeamIcon(teams[currentTeam].Name);
        }

        private string ConvertNameToJPG(string teamName)
        {
            switch (teamName)
            {
                case "Cows":
                    return "cow";
                case "Hens":
                    return "chicken";
                case "Sheeps":
                    return "sheep";
                case "Pigs":
                    return "pig";
            }

            return string.Empty;
        }

        //TODO: Maybe there is a color property somewhere, and this method is useless.
        private Color GetColorFromTeamName(string teamName)
        {
            switch (teamName)
            {
                case "Cows":
                    return ColorHelper.FromArgb(255, 144, 238, 144);
                case "Hens":
                    return ColorHelper.FromArgb(255, 255, 255, 0);
                case "Sheeps":
                    return ColorHelper.FromArgb(255, 173, 216, 230);
                case "Pigs":
                    return ColorHelper.FromArgb(255, 219, 112, 147);
            }
            return Colors.Black;
        }

        private void ChangeActiveTeamIcon(string teamName)
        {
            BitmapImage newActiveTeamIcon = new BitmapImage(new Uri($"ms-appx:///Assets/TeamIcons/{ConvertNameToJPG(teamName)}.jpg"));
            activeTeamIcon.Source = newActiveTeamIcon;

            activeDiceBorder.Background = new SolidColorBrush(GetColorFromTeamName(teamName));
            activeTeamIconBorder.Background = new SolidColorBrush(GetColorFromTeamName(teamName));

            _dice.NewTurn();
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
            //gameGrid.SetEllipsesPositions(true,showInd: true);

            CreatePawns();

            // Add the elements to the canvas
            foreach (Team team in teams)
            {
                foreach (Pawn pawn in team.Pawns)
                    layoutRoot.Children.Add(pawn.PawnCanvas);
            }

            _dice = new Dice(this);
            Debug.Write("Length of coarse is: " + gameGrid.CountCourseLength());
            //Debug.Write(gameGrid.GetActualPositionOf(10, 10) + "\n");
        }

        /// <summary>
        /// Create the Teams of different types (species)
        /// </summary>
        bool isCows = true, isHens = true, isSheeps = true, isPigs = true;
        // TODO replace above hardcoded with values passed from loby so we only create the teams chosen by a player
        private void CreatePawns()
        {
            for (int i = 0; i < teams.Length; i++)
            {
                switch (i)
                {
                    case 0 when isCows:
                        teams[i] = new Cows(gameGrid, globalCoarse, new Position(9, 9), goalPosition);
                        break;
                    case 1 when isHens:
                        teams[i] = new Hens(gameGrid, globalCoarse, new Position(9, 1), goalPosition);
                        break;
                    case 2 when isSheeps:
                        teams[i] = new Sheeps(gameGrid, globalCoarse, new Position(1, 1), goalPosition);
                        break;
                    case 3 when isPigs:
                        teams[i] = new Pigs(gameGrid, globalCoarse, new Position(1, 9), goalPosition);
                        break;
                    default:
                        teams[i] = null;
                        break;
                }
            }

            _dice = new Dice(this);

            ChangeActiveTeamIcon(teams[currentTeam].Name);

            Debug.Write("Length of coarse is: " + gameGrid.CountCourseLength());
            Debug.Write(Team.NUMBER_OF_TEAMS);
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
                debugtext.Text = $"Mouse Position: X={MouseX}, Y={MouseY}, {gameGrid.Squish} Curent team is: {teams[currentTeam].Name}";
            }
            MouseX = roundedX;
            MouseY = roundedY;
        }

        private void DebugTextUpdateModifier()
        {
            debugtext.Text = $"Mouse Position: X={MouseX}, Y={MouseY}, {gameGrid.Squish} Curent team is: {teams[currentTeam].Name}";
        }

        private new void PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // NextTeamsTurn();
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

        private Team CheckOtherTeamsPositions(Team currentActiveTeam)
        {
            foreach (Team team in teams)
            {
                if (team.Name != currentActiveTeam.Name)
                {
                    foreach (Pawn activePawn in currentActiveTeam.Pawns)
                    {
                        foreach (Pawn opposingPawn in team.Pawns)
                        {
                            if (opposingPawn.IsInGoal == false &&
                               activePawn.CurrentPosition.X == opposingPawn.CurrentPosition.X &&
                               activePawn.CurrentPosition.Y == opposingPawn.CurrentPosition.Y)
                            {
                                Debug.WriteLine($"{currentActiveTeam.Name} attacked : {currentActiveTeam.Name}");
                                opposingPawn.Steps = 0;
                                opposingPawn.ResetDirection();
                                opposingPawn.ReplaceImage();
                                opposingPawn.PositionAtNest();
                                return team;
                            }
                        }
                    }
                }
                else
                {
                    continue;
                }
            }

            return null;
        }
    }
}
