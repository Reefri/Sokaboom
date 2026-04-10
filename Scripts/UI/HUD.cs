using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HUD : Control
	{
		[Export] private Button buttonUndo;
		[Export] private Button buttonRedo;
        [Export] private Button reStart;
        [Export] private Label par;

        [Export] public Label steps;
        [Export] private Label name;
        [Export] private Label author;

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
            name.Text = Tr(GameManager.GetInstance().currentLevel.Title);
            author.Text = Tr(BY) + GameManager.GetInstance().currentLevel.Author;
            steps.Text = Tr(STEPS) + 0;

            reStart.Pressed += Retry;
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
