using System;
using System.Diagnostics;
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

        public GamePage()
        {
            InitializeComponent();
            Window.Current.CoreWindow.PointerMoved += CoreWindow_PointerMoved;
            layoutRoot.PointerWheelChanged += new PointerEventHandler(PointerWheelChanged);
            Loaded += MainPage_Loaded;
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
    }
}
