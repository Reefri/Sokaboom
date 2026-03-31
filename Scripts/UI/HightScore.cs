using Godot;
using System.Collections.Generic;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HightScore : Control
	{
		public override void _Ready()
		{
			foreach (Account lAccount in AccountManager.GetInstance().GetTopPlayers(10))
            {
				GD.Print(lAccount.Id + " " + lAccount.FinalScore());
			}
		}
	}
}
