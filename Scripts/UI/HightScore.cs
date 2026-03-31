using Godot;
using System.Collections.Generic;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HightScore : Control
	{
		[Export] Button next;
		public override void _Ready()
		{
			next.Pressed += () => UIManager.GetInstance().GoToTitle();

			foreach (Account lAccount in AccountManager.GetInstance().GetTopPlayers(10))
            {
				GD.Print(lAccount.Id + " " + lAccount.FinalScore());
			}
		}
	}
}
