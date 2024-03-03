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
        static Position nestIndex;
        public Position[] coarse;
        GameBoardGrid boardgrid;
        Pawn pawn;

        internal Pawn Pawn { get => pawn; set => pawn = value; }

        public Team(GameBoardGrid gbg, Position[] coarse, Position start, Position goal)
        {
            // Combine the original coarse array with the goal position
            this.coarse = new Position[coarse.Length + 1];
            Array.Copy(coarse, this.coarse, coarse.Length);
            this.coarse[coarse.Length] = goal;

            nestIndex = start;
            boardgrid = gbg;

            pawn = new Pawn(boardgrid, nestIndex, ref this.coarse);
        }
    }
}
