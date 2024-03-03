using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FIA_Grupp2
{
    internal class Pawn
    {
        int direction = 3;
        // GameBoardGrid board;
        Position indexPosition;
        GameBoardGrid boardgrid;
        int steps = 0;
        public Position[] coarse;

        Canvas pawnCanvas = new Canvas
        {
            // Name = "pawn_1_canvas",
            Height = 80,
            Width = 80
        };

        Image pawnImage = new Image
        {
            // Name = "team_1_pawn_1",
            Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/cow_0.png")),
            Height = 80,
            Width = 80
        };

        Image[] pawnImages = new Image[4]
        {
            new Image{ Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/cow_0.png")), Height = 80, Width = 80 },
            new Image{ Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/cow_1.png")), Height = 80, Width = 80 },
            new Image{ Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/cow_2.png")), Height = 80, Width = 80 },
            new Image{ Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/cow_3.png")), Height = 80, Width = 80 }
        };

        public Canvas PawnCanvas { get => pawnCanvas; set => pawnCanvas = value; }
        public Image PawnImage { get => pawnImage; set => pawnImage = value; }

        public Pawn(GameBoardGrid gbg, Position startpos, ref Position[] teamcoarse)
        {

            pawnImage = pawnImages[direction];
            pawnCanvas.Children.Add(pawnImage);
            indexPosition = startpos;
            boardgrid = gbg;
            PositionAtNest();
            coarse = teamcoarse;
        }

        void PositionAtNest()
        {
            Point pos = boardgrid.GetActualPositionOf(indexPosition.X, indexPosition.Y);
            // Debug.WriteLine(pos.ToString());
            double newX = pos.X;
            double newY = pos.Y;
            Canvas.SetLeft(PawnImage, newX + 8);
            Canvas.SetTop(PawnImage, newY - 10);
        }

        void PositionAt(Position ind)
        {
            Point pos = boardgrid.GetActualPositionOf(ind.X, ind.Y);
            // Debug.WriteLine(pos.ToString());
            double newX = pos.X;
            double newY = pos.Y;
            Canvas.SetLeft(PawnImage, newX + 8);
            Canvas.SetTop(PawnImage, newY - 10);
        }

        internal void Step()
        {
            Debug.Write($"\n");
            Debug.Write($"Direction: {direction} ");
            Debug.Write($" From: {coarse[steps]} ");
            Debug.Write($" To: {coarse[steps + 1]} ");
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

        private bool IsSameDirection()
        {
            if (direction == 3 || direction == 1)
            {
                Debug.Write($" Y ");
                if (coarse[steps].Y == coarse[steps + 1].Y) { return true; }
                else { return false; }
            }
            else if (direction == 2 || direction == 0)
            {
                Debug.Write($" X ");
                if (coarse[steps].X == coarse[steps + 1].X) { return true; }
                else { return false; }
            }
            return false;
        }

        private void TurnImageRight()
        {
            direction--;
            if (direction < 0) { direction = 3; }
            ReplaceImage();
            Debug.Write($" Turned Right");
        }

        private void TurnImageLeft()
        {
            direction++;
            if (direction > 3) { direction = 0; }
            ReplaceImage();
            Debug.Write($" Turned Left");
        }

        private void ReplaceImage()
        {
            pawnCanvas.Children.Remove(pawnImage);
            pawnImage = pawnImages[direction];
            pawnCanvas.Children.Add(pawnImage);
        }
    }
}
