using Godot;

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
        private PackedScene uiWinFinal = GD.Load<PackedScene>("res://Scenes/UI/WinFinal.tscn");
        private PackedScene uiHightScore = GD.Load<PackedScene>("res://Scenes/UI/HightScore.tscn");

        private PackedScene uiMenuChangeTransition = GD.Load<PackedScene>("res://Scenes/UI/Transitions/MenuTransition.tscn");

        private PackedScene uiSpiral = GD.Load<PackedScene>("res://Scenes/Juiciness/Spiral.tscn");

        private Node2D spiralOnMouse;

        public HUD instanceHud;

        public int levelIndex;
        private int currentIndex = -1;
        public bool comeToMenu = true;

		public int finalScore;
        private const string ID_LEVEL = "ID_LEVEL";
        private const string ID_STEPS = "ID_STEPS";
        private const string TUTO_TEXT = "tuto";
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

            spiralOnMouse = (Node2D)uiSpiral.Instantiate();
            spiralOnMouse.ZIndex = 0;
            CallDeferred("add_sibling", spiralOnMouse);
        }
        public override void _Process(double delta)
        {
            base._Process(delta);

            if (GameManager.GetInstance().currentLevel == null)
            {
                spiralOnMouse.GlobalPosition = GetGlobalMousePosition();
                spiralOnMouse.Visible = true;
            }
                
            else spiralOnMouse.Visible = false;
        }

        public void ChangeLayer()
        {
            CanvasLayer lParent = (CanvasLayer)GetParent();
            lParent.Layer = 1;
        }

        public void UpdateHud()
        {
            if (instanceHud != null) instanceHud.steps.Text = Tr(ID_STEPS) + GameManager.GetInstance().CurrentPar;
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
            TitleDoors.GetInstance().goingToLevel = false;
            TitleDoors.GetInstance().Transition();

        }

        public void ContinueToLevelSelect()
        {
            GetChild(0).QueueFree();
            AddChild(uiLevelSelect.Instantiate());
        }

        public void GoToLevel(int pIndex)
        {
            if (pIndex > GridManager.GetInstance().numberOfLevel && !(Main.GetInstance().testOnlyGameFeature)) { GoToLevelSelect(); return; }

            currentIndex = pIndex;

            TitleDoors.GetInstance().goingToLevel = true;
            TitleDoors.GetInstance().Transition();
        }

        public void ContinueToLevel()
        {
            GetChild(0).QueueFree();

            levelIndex = currentIndex;
            
            Main.GetInstance().AddChild(GameManager.GetInstance());

            AddChild(uiHUD.Instantiate());

            instanceHud.number.Text = Tr(ID_LEVEL);
            if (currentIndex == 0) instanceHud.number.Text += TUTO_TEXT;
            else instanceHud.number.Text += currentIndex;

            CameraManager.GetInstance().CenterCameraOnCurrentLevel();
        }

        public void GoToWin()
		{
            GetChild(0).QueueFree();
            AddChild(uiWin.Instantiate());
        }

        public void GoToWinFinal()
        {
            GetChild(0).QueueFree();
            AddChild(uiWinFinal.Instantiate());
        }

        public void GoToHightScore()
        {
            GetChild(0).QueueFree();
            AddChild(uiHightScore.Instantiate());
        }

        protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
