using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class UIManager : Control
	{
		static private UIManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/UIManager.tscn");

        public const string TITLE_SCREEN_PATH = "res://Scenes/TitleCard.tscn";
        public const string HELP_PATH = "res://Scenes/HelpMenu.tscn";
        public const string LEVEL_SELECT_PATH = "res://Scenes/LevelSelect.tscn";

        private UIManager():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(UIManager) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public UIManager GetInstance()
		{
			if (instance == null) instance = (UIManager)factory.Instantiate();
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

			GD.Print("cc");
		}

        protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
