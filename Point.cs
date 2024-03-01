namespace FIA_Grupp2
{
    /// <summary>
    /// Just a point used by GameBoardGrid
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
            return "Position: " + X + " " + Y;
        }
    }
}
