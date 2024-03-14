using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIA_Grupp2
{
    /// <summary>
    /// Represents the options for a game session, including game time and turn time.
    /// </summary>
    internal class GameSessionOptions
	{
		public int GameTimeHours, GameTimeMinutes, GameTimeSeconds;
		public int TurnTimeHours, TurnTimeMinutes, TurnTimeSeconds;
	}
}
