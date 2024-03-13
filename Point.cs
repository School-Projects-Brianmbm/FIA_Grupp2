namespace FIA_Grupp2
{
    /// <summary>
    /// Point used by GameBoardGrid
    /// </summary>
    internal struct Point
    {
        public double X;
        public double Y;
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return "Point: " + X + " " + Y;
        }
    }
    /// <summary>
    /// Position used by other classes for calculations on the board.
    /// </summary>
    internal struct Position
    {
        public int X;
        public int Y;
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return "Position: " + Y + " " + X;
        }
    }
}
