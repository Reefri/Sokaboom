using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HUD : Control
	{
		[Export] private Button buttonUndo;
		[Export] private Button buttonRedo;
        [Export] private Label par;
        [Export] public Label steps;

        public override void _Ready()
		{
			buttonUndo.Pressed += GameManager.GetInstance().MoveBackInTime;
            buttonRedo.Pressed += GameManager.GetInstance().MoveForwardInTime;

			ResetHUD();
        }

		public void ResetHUD()
		{
            par.Text = "Par : " + GameManager.GetInstance().currentLevel.Par;

        }


        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
        }

		private void QuitPressed()
		{
            UIManager.GetInstance().GoToLevelSelect();
            GameManager.GetInstance().QueueFree();
        }

        private void RetryPressed()
        {
            GameManager.GetInstance().currentPosition = new HistoricHeap(GameManager.GetInstance().currentLevel);
            GameManager.GetInstance().ChargeMapFromCurrentLevel();
            GameManager.GetInstance().CurrentPar = 0;
        }

        protected override void Dispose(bool pDisposing)
		{
            UIManager.GetInstance().uiHUD = (HUD)GD.Load<PackedScene>("res://Scenes/HUD.tscn").Instantiate();
        }
	}
}
