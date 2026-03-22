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
			QueueFree();

            UIManager.GetInstance().uiHUD = (HUD)GD.Load<PackedScene>("res://Scenes/HUD.tscn").Instantiate();
        }

        protected override void Dispose(bool pDisposing)
		{

		}
	}
}
