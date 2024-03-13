﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FIA_Grupp2
{
    /// <summary>
    /// Represents a team in the game.
    /// </summary>
    internal class Team
    {
        public static int NUMBER_OF_TEAMS = 0;
        string name = "Team";
        private bool isAI;
        public string Name { get => name; set => name = value; }

        protected static Position nestIndex;
        public Position[] coarse;
        protected GameBoardGrid boardgrid;

        internal Pawn pawn;
        internal Pawn Pawn { get => pawn; set => pawn = value; }

        internal Pawn[] pawns;
        internal Pawn[] Pawns { get => pawns; set => pawns = value; }
        public bool IsAI { get => isAI; set => isAI = value; }
        /// <summary>
        /// Initializes a new instance of the Team class.
        /// </summary>
        /// <param name="gbg">The game board grid.</param>
        /// <param name="coarse">The possible positions on the game board path.</param>
        /// <param name="start">The start position.</param>
        /// <param name="goal">The goal position.</param>
        /// <param name="artificial">Indicates whether the team is controlled by AI.</param>
        public Team(GameBoardGrid gbg, Position[] coarse, Position start, Position goal, bool artificial = false)
        {
            NUMBER_OF_TEAMS++;
            nestIndex = start;
            boardgrid = gbg;
            IsAI = artificial;
        }


        /// <summary>
        /// Gets the pawns in the team.
        /// </summary>
        /// <returns>An array of pawns.</returns>
        public virtual Pawn[] GetPawns()
        {
            return pawns;
        }

        /// <summary>
        /// Gets the pawns currently on the game board.
        /// </summary>
        /// <returns>An array of pawns on the board.</returns>
        public virtual Pawn[] GetPawnsOnTheBoard()
        {
            Pawn[] allPawns = GetPawns();
            List<Pawn> pawnsOnTheField = new List<Pawn>();

            for (int i = 0; i < allPawns.Length; i++)
            {
                if (allPawns[i].AtNest == false)
                {
                    pawnsOnTheField.Add(allPawns[i]);
                }
            }

            return pawnsOnTheField.ToArray();
        }

        /// <summary>
        /// Gets the pawns currently in the nest.
        /// </summary>
        /// <returns>An array of pawns in the nest.</returns>
        public virtual Pawn[] GetPawnsInTheNest()
        {
            Pawn[] allPawns = GetPawns();
            List<Pawn> pawnsInTheNest = new List<Pawn>();

            for (int i = 0; i < allPawns.Length; i++)
            {
                if (allPawns[i].AtNest == true)
                {
                    pawnsInTheNest.Add(allPawns[i]);
                }
            }

            return pawnsInTheNest.ToArray();
        }


    }
    internal class Cows : Team
    {
        /// <summary>
        /// Initializes a new instance of the Cows class.
        /// </summary>
        /// <param name="gbg">The game board grid.</param>
        /// <param name="coarse">The positions on the game board.</param>
        /// <param name="start">The start position.</param>
        /// <param name="goal">The goal position.</param>
        /// <param name="artificial">Indicates whether the team is controlled by AI.</param>
        public Cows(GameBoardGrid gbg, Position[] coarse, Position start, Position goal, bool artificial = false)
        : base(gbg, coarse, start, goal, artificial)
        {
            Name = "Cows";
            IsAI = artificial;
            Position[] firstpart = new Position[40];
            Position[] lastpart = new Position[4]
            {
                new Position(5, 9),new Position(5, 8),new Position(5, 7),new Position(5, 6)
            };
            // Split the original coarse array into two equal parts
            Array.Copy(coarse, 0, firstpart, 0, 40); // Copy the first 40 elements to firstpart

            Position[] combinedCoarse = new Position[coarse.Length + 1];
            Array.Copy(firstpart, 0, combinedCoarse, 0, firstpart.Length);

            // Combine the original coarse array with the goal position
            this.coarse = new Position[coarse.Length + 1];

            Array.Copy(coarse, this.coarse, coarse.Length);

            this.coarse[coarse.Length] = goal;

            pawns = new Pawn[4]{
                new Cow(boardgrid, nestIndex, ref this.coarse, this),
                new Cow(boardgrid, nestIndex, ref this.coarse, this),
                new Cow(boardgrid, nestIndex, ref this.coarse, this),
                new Cow(boardgrid, nestIndex, ref this.coarse, this)
            };

        }

    }
    internal class Hens : Team
    {
        /// <summary>
        /// Initializes a new instance of the Hens class.
        /// </summary>
        /// <param name="gbg">The game board grid.</param>
        /// <param name="coarse">The positions on the game board.</param>
        /// <param name="start">The start position.</param>
        /// <param name="goal">The goal position.</param>
        /// <param name="artificial">Indicates whether the team is controlled by AI.</param>
        public Hens(GameBoardGrid gbg, Position[] coarse, Position start, Position goal , bool artificial = false)
        : base(gbg, coarse, start, goal, artificial)
        {
            Name = "Hens";
            IsAI = artificial;
            Position[] firstpart = new Position[10];
            Position[] secondpart = new Position[30];
            Position[] lastpart = new Position[4]
            {
                new Position(9, 5),new Position(8, 5),new Position(7, 5),new Position(6, 5)
            };

            // Split the original coarse array into two equal parts
            Array.Copy(coarse, 0, firstpart, 0, 10); // Copy the first 20 elements to firstpart
            Array.Copy(coarse, 10, secondpart, 0, 30); // Copy the next 20 elements to secondpart

            Position[] combinedCoarse = new Position[coarse.Length + 1];

            // Copy the second part (secondpart) to the beginning of the combined array
            Array.Copy(secondpart, combinedCoarse, secondpart.Length);
            Array.Copy(firstpart, 0, combinedCoarse, secondpart.Length, firstpart.Length);
            Array.Copy(lastpart, 0, combinedCoarse, secondpart.Length + firstpart.Length, lastpart.Length);

            // Add the goal position to the end of the combined array
            combinedCoarse[coarse.Length] = goal;

            // Assign the combined array back to this.coarse
            this.coarse = combinedCoarse;

            pawns = new Pawn[4]{
                new Hen(boardgrid, nestIndex, ref this.coarse, this),
                new Hen(boardgrid, nestIndex, ref this.coarse, this),
                new Hen(boardgrid, nestIndex, ref this.coarse, this),
                new Hen(boardgrid, nestIndex, ref this.coarse, this)
            };

        }

    }

    internal class Sheeps : Team
    {
        /// <summary>
        /// Initializes a new instance of the Sheeps class.
        /// </summary>
        /// <param name="gbg">The game board grid.</param>
        /// <param name="coarse">The positions on the game board.</param>
        /// <param name="start">The start position.</param>
        /// <param name="goal">The goal position.</param>
        /// <param name="artificial">Indicates whether the team is controlled by AI.</param>
        public Sheeps(GameBoardGrid gbg, Position[] coarse, Position start, Position goal, bool artificial = false)
        : base(gbg, coarse, start, goal, artificial)
        {
            Name = "Sheeps";
            IsAI = artificial;
            Position[] firstpart = new Position[20];
            Position[] secondpart = new Position[20];
            Position[] lpart = new Position[4];

            Position[] lastpart = new Position[4]
            {
                new Position(5, 1),new Position(5, 2),new Position(5, 3),new Position(5, 4)
            };

            // Split the original coarse array into two equal parts
            Array.Copy(coarse, 0, firstpart, 0, 20); // Copy the first 20 elements to firstpart
            Array.Copy(coarse, 20, secondpart, 0, 20); // Copy the next 20 elements to secondpart

            Position[] combinedCoarse = new Position[coarse.Length + 1];

            // Copy the second part (secondpart) to the beginning of the combined array
            Array.Copy(secondpart, combinedCoarse, secondpart.Length);
            Array.Copy(firstpart, 0, combinedCoarse, secondpart.Length, firstpart.Length);
            Array.Copy(lastpart, 0, combinedCoarse, secondpart.Length + firstpart.Length, lastpart.Length);

            // Add the goal position to the end of the combined array
            combinedCoarse[coarse.Length] = goal;

            // Assign the combined array back to this.coarse
            this.coarse = combinedCoarse;

            // pawn = new Sheep(boardgrid, nestIndex, ref this.coarse);
            pawns = new Pawn[4]{
                new Sheep(boardgrid, nestIndex, ref this.coarse , this),
                new Sheep(boardgrid, nestIndex, ref this.coarse , this),
                new Sheep(boardgrid, nestIndex, ref this.coarse , this),
                new Sheep(boardgrid, nestIndex, ref this.coarse , this)
            };
        }

    }
    internal class Pigs : Team
    {
        /// <summary>
        /// Initializes a new instance of the Pigs class.
        /// </summary>
        /// <param name="gbg">The game board grid.</param>
        /// <param name="coarse">The positions on the game board.</param>
        /// <param name="start">The start position.</param>
        /// <param name="goal">The goal position.</param>
        /// <param name="artificial">Indicates whether the team is controlled by AI.</param>
        public Pigs(GameBoardGrid gbg, Position[] coarse, Position start, Position goal, bool artificial = false)
        : base(gbg, coarse, start, goal, artificial)
        {
            Name = "Pigs";
            IsAI = artificial;
            Position[] firstpart = new Position[30];
            Position[] secondpart = new Position[10];
            Position[] lpart = new Position[4];

            Position[] lastpart = new Position[4]
            {
                new Position(1, 5),new Position(2, 5),new Position(3, 5),new Position(4, 5)
            };

            // Split the original coarse array into two equal parts
            Array.Copy(coarse, 0, firstpart, 0, 30); // Copy the first 20 elements to firstpart
            Array.Copy(coarse, 30, secondpart, 0, 10); // Copy the next 20 elements to secondpart

            Position[] combinedCoarse = new Position[coarse.Length + 1];

            // Copy the second part (secondpart) to the beginning of the combined array
            Array.Copy(secondpart, combinedCoarse, secondpart.Length);
            Array.Copy(firstpart, 0, combinedCoarse, secondpart.Length, firstpart.Length);
            Array.Copy(lastpart, 0, combinedCoarse, secondpart.Length + firstpart.Length, lastpart.Length);

            // Add the goal position to the end of the combined array
            combinedCoarse[coarse.Length] = goal;

            // Assign the combined array back to this.coarse
            this.coarse = combinedCoarse;

            // pawn = new Pig(boardgrid, nestIndex, ref this.coarse);
            pawns = new Pawn[4]{
                new Pig(boardgrid, nestIndex, ref this.coarse, this),
                new Pig(boardgrid, nestIndex, ref this.coarse, this),
                new Pig(boardgrid, nestIndex, ref this.coarse, this),
                new Pig(boardgrid, nestIndex, ref this.coarse, this)
            };
        }

    }
}
