using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HelpMenu : Node
	{
		static private HelpMenu instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/HelpMenu.tscn");

		public bool comeToMenu = true;

		private HelpMenu():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(HelpMenu) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public HelpMenu GetInstance()
		{
			if (instance == null) instance = (HelpMenu)factory.Instantiate();
			return instance;
		}

		public override void _Ready()
		{
			base._Ready();

			GD.Print(comeToMenu);
		}

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
