using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace FIA_Grupp2
{
    internal class GameBoardGrid
    {
        static readonly float _dotSize = 4;
        static readonly float _brickSize = 40;

        /// <summary>
        /// Numbor of rows and columns defines the dimensions of the grid
        /// </summary>
        /// <note>May be overriden later in constructor when creating new maps</note>
        static readonly int _numberOfColumns = 11;
        static readonly int _numberOfRows = 11;
        private static double _colDist = 30;
        static readonly double _rowDist = 60;
        static readonly double _goalSize = _colDist * 2;
        static readonly double _nestSize = _colDist * 3;
        readonly double _origoX;
        private double _origoY;

        private double squish = _rowDist - _colDist;

        readonly Canvas _canvas;
        readonly Point[][] _pointsArray = new Point[_numberOfRows][];
        readonly Ellipse[][] _dotsArray = new Ellipse[_numberOfRows][];
        readonly Point[,] _actualPositions = new Point[_numberOfRows, _numberOfColumns];
        readonly int[,] _pathArray = new int[11, 11];

        /// <summary>
        /// Generic map
        /// </summary>
        readonly int[,] defaultArray = new int[11, 11]
        {
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 9, 0, 0, 1, 5, 1, 0, 0, 8, 0 },
            { 0, 0, 0, 0, 1, 5, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 5, 1, 0, 0, 0, 0 },
            { 1, 1, 1, 1, 1, 5, 1, 1, 1, 1, 1 },
            { 1, 2, 2, 2, 2,-1, 4, 4, 4, 4, 1 },
            { 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 1, 3, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 3, 1, 0, 0, 0, 0 },
            { 0, 6, 0, 0, 1, 3, 1, 0, 0, 7, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 }
        };

        readonly TextBlock[][] textArray = new TextBlock[_numberOfRows][];

        public double Squish { get => squish; set => squish = value; }

        /// <summary>
        /// Constructor for the generic parques map
        /// </summary>
        /// <param name="canvas">The name of the canvas from the page</param>
        public GameBoardGrid(Canvas canvas)
        {
            this._canvas = canvas;
            _pathArray = defaultArray;
            _origoX = canvas.ActualWidth / 2;
            CreateArrayOfPoints();
            CreateArrayOfDots();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="mapArray"></param>
        public GameBoardGrid(Canvas canvas, int[,] mapArray)
        {
            this._canvas = canvas;
            _pathArray = mapArray;
            _origoX = canvas.ActualWidth / 2;
            CreateArrayOfPoints();
            CreateArrayOfDots();
        }

        /// <summary>
        /// Returns the actual position on canvas for the point at passed index [,]
        /// </summary>
        /// <param name="x">The X Index</param>
        /// <param name="y">The Y Index</param>
        /// <returns>Position on canvas of specified point</returns>
        internal Point GetActualPositionOf(int x, int y)
        {
            return _actualPositions[x, y];
        }

        /// <summary>
        /// Creates an array of positions rellative to origo (0,0)
        /// </summary>
        internal void CreateArrayOfPoints()
        {
            double rowmodi = 0;
            for (int row = 0; row < _numberOfRows; row++)
            {
                _pointsArray[row] = new Point[_numberOfColumns];
                _dotsArray[row] = new Ellipse[_numberOfColumns];
                textArray[row] = new TextBlock[_numberOfColumns];

                if (row > 0) { rowmodi += Squish; }

                for (int col = 0; col < _numberOfColumns; col++)
                {
                    _pointsArray[row][col] = new Point(col * (_colDist + Squish), row * (_rowDist - Squish));
                }
            }
        }

        /// <summary>
        /// Creates an circle element for each position on the map
        /// </summary>
        internal void CreateArrayOfDots()
        {
            for (int row = 0; row < _numberOfRows; row++)
            {
                _dotsArray[row] = new Ellipse[_numberOfColumns];
                textArray[row] = new TextBlock[_numberOfColumns];

                for (int col = 0; col < _numberOfColumns; col++)
                {
                    // Create the underlying grid dots
                    if (_pathArray[row, col] == 0)
                    {
                        _dotsArray[row][col] = new Ellipse { Fill = new SolidColorBrush(Colors.Black), Width = _dotSize, Height = _dotSize };
                    }
                    // Create walkable "bricks"
                    else if (_pathArray[row, col] == 1)
                    {
                        _dotsArray[row][col] = new Ellipse { Fill = new SolidColorBrush(Colors.Gray), Width = _brickSize, Height = _brickSize };
                    }
                    // Create reserved bricks
                    else if (_pathArray[row, col] > 1 && _pathArray[row, col] < 6)
                    {
                        switch (_pathArray[row, col])
                        {
                            case 2:
                                _dotsArray[row][col] = new Ellipse { Fill = new SolidColorBrush(Colors.LightBlue), Width = _brickSize, Height = _brickSize };
                                break;
                            case 3:
                                _dotsArray[row][col] = new Ellipse { Fill = new SolidColorBrush(Colors.PaleVioletRed), Width = _brickSize, Height = _brickSize };
                                break;
                            case 4:
                                _dotsArray[row][col] = new Ellipse { Fill = new SolidColorBrush(Colors.LightGreen), Width = _brickSize, Height = _brickSize };
                                break;
                            case 5:
                                _dotsArray[row][col] = new Ellipse { Fill = new SolidColorBrush(Colors.Yellow), Width = _brickSize, Height = _brickSize };
                                break;
                            default:
                                break;
                        }
                        // brings the nest above the dots
                        Canvas.SetZIndex(_dotsArray[row][col], 1);
                    }
                    // Create indivual nests
                    else if (_pathArray[row, col] > 5)
                    {
                        switch (_pathArray[row, col])
                        {
                            case 6:
                                _dotsArray[row][col] = new Ellipse { Fill = new SolidColorBrush(Colors.PaleVioletRed), Width = _nestSize, Height = _nestSize };
                                break;
                            case 7:
                                _dotsArray[row][col] = new Ellipse { Fill = new SolidColorBrush(Colors.LightGreen), Width = _nestSize, Height = _nestSize };
                                break;
                            case 8:
                                _dotsArray[row][col] = new Ellipse { Fill = new SolidColorBrush(Colors.Yellow), Width = _nestSize, Height = _nestSize };
                                break;
                            case 9:
                                _dotsArray[row][col] = new Ellipse { Fill = new SolidColorBrush(Colors.LightBlue), Width = _nestSize, Height = _nestSize };
                                break;
                            default:
                                break;
                        }
                        // brings the nest above the dots
                        Canvas.SetZIndex(_dotsArray[row][col], -1);
                    }
                    // Create goal
                    else if (_pathArray[row, col] < 0)
                    {
                        _dotsArray[row][col] = new Ellipse
                        {
                            Fill = new SolidColorBrush(Colors.White),
                            Width = _goalSize,
                            Height = _goalSize,
                            Stroke = new SolidColorBrush(Colors.Black),
                            StrokeThickness = 1
                        };
                    }
                    textArray[row][col] = new TextBlock
                    {
                        FontSize = 10,
                    };
                }
            }
        }

        /// <summary>
        /// Calculate and write the actual positions on the canvas
        /// to the actualPositions array.
        /// </summary>
        internal void CalculateActualPositions()
        {
            for (int col = 0; col < _numberOfColumns; col++)
            {
                for (int row = 0; row < _numberOfRows; row++)
                {
                    Point currentPoint = _pointsArray[row][col];

                    // Calculate the actual position for the current point
                    double left = _origoX + currentPoint.X;
                    double top = _origoY - currentPoint.Y;

                    double actualX = left - (row * _rowDist);
                    double actualY = top - (col * _colDist);

                    _actualPositions[col, row] = new Point(actualX, actualY);
                }
            }
        }

        /// <summary>
        /// Sets the positions of all Ellipses and finaly add the to the canvas
        /// </summary>
        /// <param name="showgrid">When true the debug points for the underlying grid will also be added for debuging purposes.</param>
        internal void SetEllipsesPositions(bool showgrid = false)
        {
            for (int col = 0; col < _numberOfColumns; col++)
            {
                for (int row = 0; row < _numberOfRows; row++)
                {
                    Point currentPoint = _pointsArray[row][col];
                    Ellipse currentEllipse = _dotsArray[row][col];
                    TextBlock currentText = textArray[row][col];

                    // Calculate the position for the current ellipse
                    // TBD Borde inte behöva räkna ut detta igen utan kan hämta positionerna från actualPositions ist.
                    double left = _origoX + currentPoint.X;
                    double top = _origoY - currentPoint.Y;

                    double actualX = left - (row * _rowDist);
                    double actualY = top - (col * _colDist);


                    Canvas.SetLeft(currentEllipse, actualX - (currentEllipse.Width / 2));
                    Canvas.SetTop(currentEllipse, actualY - (currentEllipse.Height / 2));

                    // Comment to hide the text at each point
                    // Debug text coordinates
                    currentText.Text = $"X={actualX},\n Y={actualY}";

                    // Uncomment to show the index of each point
                    //// Debug text index
                    //currentText.Text = $"X={row}, Y={col}, [{i++}]";


                    // Set the position for the current text
                    Canvas.SetLeft(currentText, actualX);
                    Canvas.SetTop(currentText, actualY);
                    Canvas.SetZIndex(currentText, +1);

                    // Add the current ellipse to the canvas
                    if (showgrid)
                    {
                        _canvas.Children.Add(currentEllipse);
                        _canvas.Children.Add(currentText);
                    }
                    else if (_pathArray[row, col] > 0 || _pathArray[row, col] == -1)
                    {
                        _canvas.Children.Add(currentEllipse);
                    }
                }
            }
        }

        /// <summary>
        /// Corrector for when map is not a perfect square
        /// </summary>
        internal void CalculateColumnDist()
        {
            _colDist = _rowDist - squish;
        }

        /// <summary>
        /// Calculate where to put the 0,0 (low corner point)
        /// to center the grid in height.
        /// </summary>
        internal void CalculateOrigoY()
        {
            double gbheight = _actualPositions[0, 0].Y - _actualPositions[10, 10].Y;
            _origoY = _canvas.ActualHeight - ((_canvas.ActualHeight - gbheight) / 2);
            Debug.Write("origoY: " + _origoY + " ");
        }
    }

}
