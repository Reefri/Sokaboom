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

        private IsartLogo uiScreenSplash = (IsartLogo)GD.Load<PackedScene>("res://Scenes/UI/IsartLogo.tscn").Instantiate();
        private LoginUI uiLogin = (LoginUI)GD.Load<PackedScene>("res://Scenes/UI/Login.tscn").Instantiate();
        private TitleCard uiTitle = (TitleCard)GD.Load<PackedScene>("res://Scenes/UI/TitleCard.tscn").Instantiate();
        private HelpMenu uiHelp = (HelpMenu)GD.Load<PackedScene>("res://Scenes/UI/HelpMenu.tscn").Instantiate();
        private LevelSelect uiLevelSelect = (LevelSelect)GD.Load<PackedScene>("res://Scenes/UI/LevelSelect.tscn").Instantiate();
        public HUD uiHUD = (HUD)GD.Load<PackedScene>("res://Scenes/UI/HUD.tscn").Instantiate();
        private Win uiWin = (Win)GD.Load<PackedScene>("res://Scenes/UI/Win.tscn").Instantiate();

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

			AddChild(uiScreenSplash);
        	//A mettre dans le splash I guess ?
        	//if (!noLogin) AddChild(uiLogin);
			//else
			//{
			//	AccountManager.GetInstance().TestConnexion("Guest", "");
			//	AddChild(uiTitle);
			//}
        }

		public void UpdateHud()
		{
			uiHUD.steps.Text = "Steps : " + GameManager.GetInstance().CurrentPar;
        }

        public void GoToLogin()
        {
            RemoveChild(GetChild(0));
            AddChild(uiLogin);
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
			if (pIndex > uiLevelSelect.numberOfLevel && !(Main.GetInstance().testOnlyGameFeature)) { GoToLevelSelect(); return; }

            RemoveChild(GetChild(0));

			levelIndex = pIndex;
			Main.GetInstance().AddChild(GameManager.GetInstance());
            AddChild(uiHUD);

			uiHUD.number.Text = "level ";
            if (pIndex == 0) uiHUD.number.Text += "tuto";
			else uiHUD.number.Text += pIndex;

            CameraManager.GetInstance().CenterCameraOnCurrentLevel();
        }

		public void GoToWin()
		{
            RemoveChild(GetChild(0));
			GameManager.GetInstance().QueueFree();
            
            AddChild(uiWin);
            foreach (AnimatedSprite2D lStars in uiWin.stars.GetChildren()) lStars.Frame = 0;
            uiWin.CalculScoreLevel();
        }

        protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
