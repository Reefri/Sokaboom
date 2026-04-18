using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Collections.Generic;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban
{
	public partial class Win : Control
	{
		[Export] public Control stars;

        [Export] private GpuParticles2D confettis;
        [Export] private Label scoreText;
        [Export] private Label perfectText;
        [Export] private GpuParticles2D perfectParticles;
		private int score ;

        private int scoreToReach;
		private int numberStars;

        private float time = 0;
        [Export]private float winScoreDuration = 1;

        [Export] private Button restart;
        [Export] private Button next;

        [Export] private Control explosionParticles;

        private bool showScore = false;

        private const string SCORE = "Score : ";

        RandomNumberGenerator rand = new RandomNumberGenerator();

        public override void _Ready()
		{

            GameManager.GetInstance().QueueFree();

            SoundManager.GetInstance().PlayFireworkWhistleWin();


            List<Node> lStars = stars.GetChildren().ToList();

            foreach (Control lStar in lStars) 
            {
                AnimatedSprite2D star = (AnimatedSprite2D)lStar.GetChild(0);
                star.Frame = 0;
            }
            CalculScoreLevel();

            restart.Pressed += () => UIManager.GetInstance().GoToLevel(UIManager.GetInstance().levelIndex);

            if (UIManager.GetInstance().levelIndex + 1 < GridManager.GetInstance().numberOfLevel) next.Pressed += () => UIManager.GetInstance().GoToLevel(UIManager.GetInstance().levelIndex + 1);
			else
            {
                Banderole.GetInstance().winFinal = true;
                next.Pressed += Banderole.GetInstance().StartTransitionToWin;
            }

            perfectText.PivotOffset = perfectText.Size / 2;
            perfectText.Scale = Vector2.Zero;
            perfectParticles.Visible = false;
        }

      

        public override void _Process(double delta)
        {
            base._Process(delta);

            if (Input.IsActionJustPressed("BackToLevelSelect"))
            {
                UIManager.GetInstance().GoToLevelSelect();

            }

            if (showScore)
            {
                time += (float)delta;

                if (score != scoreToReach)
                {
                    score = (int)Tween.InterpolateValue(score, scoreToReach - score, time, winScoreDuration * 3, Tween.TransitionType.Quad, Tween.EaseType.In);
                    if (score >= scoreToReach)
                    {
                        score = scoreToReach;
                        score = (score >= 0) ? score : 0;
                        showScore = false;

                        if (score >= 5000)
                        {
                            Tween lTween = CreateTween()
                            .SetTrans(Tween.TransitionType.Expo)
                            .SetEase(Tween.EaseType.In);

                            lTween.TweenProperty(perfectText, TweenProp.SCALE, Vector2.One, winScoreDuration/2);
                            lTween.Parallel().TweenProperty(perfectParticles, TweenProp.VISIBLE, true, winScoreDuration).SetDelay(winScoreDuration / 2);


                            SoundManager.GetInstance().PlayExplosion();

                            foreach (GpuParticles2D lParticles in explosionParticles.GetChildren())
                            {
                                lParticles.Emitting = true;
                            }

                            
                        }
                    }

                    scoreText.Text = SCORE + score;
                }
            }
        }

		private void CalculScoreLevel()
		{
            if (GameManager.GetInstance().currentLevel.Par >= GameManager.GetInstance().CurrentPar) 
			{
                scoreToReach = 5000 + (GameManager.GetInstance().currentLevel.Par - GameManager.GetInstance().CurrentPar) * 100;
				numberStars = 3;
            }
            else if (GameManager.GetInstance().currentLevel.Par * 1.5f >= GameManager.GetInstance().CurrentPar)
			{
                scoreToReach = 2000 + (GameManager.GetInstance().currentLevel.Par - GameManager.GetInstance().CurrentPar) * 50;
				numberStars = 2;
            }
			else 
			{
                scoreToReach = 1000 + (GameManager.GetInstance().currentLevel.Par - GameManager.GetInstance().CurrentPar) * 50;
				numberStars = 1;
            }
            scoreToReach = (scoreToReach >= 0) ? scoreToReach : 0;

            for (int i = 0; i < numberStars; i++)
			{
                Control controlStar = (Control)stars.GetChild(i);
                AnimatedSprite2D lStars = (AnimatedSprite2D)controlStar.GetChild(0);
                lStars.Scale = Vector2.Zero;
                AnimationStars(lStars, i * 0.5f,i);
            }

            confettis.Emitting = true;


            AccountManager.GetInstance().NewWin(scoreToReach, GameManager.GetInstance().CurrentPar);
        }

        private void AnimationStars(AnimatedSprite2D pStars, float pDelay,int pIndex)
        {
            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out).SetParallel();
            

            lTween.TweenProperty(pStars, TweenProp.FRAME, 1, 0).SetDelay(pDelay);
            lTween.TweenProperty(pStars, TweenProp.SCALE, Vector2.One*(0.8f + 0.1f), 1f).SetDelay(pDelay);
            lTween.TweenProperty(pStars, TweenProp.ROTATION, Mathf.Tau, 1f).AsRelative().SetDelay(pDelay);
            lTween.TweenCallback(
                Callable.From(() =>
                {
                    SoundManager.GetInstance().PlayStarIndex(pIndex);
                }
                )).SetDelay(pDelay);

            lTween.Finished += () => ParticulesStars(pStars);
        }

        private void ParticulesStars(AnimatedSprite2D pStars)
        {
            GpuParticles2D lParticules = (GpuParticles2D)pStars.GetChild(0);
            lParticules.Emitting = true;

            
            FireWork.CreateMult(null , pStars, Vector2.Zero,false);

            showScore = true;
        }
    }
}
