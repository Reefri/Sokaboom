using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HUD : Control
	{
		[Export] private Button buttonUndo;
		[Export] private Button buttonRedo;
        [Export] private Label par;
        [Export] public Label steps;
        [Export] private Label name;
        [Export] public Label number;


        public override void _Ready()
		{
            UIManager.GetInstance().instanceHud = this;

            buttonUndo.Pressed += () => GameManager.GetInstance().MoveBackInTime();
            buttonRedo.Pressed += () => GameManager.GetInstance().MoveForwardInTime();

            par.Text = "Par : " + GameManager.GetInstance().currentLevel.Par;
            name.Text = GameManager.GetInstance().currentLevel.Title + " by : " + GameManager.GetInstance().currentLevel.Author;
            
        }

        private void QuitPressed()
		{
            UIManager.GetInstance().GoToLevelSelect();
            GameManager.GetInstance().QueueFree();
        }

        private void RetryPressed()
        {
            Player.GetInstance().canInput = true;
            GameManager.GetInstance().currentPosition = new HistoricHeap(GameManager.GetInstance().currentLevel);
            GameManager.GetInstance().ChargeMapFromCurrentLevel();
            GameManager.GetInstance().CurrentPar = 0;
        }
	}
}
