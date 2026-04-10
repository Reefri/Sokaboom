using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HUD : Control
	{
		[Export] private Button buttonUndo;
        [Export] private Vector2 undoLandscapePos;
        [Export] private Vector2 undoPortraitPos;
		[Export] private Button buttonRedo;
        [Export] private Vector2 redoLandscapePos;
        [Export] private Vector2 redoPortraitPos;
        [Export] private Button restart;
        [Export] private Vector2 restartLandscapePos;
        [Export] private Vector2 restartPortraitPos;
        [Export] private Label par;
        [Export] public Label steps;
        [Export] private Label name;
        [Export] public Label number;

        [Export] private Timer quitDelay = new Timer();

        private const string PAR = "Par : ";
        private const string BY = "ID_BY";
        private const string STEPS = "ID_STEPS";

        public override void _Ready()
		{
            quitDelay.Timeout += DestroyGameManager;

            UIManager.GetInstance().instanceHud = this;

            buttonUndo.Pressed += GameManager.GetInstance().MoveBackInTime;
            buttonRedo.Pressed += GameManager.GetInstance().MoveForwardInTime;

            par.Text = PAR + GameManager.GetInstance().currentLevel.Par;
            name.Text = Tr(GameManager.GetInstance().currentLevel.Title) + Tr(BY) + GameManager.GetInstance().currentLevel.Author;
            steps.Text = Tr(STEPS) + 0;

            restart.Pressed += Retry;
        }

        private void PortraitMode()
        {

        }

        private void Retry()
        {
            MenuTransition.Create(RetryPressed);
        }

        private void DestroyGameManager()
        {
            GameManager.GetInstance().QueueFree();
        }

        private void QuitPressed()
		{
            UIManager.GetInstance().GoToLevelSelect();
            quitDelay.Start();
            
        }
        private void RetryPressed()
        {
            GameManager.GetInstance().QuickResetInit();

            GameManager.GetInstance().currentPosition = new HistoricHeap(GameManager.GetInstance().currentLevel);
            GameManager.GetInstance().ChargeMapFromCurrentLevel();
            GameManager.GetInstance().CurrentPar = 0;
        }

        public void DisabledRedo(bool pBool)
        {
            buttonRedo.Disabled = !pBool;
        }

        protected override void Dispose(bool pDisposing)
        {
            UIManager.GetInstance().instanceHud = null;
        }
    }
}
