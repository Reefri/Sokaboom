using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class UIManager : Control
	{
		static private UIManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/UIManager.tscn");

        public const string LOGIN_PATH = "res://Scenes/Login.tscn";
        public const string TITLE_SCREEN_PATH = "res://Scenes/TitleCard.tscn";
        public const string HELP_PATH = "res://Scenes/HelpMenu.tscn";
        public const string LEVEL_SELECT_PATH = "res://Scenes/LevelSelect.tscn";

        public Node uiLogin = GD.Load<PackedScene>(LOGIN_PATH).Instantiate();
        public Node uiTitle = GD.Load<PackedScene>(TITLE_SCREEN_PATH).Instantiate();
        public Node uiHelp = GD.Load<PackedScene>(HELP_PATH).Instantiate();
        public Node uiLevelSelect = GD.Load<PackedScene>(LEVEL_SELECT_PATH).Instantiate();

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

			AddChild(uiLogin);
        }

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;

        }
        public void GoToTitle()
        {
			RemoveChild(GetChild(0));
            AddChild(uiTitle);
        }

        public void GoToHelp()
        {
            RemoveChild(GetChild(0));
            AddChild(uiHelp);
        }

        public void GoToLevelSelect()
        {
            RemoveChild(GetChild(0));
            AddChild(uiLevelSelect);
        }

        protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
