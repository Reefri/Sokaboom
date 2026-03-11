using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HelpMenu : Control
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
		}

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;
		}

		private void ReturnPressed()
		{
			if (comeToMenu) UIManager.GetInstance().GoToTitle();
            else GD.Print("Va au Niveau Tuto"); //truc pour aller au niveau tuto
        }

        protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
