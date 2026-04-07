using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban
{
	public partial class Win : Control
	{
		[Export] public Control stars;

        [Export] private GpuParticles2D confettis;
        [Export] private Label scoreText;
		private int score;
		private int numberStars;

        [Export] private Button restart;
        [Export] private Button next;

        private const string SCORE = "Score : ";

        RandomNumberGenerator rand = new RandomNumberGenerator();

        public override void _Ready()
		{
            GameManager.GetInstance().QueueFree();

            foreach (AnimatedSprite2D lStars in stars.GetChildren()) lStars.Frame = 0;
            CalculScoreLevel();

            restart.Pressed += () => UIManager.GetInstance().GoToLevel(UIManager.GetInstance().levelIndex);

            if (UIManager.GetInstance().levelIndex + 1 < GridManager.GetInstance().numberOfLevel) next.Pressed += () => UIManager.GetInstance().GoToLevel(UIManager.GetInstance().levelIndex + 1);
			else next.Pressed += () => UIManager.GetInstance().GoToWinFinal();
        }

		private void CalculScoreLevel()
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

            score = (score>0)?score:0;

            for (int i = 0; i <= numberStars - 1; i++)
			{
				AnimatedSprite2D lStars = (AnimatedSprite2D)stars.GetChild(i);
                lStars.Scale = Vector2.Zero;
                AnimationStars(lStars, i * 0.5f);
            }
            
			scoreText.Text = SCORE + score;

			AccountManager.GetInstance().NewWin(score, GameManager.GetInstance().CurrentPar);
        }

        private void AnimationStars(AnimatedSprite2D pStars, float pDelay)
        {
            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out).SetParallel();
            lTween.TweenProperty(pStars, TweenProp.FRAME, 1, 0).SetDelay(pDelay);
            lTween.TweenProperty(pStars, TweenProp.SCALE, Vector2.One*(0.8f + 0.1f), 1f).SetDelay(pDelay);
            lTween.TweenProperty(pStars, TweenProp.ROTATION, Mathf.Tau, 1f).AsRelative().SetDelay(pDelay);

            lTween.Finished += () => ParticulesStars(pStars);
        }

        private void ParticulesStars(AnimatedSprite2D pStars)
        {
            GpuParticles2D lParticules = (GpuParticles2D)pStars.GetChild(0);
            lParticules.Emitting = true;

            for (int i = numberStars - 1; i < 3; i++)
            {
                FireWork.CreateMult(stars.GlobalPosition + new Vector2(rand.RandiRange(-500, 100), rand.RandiRange(-100, 100)), pStars);
            }
            confettis.Emitting = true;
        }
    }
}
