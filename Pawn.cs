using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace FIA_Grupp2
{
    internal class Pawn
    {
        // GameBoardGrid board;
        Position indexPosition;
        GameBoardGrid boardgrid;
        int steps = 0;
        public Position[] coarse;

        Canvas pawnCanvas = new Canvas
        {
            Name = "pawn_1_canvas",
            Height = 80,
            Width = 80
        };

        Image pawnImage = new Image
        {
            Name = "team_1_pawn_1",
            Source = new BitmapImage(new Uri("ms-appx:///Assets/Pawns/cow_0.png")),
            Height = 80,
            Width = 80,
        };

        public Canvas PawnCanvas { get => pawnCanvas; set => pawnCanvas = value; }
        public Image PawnImage { get => pawnImage; set => pawnImage = value; }

        public Pawn(GameBoardGrid gbg, Position startpos,ref Position[] teamcoarse)
        {
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
            PositionAt(coarse[steps++]);
            Debug.Write($"OneStep: {steps} \n");
        }
    }
}
