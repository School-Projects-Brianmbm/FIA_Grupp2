using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIA_Grupp2
{
	public enum SlotUserType
	{
		Player,
		AI,
		None
	}

	public enum Teams
	{
		Cow,
		Pig,
		Sheep,
		Chicken
	}

	internal class LobbyOptions
	{
		public string slot1Usertype, slot2Usertype, slot3Usertype, slot4Usertype;
		public string slot1Username, slot2Username, slot3Username, slot4Username;	
		public string slot1Team, slot2Team, slot3Team, slot4Team;
	}
}
