using Godot;
using System.Collections.Generic;
using System.Security.Principal;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HightScore : Control
	{
		[Export] Button next;
		[Export] BoxContainer levelAccount;

		int i;
		bool currentAccountInTopTen;

		List<Account> accounts;
        public override void _Ready()
		{
            next.Pressed += () => UIManager.GetInstance().GoToTitle();
            
			accounts = AccountManager.GetInstance().GetTopPlayers(10);

			Label lAccountPosition;
            foreach (Account lAccount in accounts)
            {
				lAccountPosition = (Label)levelAccount.GetChild(i);
                Label lAccountScore = (Label)lAccountPosition.GetChild(0);
                i++;

				lAccountPosition.Text = i + ". " + lAccount.Id + " ";
				lAccountScore.Text = "Score : " + lAccount.FinalScore();

                if (lAccount.Id == AccountManager.GetInstance().currentAccount.Id) 
				{
                    lAccountPosition.Modulate = new Color(1, 1, 0);
					currentAccountInTopTen = true;
                }
            }

			if (!currentAccountInTopTen)
			{
				lAccountPosition = (Label)levelAccount.GetChild(10);
				lAccountPosition.Text = "11. " + AccountManager.GetInstance().currentAccount.Id + " " + AccountManager.GetInstance().currentAccount.FinalScore();
				lAccountPosition.Modulate = new Color(1, 1, 0);
				lAccountPosition.Visible = true;
			}
        }
	}
}
