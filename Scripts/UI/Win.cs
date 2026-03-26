using Godot;
using static System.Formats.Asn1.AsnWriter;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban
{
	public partial class Win : Control
	{
		[Export] private Label stars;

        [Export] private Label scoreText;
		private int score;
		private int numberStars;

        [Export] private Button restart;
        [Export] private Button next;

        public override void _Ready()
		{
			restart.Pressed += () => UIManager.GetInstance().GoToLevel(UIManager.GetInstance().levelIndex);
			next.Pressed += () => UIManager.GetInstance().GoToLevel(UIManager.GetInstance().levelIndex + 1);
        }

		public void CalculScoreLevel()
		{
            score = (GameManager.GetInstance().currentLevel.Par - GameManager.GetInstance().CurrentPar) * 50;
			stars.Text = "";

            if (GameManager.GetInstance().currentLevel.Par >= GameManager.GetInstance().CurrentPar) 
			{
                score = 5000 + score * 2;
				numberStars = 3;
            }
            else if (GameManager.GetInstance().currentLevel.Par * 1.5f >= GameManager.GetInstance().CurrentPar)
			{
                score += 2000;
				numberStars = 2;
            }
			else 
			{
                score += 1000;
				numberStars = 1;
            }

			for (int i = 0; i < numberStars; i++) stars.Text += "*";

			if (score < 0) score = 0;

			scoreText.Text = "Score : " + score;
			UIManager.GetInstance().finalScore += score;

			GD.Print(UIManager.GetInstance().finalScore);
        }

		protected override void Dispose(bool pDisposing)
		{

		}
	}
}
