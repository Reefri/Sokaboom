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
        private PackedScene LevelOpenTransition = GD.Load<PackedScene>("res://Scenes/UI/Transitions/SlideTransition.tscn");
        private PackedScene uiMenuChangeTransition = GD.Load<PackedScene>("res://Scenes/UI/Transitions/MenuTransition.tscn");

        public HUD instanceHud;

        [Export] private bool noLogin = true;
        public int levelIndex;
        private int currentIndex = -1;
        public bool comeToMenu = true;
        private bool menuChanging = false;

		public int finalScore;

        private Timer delay = new Timer();

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

            GetParent().CallDeferred("add_child", delay);
            delay.OneShot = true;
            delay.Timeout += Continue;
        }

        private void Continue()
        {
            if (currentIndex != -1)
            {
                GoToLevel(currentIndex);
                currentIndex = -1;
            }

            if (!menuChanging)
            {
                
                menuChanging = true;
                GoToLevelSelect();
            }
        }

        public void UpdateHud()
        {
            if (instanceHud != null) instanceHud.steps.Text = Tr("ID_STEPS") + GameManager.GetInstance().CurrentPar;
        }

        public void GoToLogin()
        {
            GetChild(0).QueueFree();
            AddChild(uiLogin.Instantiate());
        }

        public void GoToTitle()
        {
            GetChild(0).QueueFree();
            AddChild(uiTitle.Instantiate());
        }

        public void GoToHelp()
        {
            GetChild(0).QueueFree();
            AddChild(uiHelp.Instantiate());
        }

        public void GoToLevelSelect()
        {
            if (!menuChanging)
            {
                MenuTransition lTransition = (MenuTransition)uiMenuChangeTransition.Instantiate();
                GetParent().AddChild(lTransition);
                delay.WaitTime = lTransition.tweenDuration/1.7f;
                delay.Start();
            }

            if (delay.IsStopped())
            {
                GetChild(0).QueueFree();
                AddChild(uiLevelSelect.Instantiate());
                menuChanging = false;
            }
            
        }

        public void GoToLevel(int pIndex)
        {

            if (pIndex > GridManager.GetInstance().numberOfLevel && !(Main.GetInstance().testOnlyGameFeature)) { GoToLevelSelect(); return; }

            if (currentIndex == -1)
            {
                currentIndex = pIndex;
                SlideTransition lLevelOpenTransition = (SlideTransition)LevelOpenTransition.Instantiate();
                AddChild(lLevelOpenTransition);
                delay.WaitTime = lLevelOpenTransition.canChangeLevel;
                delay.Start();
            }

            if(delay.IsStopped())
            {
                GetChild(0).QueueFree();

                levelIndex = pIndex;
                Main.GetInstance().AddChild(GameManager.GetInstance());

                AddChild(uiHUD.Instantiate());

                instanceHud.number.Text = Tr("ID_LEVEL");
                if (pIndex == 0) instanceHud.number.Text += "tuto";
                else instanceHud.number.Text += pIndex;

                CameraManager.GetInstance().CenterCameraOnCurrentLevel();
            }
            
        }

		public void GoToWin()
		{
            GetChild(0).QueueFree();
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
