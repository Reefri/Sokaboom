using Godot;
using System;
using System.Reflection;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HelpMenu : Control
	{
		private void ReturnPressed()
		{
			if (UIManager.GetInstance().comeToMenu) UIManager.GetInstance().GoToTitle();
            else UIManager.GetInstance().GoToLevel(0);
        }

	}
}
