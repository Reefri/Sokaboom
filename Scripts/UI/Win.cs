using Godot;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban
{
	public partial class Win : Control
	{
		[Export] public Control stars;

        [Export] private Label scoreText;
		private int score;
		private int numberStars;

        [Export] private Button restart;
        [Export] private Button next;

        public override void _Ready()
		{
			restart.Pressed += () => UIManager.GetInstance().GoToLevel(UIManager.GetInstance().levelIndex);
			if (UIManager.GetInstance().levelIndex + 1 < GridManager.GetInstance().numberOfLevel) next.Pressed += () => UIManager.GetInstance().GoToLevel(UIManager.GetInstance().levelIndex + 1);
			else next.Pressed += () => UIManager.GetInstance().GoToWinFinal();
        }

		public void CalculScoreLevel()
		{
            if (GameManager.GetInstance().currentLevel.Par >= GameManager.GetInstance().CurrentPar) 
			{
                score = 5000 + (GameManager.GetInstance().currentLevel.Par - GameManager.GetInstance().CurrentPar) * 100;
				numberStars = 3;
            }
            else if (GameManager.GetInstance().currentLevel.Par * 1.5f >= GameManager.GetInstance().CurrentPar)
			{
                score = 2000 + (GameManager.GetInstance().currentLevel.Par - GameManager.GetInstance().CurrentPar) * 50;
				numberStars = 2;
            }
			else 
			{
                score = 1000 + (GameManager.GetInstance().currentLevel.Par - GameManager.GetInstance().CurrentPar) * 50;
				numberStars = 1;
            }

            score = Mathf.Clamp(score, 0, score);

            for (int i = 0; i <= numberStars - 1; i++)
			{
				AnimatedSprite2D lStars = (AnimatedSprite2D)stars.GetChild(i);
				lStars.Frame = 1;
            }
            
			scoreText.Text = "Score : " + score;

			AccountManager.GetInstance().NewWin(score, GameManager.GetInstance().CurrentPar);
        }

		protected override void Dispose(bool pDisposing)
		{

		}
	}
}
