using Com.IsartDigital.Sokoban.UI;
using Com.IsartDigital.UI;
using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class UIManager : Control
	{
		static private UIManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/UIManager.tscn");

        private LoginUI uiLogin = (LoginUI)GD.Load<PackedScene>("res://Scenes/Login.tscn").Instantiate();
        private TitleCard uiTitle = (TitleCard)GD.Load<PackedScene>("res://Scenes/TitleCard.tscn").Instantiate();
        private HelpMenu uiHelp = (HelpMenu)GD.Load<PackedScene>("res://Scenes/HelpMenu.tscn").Instantiate();
        private LevelSelect uiLevelSelect = (LevelSelect)GD.Load<PackedScene>("res://Scenes/LevelSelect.tscn").Instantiate();
        public HUD uiHUD = (HUD)GD.Load<PackedScene>("res://Scenes/HUD.tscn").Instantiate();

        [Export] private bool noLogin = true;
        public int levelIndex;
        public bool comeToMenu = true;

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

			if (!noLogin) AddChild(uiLogin);
			else AddChild(uiTitle);
        }

		public void UpdateHud()
		{
			uiHUD.steps.Text = "Steps : " + GameManager.GetInstance().CurrentPar;
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

		public void GoToLevel(int pIndex)
		{
            RemoveChild(GetChild(0));
			
            levelIndex = pIndex;
			Main.GetInstance().AddChild(GameManager.GetInstance());
            AddChild(uiHUD);

            CameraManager.GetInstance().CenterCameraOnCurrentLevel();
        }

        protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
