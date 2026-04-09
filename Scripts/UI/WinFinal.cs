using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class WinFinal : Control
	{
		[Export] Label ScoreFinal;

		[Export] Button highScore;
		[Export] Button menu;

        private const string SCORE = "Score Total : ";

        public override void _Ready()
		{
			ScoreFinal.Text = SCORE + AccountManager.GetInstance().currentAccount.FinalScore();

			menu.Pressed += UIManager.GetInstance().GoToTitle;
			highScore.Pressed += UIManager.GetInstance().GoToHightScore;
        }
	}
}
