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
        // Pawn[] pawns = new Pawn[4];

        Pawn pawn;
        internal Pawn Pawn { get => pawn; set => pawn = value; }

        public Team(GameBoardGrid gbg, Position[] coarse, Position start)
        {
            this.coarse = coarse;
            nestIndex = start;
            boardgrid = gbg;

            pawn = new Pawn(boardgrid, nestIndex, ref this.coarse);
        }
    }
}
