using System;
using System.Diagnostics;
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

        Team cows, team2;
        // Pawn pawn = new Pawn();



        public GamePage()
        {
            InitializeComponent();
            Window.Current.CoreWindow.PointerMoved += CoreWindow_PointerMoved;
            layoutRoot.PointerWheelChanged += new PointerEventHandler(PointerWheelChanged);
            // layoutRoot.PointerReleased += new PointerEventHandler(PointerReleased);
            Loaded += MainPage_Loaded;
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

            cows = new Team(gameGrid, globalCoarse, new Position(9, 9));
            team2 = new Team(gameGrid, globalCoarse, new Position(1, 1));

            layoutRoot.Children.Add(cows.Pawn.PawnCanvas);
            layoutRoot.Children.Add(team2.Pawn.PawnCanvas);


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
            debugtext.Text = $"Mouse Position: X={roundedX}, Y={roundedY}, {gameGrid.Squish}";
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
    }
}
