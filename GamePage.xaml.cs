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
using System.ComponentModel;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FIA_Grupp2
{
    public sealed partial class GamePage : Page
    {
        bool gameIsOver = false;
        public static GamePage Instance;

        int MouseX, MouseY;
        int aiDelay = 100;
        static int nrOfPlayers = 4;
        static int currentTeam = 0;
        static GameBoardGrid gameGrid;

        private bool toggleIngameMenu = false;
        private bool toggleVolumeButton = true;

        private double _musicVolume = 100;

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
        public Playlist gameAudio;

        private Dice _dice;

        private bool isTurnTimerEnabled = false;
        private bool isGameTimerEnabled = false;
        private DispatcherTimer gameTimer;
        private DispatcherTimer turnTimer;
        private TimeSpan remainingGameTime;
        private TimeSpan remainingTurnTime;
        private int gameHours, gameMinutes, gameSeconds;
        private int turnHours, turnMinutes, turnSeconds;
        private bool isCows = false, isHens = false, isSheeps = false, isPigs = false;
        private bool isCowsAi = false, isHensAi = false, isSheepsAi = false, isPigsAi = false;
        private string slot1Usertype, slot2Usertype, slot3Usertype, slot4Usertype;
        private string slot1Username, slot2Username, slot3Username, slot4Username;
        private string slot1Team, slot2Team, slot3Team, slot4Team;

        public double MusicVolume
        {
            get => _musicVolume;
            set
            {
                if (_musicVolume != value)
                {
                    _musicVolume = value;
                }
            }
        }


        public GamePage()
        {
            InitializeComponent();
            Instance = this;
            Window.Current.CoreWindow.PointerMoved += CoreWindow_PointerMoved;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            layoutRoot.PointerWheelChanged += new PointerEventHandler(PointerWheelChanged);
            gameAudio = new Playlist();
            StartMusic();

            LoadGameSessionOptions();
            LoadLobbyOptions();
            SetAvailableTeams();
            nrOfPlayers = GetPlayerCount();
            teams = new Team[nrOfPlayers];
            InitiateGameTimer();
            InitiateTurnTimer();
            

            Loaded += MainPage_Loaded;

        }

        private void LoadLobbyOptions()
        {
            string lobbyData = (string)ApplicationData.Current.LocalSettings.Values["LobbyOptionsData"];
            if (!string.IsNullOrEmpty(lobbyData))
            {
                LobbyOptions lobbyOptionsData = JsonConvert.DeserializeObject<LobbyOptions>(lobbyData);
                slot1Usertype = lobbyOptionsData.slot1Usertype;
                slot2Usertype = lobbyOptionsData.slot2Usertype;
                slot3Usertype = lobbyOptionsData.slot3Usertype;
                slot4Usertype = lobbyOptionsData.slot4Usertype;

                slot1Username = lobbyOptionsData.slot1Username;
                slot2Username = lobbyOptionsData.slot2Username;
                slot3Username = lobbyOptionsData.slot3Username;
                slot4Username = lobbyOptionsData.slot4Username;

                slot1Team = lobbyOptionsData.slot1Team;
                slot2Team = lobbyOptionsData.slot2Team;
                slot3Team = lobbyOptionsData.slot3Team;
                slot4Team = lobbyOptionsData.slot4Team;
            }
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.Write("BAM MainPage Loaded");
            gameGrid = new GameBoardGrid(gameCanvas);
            gameGrid.CreateArrayOfPoints();
            gameGrid.CreateArrayOfDots();

            gameGrid.CalculateActualPositions();
            gameGrid.CalculateOrigoY();

            await StartGameDelay(0);
            gameGrid.SetEllipsesPositions();

            // Get the reference to your Storyboard (assuming you've named it "StartGameStoryboard")
            Storyboard myStoryboard = this.Resources["StartGameStoryboard"] as Storyboard;

            // Start the Storyboard
            myStoryboard.Begin();

            await StartGameDelay(2600);
            CreatePawns();

            // Add the elements to the canvas
            foreach (Team team in teams)
            {
                foreach (Pawn pawn in team.Pawns)
                    layoutRoot.Children.Add(pawn.PawnCanvas);
            }

            _dice = new Dice(this,diceButton);
            Debug.Write("Length of coarse is: " + gameGrid.CountCourseLength());
        }

        /// <summary>
        /// Create the Teams of different types (species)
        /// </summary>
        private void CreatePawns()
        {
            int index = 0;
            if (isCows)
            {
                if (isCowsAi)
                {
                    teams[index] = new Cows(gameGrid, globalCoarse, new Position(9, 9), goalPosition, true);
                }
                else
                {
                    teams[index] = new Cows(gameGrid, globalCoarse, new Position(9, 9), goalPosition);
                }
                index++;
            }

            if (isHens)
            {
                if (isHensAi)
                {
                    teams[index] = new Hens(gameGrid, globalCoarse, new Position(9, 1), goalPosition, true);
                }
                else
                {
                    teams[index] = new Hens(gameGrid, globalCoarse, new Position(9, 1), goalPosition);
                }
                index++;
            }

            if (isSheeps)
            {
                if (isSheepsAi)
                {
                    teams[index] = new Sheeps(gameGrid, globalCoarse, new Position(1, 1), goalPosition, true);
                }
                else
                {
                    teams[index] = new Sheeps(gameGrid, globalCoarse, new Position(1, 1), goalPosition);
                }
                index++;
            }

            if (isPigs)
            {
                if (isPigsAi)
                {
                    teams[index] = new Pigs(gameGrid, globalCoarse, new Position(1, 9), goalPosition, true);
                }
                else
                {
                    teams[index] = new Pigs(gameGrid, globalCoarse, new Position(1, 9), goalPosition);
                }
            }

            _dice = new Dice(this,diceButton);

            ChangeActiveTeamIcon(teams[currentTeam].Name);
            Debug.Write("Number of teams is: " + Team.NUMBER_OF_TEAMS);
            if (teams[currentTeam].IsAI && !gameIsOver)
            {
                _dice.SpinDice();
            }
        }

        private int GetPlayerCount()
        {
            int playerCount = 0;

            if (isCows)
            {
                playerCount++;
            }
            if (isSheeps)
            {
                playerCount++;
            }
            if (isHens)
            {
                playerCount++;
            }
            if (isPigs)
            {
                playerCount++;
            }

            return playerCount;
        }

        private void LoadGameSessionOptions()
        {
            string gameSessionOptionsData = (string)ApplicationData.Current.LocalSettings.Values["SessionOptionsData"];
            if (!string.IsNullOrEmpty(gameSessionOptionsData))
            {
                GameSessionOptions sessionOptionsData = JsonConvert.DeserializeObject<GameSessionOptions>(gameSessionOptionsData);
                gameHours = int.Parse(sessionOptionsData.GameTimeHours.ToString());
                gameMinutes = int.Parse(sessionOptionsData.GameTimeMinutes.ToString());
                gameSeconds = int.Parse(sessionOptionsData.GameTimeSeconds.ToString());
                turnHours = int.Parse(sessionOptionsData.TurnTimeHours.ToString());
                turnMinutes = int.Parse(sessionOptionsData.TurnTimeMinutes.ToString());
                turnSeconds = int.Parse(sessionOptionsData.TurnTimeSeconds.ToString());
            }
        }

        private void SetAvailableTeams()
        {
            if (slot1Usertype != "None")
            {
                if (slot1Team == "cow")
                {
                    isCows = true;
                    if (slot1Usertype == "AI") { isCowsAi = true; }
                }

            }
            if (slot2Usertype != "None")
            {

                if (slot2Team == "pig")
                {
                    isPigs = true;
                    if (slot2Usertype == "AI") { isPigsAi = true; }

                }

            }
            if (slot3Usertype != "None")
            {

                if (slot3Team == "chicken")
                {
                    isHens = true;
                    if (slot3Usertype == "AI") { isHensAi = true; }
                }

            }

            if (slot4Usertype != "None")
            {

                if (slot4Team == "sheep")
                {
                    isSheeps = true;
                    if (slot4Usertype == "AI") { isSheepsAi = true; }
                }
            }
        }

        private void InitiateGameTimer()
        {
            remainingGameTime = new TimeSpan(gameHours, gameMinutes, gameSeconds);
            if (remainingGameTime.TotalSeconds > 0)
            {
                isGameTimerEnabled = true;
            }

            if (isGameTimerEnabled)
            {
                gameTimer = new DispatcherTimer();
                gameTimer.Interval = TimeSpan.FromSeconds(1);
                gameTimer.Tick += GameTimerTick;
                gameTimer.Start();
            }
        }

        private void InitiateTurnTimer()
        {
            remainingTurnTime = new TimeSpan(turnHours, turnMinutes, turnSeconds);
            if (remainingTurnTime.TotalSeconds > 0)
            {
                isTurnTimerEnabled = true;
            }

            if (isTurnTimerEnabled)
            {
                turnTimer = new DispatcherTimer();
                turnTimer.Interval = TimeSpan.FromSeconds(1);
                turnTimer.Tick += TurnTimerTick;
                turnTimer.Start();
            }
        }

        private async void StartMusic()
        {
            await gameAudio.InitializePlaylist("Assets\\Sound\\InGame");
            gameAudio.SetVolume(_musicVolume / 100.0);
            gameAudio.StartPlayback();
        }

        private void DiceClicked(object sender, RoutedEventArgs e)
        {
            SoundEffect.PlayTrack(SoundEffect.DicePath);

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

            // OM INGEN ÄR HIT TEST VISIBLE
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

            if (teams[currentTeam].IsAI && !gameIsOver)
            {
                AiChoosePawn();
            }

            async void AiChoosePawn()
            {
                await Task.Delay(aiDelay); // Delay for 1 second
                foreach (Pawn pwn in teams[currentTeam].pawns)
                {
                    if (pwn.PawnCanvas.IsHitTestVisible == true)
                    {
                        pwn.AI_MadeItsChoise();
                        break;
                    }
                }
            }
        }

        public async Task StartGameDelay(int ms = 2000)
        { await Task.Delay(ms); }

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

            bool allPawnsInGoal = true;

            foreach (Pawn pawn in teams[currentTeam].Pawns)
            {
                if (!pawn.IsInGoal)
                {
                    allPawnsInGoal = false;
                }
            }

            if (allPawnsInGoal)
            {
                gameIsOver = true;
            }

            currentTeam++;
            if (currentTeam > nrOfPlayers - 1)
            {
                currentTeam = 0;
            }
            DebugTextUpdateModifier();

            ChangeActiveTeamIcon(teams[currentTeam].Name);
            if (isTurnTimerEnabled)
            {
                ResetTurnTimer();
            }

            if(!allPawnsInGoal)
            {
                switch (teams[currentTeam].Name)
                {
                    case "Cows":
                        SoundEffect.PlayTrack(SoundEffect.CowPath);
                        break;
                    case "Hens":
                        SoundEffect.PlayTrack(SoundEffect.ChickenPath);
                        break;
                    case "Sheeps":
                        SoundEffect.PlayTrack(SoundEffect.SheepPath);
                        break;
                    case "Pigs":
                        SoundEffect.PlayTrack(SoundEffect.PigPath);
                        break;
                }
            }
            
            if (teams[currentTeam].IsAI)
            {
                AiSpinDiceDelay();
            }

            async void AiSpinDiceDelay()
            {
                await Task.Delay(aiDelay);
                _dice.SpinDice();
            }
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

        private void ResetTurnTimer()
        {
            turnTimer.Stop();

            remainingTurnTime = new TimeSpan(turnHours, turnMinutes, turnSeconds);
            turnTimer = new DispatcherTimer();
            turnTimer.Interval = TimeSpan.FromSeconds(1);
            turnTimer.Tick += TurnTimerTick;
            turnTimer.Start();
        }




        private void CoreWindow_PointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            // DebugTextUpdate(args);
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
            /*
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
            */
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
                if (isTurnTimerEnabled)
                {
                    NextTeamsTurn();
                }
            }
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
        private void UpdateGameTimerText()
        {
            gameTimerText.Text = $"{remainingGameTime.Hours:D2}:{remainingGameTime.Minutes:D2}:{remainingGameTime.Seconds:D2}";
        }
        private void UpdateTurnTimerText()
        {
            turnTimerText.Text = $"{remainingTurnTime.Hours:D2}:{remainingTurnTime.Minutes:D2}:{remainingTurnTime.Seconds:D2}";
        }

        private void SetIngameMenuVisible(bool value)
        {
            toggleIngameMenu = value;
            if (value == true)
            {
                ingame_menu.Visibility = Visibility.Visible;
            }
            else
            {
                ingame_menu.Visibility = Visibility.Collapsed;
            }
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Escape)
            {
                SetIngameMenuVisible(!toggleIngameMenu);
            }
        }

        private void InGameMenuContinue(object sender, RoutedEventArgs e)
        {
            SoundEffect.PlayTrack(SoundEffect.ClickPath);

            SetIngameMenuVisible(false);
        }

        private void GameVolumeButtonClicked(object sender, RoutedEventArgs e)
        {
            SoundEffect.PlayTrack(SoundEffect.ClickPath);

            string path = $"ms-appx:///Assets/InGameIcons/";

            toggleVolumeButton = !toggleVolumeButton;

            if (toggleVolumeButton)
            {
                path += "volume.png";
                //_musicVolume = _musicSavedValue;
                gameAudio.SetVolume(_musicVolume / 100.0);
                
            }
            else
            {
                path += "volume-off.png";
                gameAudio.SetVolume(0.0);
            }

            BitmapImage newVolumeIconImage = new BitmapImage(new Uri(path));

            volumeButtonIcon.Source = newVolumeIconImage;

            volumeSlider.Value = _musicVolume;
        }

        private void OpenMainMenuButtonClicked(object sender, RoutedEventArgs e)
        {
            SoundEffect.PlayTrack(SoundEffect.ClickPath);
            SetIngameMenuVisible(true);
        }

        private void ReturnToMainMenuButtonClicked(object sender, RoutedEventArgs e)
        {
            SoundEffect.PlayTrack(SoundEffect.ClickPath);
            gameIsOver = true;
            gameAudio.StopPlayback();
            this.Frame.Navigate(typeof(StartPage));
        }

        private void IngameRulesButtonClicked(object sender, RoutedEventArgs e)
        {
            SoundEffect.PlayTrack(SoundEffect.ClickPath);

            SetIngameMenuVisible(true);

            rulesPage.Visibility = Visibility.Visible;
        }

        private void RulesPageBackButtonClicked(object sender, RoutedEventArgs e)
        {
            SoundEffect.PlayTrack(SoundEffect.ClickPath);

            SetIngameMenuVisible(true);

            rulesPage.Visibility = Visibility.Collapsed;
        }


        private void VolumeSliderValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            _musicVolume = volumeSlider.Value;
            

            string path = $"ms-appx:///Assets/InGameIcons/";
            if (_musicVolume > 0)
            {

                toggleVolumeButton = true;
                path += "volume.png";
            }
            else if (_musicVolume <= 0)
            {
                toggleVolumeButton = false;
                path += "volume-off.png";
            }

            BitmapImage newVolumeIconImage = new BitmapImage(new Uri(path));

            volumeButtonIcon.Source = newVolumeIconImage;

            if (volumeSliderValueText != null)
            {
                volumeSliderValueText.Text = $"{_musicVolume}%";
            }

            if (gameAudio != null)
            {
                gameAudio.SetVolume(0);
                gameAudio.SetVolume(_musicVolume / 100.0);
            }
        }
    }
}

