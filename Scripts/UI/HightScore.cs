using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Collections.Generic;
using System.Linq;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class HightScore : Control
	{
		[Export] Button next;
		[Export] Control levelAccountTen;
        [Export] Control levelAccountEleven;
        Control levelAccount;

        int i;
		bool currentAccountInTopTen;

        private const string SCORE = "Score : ";

        List<Account> accounts;


        
        public override void _Ready()
		{
            next.Pressed += () => MenuTransition.Create(UIManager.GetInstance().GoToTitle);
            
			accounts = AccountManager.GetInstance().GetTopPlayers(10);

            foreach (Account lAccount in accounts)
            {
                if (lAccount.Id == AccountManager.GetInstance().currentAccount.Id)
                {
                    levelAccount = levelAccountTen;
                    levelAccountEleven.Visible = false;
                    levelAccountTen.Visible = true;
                    currentAccountInTopTen = true;
                }
            }

            Label lAccountPosition;

            if (!currentAccountInTopTen)
			{
                levelAccount = levelAccountEleven;
                levelAccountTen.Visible = false;
                levelAccountEleven.Visible = true;

                lAccountPosition = (Label)levelAccount.GetChild(10);
                Label lAccountScore = (Label)lAccountPosition.GetChild(0);

                lAccountPosition.Text = "You : " + AccountManager.GetInstance().currentAccount.Id + " ";
                lAccountScore.Text = SCORE + AccountManager.GetInstance().currentAccount.FinalScore();
                
                lAccountPosition.Modulate = new Color(1, 1, 0);
			}
            
            
            foreach (Account lAccount in accounts)
            {
				lAccountPosition = (Label)levelAccount.GetChild(i);
                Label lAccountScore = (Label)lAccountPosition.GetChild(0);
                i++;
                
				lAccountPosition.Text = i + ". " + lAccount.Id + " ";
				lAccountScore.Text = SCORE + lAccount.FinalScore();

                if (lAccount.Id == AccountManager.GetInstance().currentAccount.Id) lAccountPosition.Modulate = new Color(1, 1, 0);
            }

            AnimationTopTen();
        }

        private void AnimationTopTen()
        {
            int j = 0;

            List<Node> lListOfLevelAccount = levelAccount.GetChildren().ToList();
            lListOfLevelAccount.Reverse();

            foreach (Label lAccount in lListOfLevelAccount)
			{
                if (j >= 10) break;
                Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.In).SetParallel();
                lTween.TweenProperty(lAccount, TweenProp.MODULATE_ALPHA, 0f, 0);
                lTween.TweenProperty(lAccount, TweenProp.MODULATE_ALPHA, 1f, 0).SetDelay(j * 0.3);
                lTween.TweenProperty(lAccount, TweenProp.GLOBAL_POSITION_Y, lAccount.GlobalPosition.Y, 1).From(-50).SetDelay(j * 0.3);
                int lIndex = j;
                lTween.Finished += () => DustAnimation(lAccount,(lIndex+5f)/7f);
                j++;
            }
        }

        private void DustAnimation(Label pAccount,float pPitch)
        {
            Label lAccountScore = (Label)pAccount.GetChild(0);

            GpuParticles2D lDustA = (GpuParticles2D)pAccount.GetChild(1);
            GpuParticles2D lDustB = (GpuParticles2D)lAccountScore.GetChild(0);

            lDustB.Position = new Vector2(lAccountScore.Size.X, lAccountScore.Size.Y / 2);

			SoundManager.GetInstance().PlayClick(pPitch);

            lDustA.Emitting = true;
            if (lAccountScore.Text != "") lDustB.Emitting = true;
        }
    }
}
