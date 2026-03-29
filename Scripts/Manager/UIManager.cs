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
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Manager/UIManager.tscn");

        private PackedScene uiScreenSplash = GD.Load<PackedScene>("res://Scenes/UI/IsartLogo.tscn");
        private PackedScene uiLogin = GD.Load<PackedScene>("res://Scenes/UI/Login.tscn");
        private PackedScene uiTitle = GD.Load<PackedScene>("res://Scenes/UI/TitleCard.tscn");
        private PackedScene uiHelp = GD.Load<PackedScene>("res://Scenes/UI/HelpMenu.tscn");
        private PackedScene uiLevelSelect = GD.Load<PackedScene>("res://Scenes/UI/LevelSelect.tscn");
        private PackedScene uiHUD = GD.Load<PackedScene>("res://Scenes/UI/HUD.tscn");
        private PackedScene uiWin = GD.Load<PackedScene>("res://Scenes/UI/Win.tscn");

        public HUD instanceHud;

        [Export] private bool noLogin = true;
        public int levelIndex;
        public bool comeToMenu = true;

		public int finalScore;

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

			AddChild(uiScreenSplash.Instantiate());
        	
        }

        public void UpdateHud()
        {
            if (GameManager.GetInstance().CurrentPar != 0) instanceHud.steps.Text = "Steps : " + GameManager.GetInstance().CurrentPar;
        }

        public void GoToLogin()
        {
            RemoveChild(GetChild(0));
            AddChild(uiLogin.Instantiate());
        }

        public void GoToTitle()
        {
			RemoveChild(GetChild(0));
            AddChild(uiTitle.Instantiate());
        }

        public void GoToHelp()
        {
            RemoveChild(GetChild(0));
            AddChild(uiHelp.Instantiate());
        }

        public void GoToLevelSelect()
        {
            RemoveChild(GetChild(0));
            AddChild(uiLevelSelect.Instantiate());
        }

		public void GoToLevel(int pIndex)
		{
			if (pIndex > GridManager.GetInstance().numberOfLevel && !(Main.GetInstance().testOnlyGameFeature)) { GoToLevelSelect(); return; }

            RemoveChild(GetChild(0));

			levelIndex = pIndex;
			Main.GetInstance().AddChild(GameManager.GetInstance());

            AddChild(uiHUD.Instantiate());

            instanceHud.number.Text = "level ";
            if (pIndex == 0) instanceHud.number.Text += "tuto";
			else instanceHud.number.Text += pIndex;

            CameraManager.GetInstance().CenterCameraOnCurrentLevel();
        }

		public void GoToWin()
		{
            RemoveChild(GetChild(0));
			GameManager.GetInstance().QueueFree();

            Win lWin = (Win)uiWin.Instantiate();
            AddChild(lWin);
            foreach (AnimatedSprite2D lStars in lWin.stars.GetChildren()) lStars.Frame = 0;
            lWin.CalculScoreLevel();
        }

        protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
