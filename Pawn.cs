using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FIA_Grupp2
{
    internal class Pawn
    {
        protected bool isInGoal = false;
        protected int direction;
        protected Position indexPosition;
        protected GameBoardGrid boardgrid;
        protected int steps = 0;
        public Position[] coarse;

        protected Canvas pawnCanvas = new Canvas { Height = 80, Width = 80 };
        protected Image pawnImage = new Image();

        internal Image[] pawnImages = new Image[4];
        public Canvas PawnCanvas { get => pawnCanvas; set => pawnCanvas = value; }
        public Image PawnImage { get => pawnImage; set => pawnImage = value; }

        public Pawn(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse)
        {
            pawnCanvas.Children.Add(pawnImage);
            indexPosition = startpos;
            boardgrid = gbg;
            PositionAtNest();
            coarse = teamcoarse;
        }

        public virtual void PositionAtNest()
        {
            Point pos = boardgrid.GetActualPositionOf(indexPosition.X, indexPosition.Y);
            double newX = pos.X;
            double newY = pos.Y;
            Canvas.SetLeft(PawnImage, newX + 8);
            Canvas.SetTop(PawnImage, newY - 10);
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
            if (isInGoal)
            {
                Debug.Write($"Im home already :)");
            }
            else if (steps == coarse.Length - 1)
            {
                PositionAt(coarse[steps++]);
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
                    if (direction == 2 && coarse[steps + 1].X < coarse[steps].X) { TurnImageLeft(); }
                    else if (direction == 2 && coarse[steps + 1].X > coarse[steps].X) { TurnImageRight(); }
                    else if (direction == 3 && coarse[steps + 1].Y < coarse[steps].Y) { TurnImageLeft(); }
                    else if (direction == 3 && coarse[steps + 1].Y > coarse[steps].Y) { TurnImageRight(); }
                    else if (direction == 0 && coarse[steps + 1].X < coarse[steps].X) { TurnImageRight(); }
                    else if (direction == 0 && coarse[steps + 1].X > coarse[steps].X) { TurnImageLeft(); }
                    else if (direction == 1 && coarse[steps + 1].Y < coarse[steps].Y) { TurnImageRight(); }
                    else if (direction == 1 && coarse[steps + 1].Y > coarse[steps].Y) { TurnImageLeft(); }
                }
                PositionAt(coarse[steps++]);
            }
        }
        internal virtual bool IsSameDirection()
        {
            if (direction == 3 || direction == 1)
            {
                // Debug.Write($" Y ");
                if (coarse[steps].Y == coarse[steps + 1].Y) { return true; }
                else { return false; }
            }
            else if (direction == 2 || direction == 0)
            {
                // Debug.Write($" X ");
                if (coarse[steps].X == coarse[steps + 1].X) { return true; }
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
        public Cow(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse)
            : base(gbg, startpos, ref teamcoarse)
        {
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
        }
    }
    internal class Hen : Pawn
    {
        public Hen(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse)
            : base(gbg, startpos, ref teamcoarse)
        {
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
        }
    }
    internal class Sheep : Pawn
    {
        public Sheep(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse)
            : base(gbg, startpos, ref teamcoarse)
        {
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
        }
    }
    internal class Pig : Pawn
    {
        public Pig(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse)
            : base(gbg, startpos, ref teamcoarse)
        {
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
        }
    }
}
