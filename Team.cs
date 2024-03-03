using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIA_Grupp2
{
    internal class Team
    {
        protected static Position nestIndex;
        public Position[] coarse;
        protected GameBoardGrid boardgrid;
        internal Pawn pawn;

        internal Pawn Pawn { get => pawn; set => pawn = value; }

        public Team(GameBoardGrid gbg, Position[] coarse, Position start, Position goal)
        {
            nestIndex = start;
            boardgrid = gbg;

        }
    }
    internal class Cows : Team
    {
        public Cows(GameBoardGrid gbg, Position[] coarse, Position start, Position goal)
        : base(gbg, coarse, start, goal)
        {
            // Combine the original coarse array with the goal position
            this.coarse = new Position[coarse.Length + 1];
            Array.Copy(coarse, this.coarse, coarse.Length);
            this.coarse[coarse.Length] = goal;

            pawn = new Cow(boardgrid, nestIndex, ref this.coarse);
        }
    }
    internal class Sheeps : Team
    {
        public Sheeps(GameBoardGrid gbg, Position[] coarse, Position start, Position goal)
        : base(gbg, coarse, start, goal)
        {
            Position[] firstpart = new Position[20];
            Position[] secondpart = new Position[20];
            Position[] lpart = new Position[4];

            Position[] lastpart = new Position[4]
            {
                new Position(1, 5),new Position(2, 5),new Position(3, 5),new Position(4, 5)
            };

            // Split the original coarse array into two equal parts
            Array.Copy(coarse, 0, firstpart, 0, 20); // Copy the first 22 elements to firstpart
            Array.Copy(coarse, 20, secondpart, 0, 20); // Copy the next 22 elements to secondpart

            Position[] combinedCoarse = new Position[coarse.Length + 1];

            // Copy the second part (secondpart) to the beginning of the combined array
            Array.Copy(secondpart, combinedCoarse, secondpart.Length);
            Array.Copy(firstpart, 0, combinedCoarse, secondpart.Length, firstpart.Length);
            Array.Copy(lastpart, 0, combinedCoarse, secondpart.Length + firstpart.Length, lastpart.Length);

            // Add the goal position to the end of the combined array
            combinedCoarse[coarse.Length] = goal;

            // Assign the combined array back to this.coarse
            this.coarse = combinedCoarse;

            pawn = new Sheep(boardgrid, nestIndex, ref this.coarse);
        }
    }
}
