using System;
using System.Diagnostics;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace FIA_Grupp2
{

    internal class Pawn
    {
        protected bool canWalk = false;
        protected static int GLOBAL_INDEX = 0;
        protected int globalIndex;
        protected int localIndex;
        protected string Name = "Pawn";
        protected bool isInGoal = false;
        protected int direction;
        protected Position indexPosition;
        protected GameBoardGrid boardgrid;
        protected int steps = 0;
        private int turnStepsLeft = 0;
        internal int TurnStepsLeft { get => turnStepsLeft; set => turnStepsLeft = value; }

        public Position[] coarse;

        protected Canvas pawnCanvas = new Canvas { Height = 80, Width = 80 };
        protected Image pawnImage = new Image();

        internal Image[] pawnImages = new Image[4];

        public Canvas PawnCanvas { get => pawnCanvas; set => pawnCanvas = value; }
        public Image PawnImage { get => pawnImage; set => pawnImage = value; }
        Team team;

        public bool AtNest { get { return Steps == 0; }
        }

        internal int Steps { get => steps; set => steps = value; }

        public Pawn(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse, in Team myTeam)
        {
            team = myTeam;
            PawnCanvas.IsHitTestVisible = false;
            globalIndex = GLOBAL_INDEX++;
            pawnCanvas.Children.Add(pawnImage);
            indexPosition = startpos;
            boardgrid = gbg;
            PositionAtNest();
            coarse = teamcoarse;
            pawnCanvas.PointerPressed += Canvas_PointerPressed;
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.OriginalSource is UIElement clickedElement)
            {
                Debug.Write($"{globalIndex} {localIndex} {Name} is clicked And canWalk = {canWalk}\n");
                Step();
                CheckWinner();

                // Calling for the next teams turn
                GamePage.Instance.NextTeamsTurn();
            }
        }

        private void CheckWinner()
        {
            bool allPawnsInGoal = true;

            foreach (Pawn pawn in team.Pawns)
            {

                if (!pawn.isInGoal)
                {
                    allPawnsInGoal = false;
                }

                pawn.pawnCanvas.IsHitTestVisible = false;
            }
            if (allPawnsInGoal)
            {
                

                Debug.Write("All pawns are in goal");
                Debug.Write("Current team is: " + team.Name);
                
                var winnerParameters = Tuple.Create(team.Name, GamePage.Instance.gameAudio);
                GamePage.Instance.Frame.Navigate(typeof(WinnerPage), winnerParameters);
            }
        }

        public virtual void PositionAtNest()
        {
            Point pos = boardgrid.GetActualPositionOf(indexPosition.X, indexPosition.Y);
            double newX = pos.X;
            double newY = pos.Y;
            Canvas.SetLeft(PawnImage, newX + 8 + (10 * localIndex));
            Canvas.SetTop(PawnImage, newY - 10 + (10 * localIndex));
        }
        public virtual void PositionAt(Position ind)
        {
            Point pos = boardgrid.GetActualPositionOf(ind.X, ind.Y);
            double newX = pos.X;
            double newY = pos.Y;
            Canvas.SetLeft(PawnImage, newX + 8);
            Canvas.SetTop(PawnImage, newY - 10);
        }
        internal virtual void Step()
        {
            while (turnStepsLeft-- > 0)
            {

                if (isInGoal)
                {
                    Debug.Write($"Im home already :)");
                }
                else if (Steps == coarse.Length - 1)
                {
                    PositionAt(coarse[Steps++]);
                    isInGoal = true;
                }
                else
                {
                    // Debug.Write($"\n");
                    // Debug.Write($"Direction: {direction} ");
                    // Debug.Write($" From: {coarse[steps]} ");
                    // Debug.Write($" To: {coarse[steps + 1]} ");
                    if (!IsSameDirection())
                    {
                        if (direction == 2 && coarse[Steps + 1].X < coarse[Steps].X) { TurnImageLeft(); }
                        else if (direction == 2 && coarse[Steps + 1].X > coarse[Steps].X) { TurnImageRight(); }
                        else if (direction == 3 && coarse[Steps + 1].Y < coarse[Steps].Y) { TurnImageLeft(); }
                        else if (direction == 3 && coarse[Steps + 1].Y > coarse[Steps].Y) { TurnImageRight(); }
                        else if (direction == 0 && coarse[Steps + 1].X < coarse[Steps].X) { TurnImageRight(); }
                        else if (direction == 0 && coarse[Steps + 1].X > coarse[Steps].X) { TurnImageLeft(); }
                        else if (direction == 1 && coarse[Steps + 1].Y < coarse[Steps].Y) { TurnImageRight(); }
                        else if (direction == 1 && coarse[Steps + 1].Y > coarse[Steps].Y) { TurnImageLeft(); }
                    }
                    PositionAt(coarse[Steps++]);
                }
            }
        }
        internal virtual bool IsSameDirection()
        {
            if (direction == 3 || direction == 1)
            {
                // Debug.Write($" Y ");
                if (coarse[Steps].Y == coarse[Steps + 1].Y) { return true; }
                else { return false; }
            }
            else if (direction == 2 || direction == 0)
            {
                // Debug.Write($" X ");
                if (coarse[Steps].X == coarse[Steps + 1].X) { return true; }
                else { return false; }
            }
            return false;
        }
        internal virtual void TurnImageRight()
        {
            direction--;
            if (direction < 0) { direction = 3; }
            ReplaceImage();
            // Debug.Write($" Turned Right");
        }
        internal virtual void TurnImageLeft()
        {
            direction++;
            if (direction > 3) { direction = 0; }
            ReplaceImage();
            // Debug.Write($" Turned Left");
        }
        internal virtual void ReplaceImage()
        {
            pawnCanvas.Children.Remove(pawnImage);
            pawnImage = pawnImages[direction];
            pawnCanvas.Children.Add(pawnImage);
        }
    }
    internal class Cow : Pawn
    {
        public Cow(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse, in Team myTeam)
            : base(gbg, startpos, ref teamcoarse, in myTeam)
        {
            localIndex = globalIndex % 4;

            Name = "Cow";
            pawnCanvas.Name = "Cow";
            direction = 3;
            pawnImages = new Image[4]
            {
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/cow_0.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/cow_1.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/cow_2.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/cow_3.png")), Height = 80, Width = 80 }
            };
            pawnImage = pawnImages[direction];
            pawnCanvas.Children.Add(pawnImage);
            PositionAtNest();
            Debug.Write($"{Name} {localIndex} has ben created. For Team {Team.NUMBER_OF_TEAMS}\n");

        }
    }
    internal class Hen : Pawn
    {
        public Hen(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse, in Team myTeam)
            : base(gbg, startpos, ref teamcoarse, in myTeam)
        {
            localIndex = globalIndex % 4;

            Name = "Hen";
            pawnCanvas.Name = "Hen";
            direction = 0;
            pawnImages = new Image[4]
            {
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/hen_0.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/hen_1.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/hen_2.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/hen_3.png")), Height = 80, Width = 80 }
            };
            pawnImage = pawnImages[direction];
            pawnCanvas.Children.Add(pawnImage);
            PositionAtNest();
            Debug.Write($"{Name} {localIndex} has ben created. For Team {Team.NUMBER_OF_TEAMS}\n");
        }
    }
    internal class Sheep : Pawn
    {
        public Sheep(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse, in Team myTeam)
            : base(gbg, startpos, ref teamcoarse, in myTeam)
        {
            localIndex = globalIndex % 4;

            Name = "Sheep";
            pawnCanvas.Name = "Sheep";
            direction = 1;
            pawnImages = new Image[4]
            {
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/sheep_0.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/sheep_1.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/sheep_2.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/sheep_3.png")), Height = 80, Width = 80 }
            };
            pawnImage = pawnImages[direction];
            pawnCanvas.Children.Add(pawnImage);
            PositionAtNest();
            Debug.Write($"{Name} {localIndex} has ben created. For Team {Team.NUMBER_OF_TEAMS}\n");
        }
    }
    internal class Pig : Pawn
    {
        public Pig(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse, in Team myTeam)
            : base(gbg, startpos, ref teamcoarse, in myTeam)
        {
            localIndex = globalIndex % 4;

            Name = "Pig";
            pawnCanvas.Name = "Pig";
            direction = 2;
            pawnImages = new Image[4]
            {
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/pig_0.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/pig_1.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/pig_2.png")), Height = 80, Width = 80 },
                new Image { Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/pig_3.png")), Height = 80, Width = 80 }
            };
            pawnImage = pawnImages[direction];
            pawnCanvas.Children.Add(pawnImage);
            PositionAtNest();
            Debug.Write($"{Name} {localIndex} has ben created. For Team {Team.NUMBER_OF_TEAMS}\n");
        }
    }
}
