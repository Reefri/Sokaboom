using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Collections.Generic;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class WinFinal : Control
	{
		[Export] private TextureRect background;

        [Export] private Label congratulationText;
		[Export] Label ScoreFinal;
		[Export] Label rank;

		[Export] Button highScore;
		[Export] Button menu;

        [Export] private Node2D fireworksPos;
        [Export] private Node2D explosionParticlesParent;

        private const string SCORE = "Score Total : ";
        private const string FONT_SIZE_PATH = "theme_override_font_sizes/font_size";
        private int currentScore = 0;
        private int scoreToReach = 1;

		private float sideFactor = 200f;
		[Export] private float tweenDuration = 0.75f;
		private float time = 0;

        private float baseRankSize;

		private Vector2 screenSize;

        private bool showScore = false;

        [Export] private const int S_RANK_THRESHHOLD = 65000;
        [Export] private const int A_RANK_THRESHHOLD = 60000;
        [Export] private const int B_RANK_THRESHHOLD = 50000;
        [Export] private const int C_RANK_THRESHHOLD = 40000;
        [Export] private const int D_RANK_THRESHHOLD = 25000;
        [Export] private const int E_RANK_THRESHHOLD = 10000;

        [Export] private const int F_RANK_THRESHHOLD = 0;

        private const string S_RANK = "S";
        private const string A_RANK = "A";
        private const string B_RANK = "B";
        private const string C_RANK = "C";
        private const string D_RANK = "D";
        private const string E_RANK = "E";
        private const string F_RANK = "F";

        private const int S_RANK_SIZE = 500;
        private const int A_RANK_SIZE = 375;
        private const int B_RANK_SIZE = 350;
        private const int C_RANK_SIZE = 325;
        private const int D_RANK_SIZE = 300;
        private const int E_RANK_SIZE = 275;
        private const int F_RANK_SIZE = 250;

        private const float S_RANK_ROTATION = -Mathf.Pi/4;
        private const float A_RANK_ROTATION = 0;
        private const float B_RANK_ROTATION = Mathf.Pi / 4;
        private const float C_RANK_ROTATION = 0;
        private const float D_RANK_ROTATION = -Mathf.Pi/4;
        private const float E_RANK_ROTATION = 0;
        private const float F_RANK_ROTATION = Mathf.Pi/4;

        private List<int> rankThresholds;
        private List<string> rankLetters;
        private List<int> rankSizes;
        private List<float> rankRotation;

        public override void _Ready()
		{
			screenSize = GetViewportRect().Size;

            ScoreFinal.Text = SCORE;
            //scoreToReach = AccountManager.GetInstance().currentAccount.FinalScore();
            scoreToReach = 65000;

			menu.Pressed += UIManager.GetInstance().GoToTitle;
			highScore.Pressed += UIManager.GetInstance().GoToHightScore;

			SettingInitialPositions();

			Animations();

            rank.PivotOffset = rank.Size / 2;
            rank.Scale = Vector2.Zero;


            rankThresholds = new List<int>() 
            {
                F_RANK_THRESHHOLD,
                E_RANK_THRESHHOLD,
                D_RANK_THRESHHOLD,
                C_RANK_THRESHHOLD,
                B_RANK_THRESHHOLD,
                A_RANK_THRESHHOLD,
                S_RANK_THRESHHOLD,
            };

            rankLetters = new List<string>()
            {
                F_RANK,
                E_RANK,
                D_RANK,
                C_RANK,
                B_RANK,
                A_RANK,
                S_RANK,
            };

            rankSizes = new List<int>()
            {
                F_RANK_SIZE,
                E_RANK_SIZE,
                D_RANK_SIZE,
                C_RANK_SIZE,
                B_RANK_SIZE,
                A_RANK_SIZE,
                S_RANK_SIZE,
            };

            rankRotation = new List<float>()
            {
                F_RANK_ROTATION,
                E_RANK_ROTATION,
                D_RANK_ROTATION,
                C_RANK_ROTATION,
                B_RANK_ROTATION,
                A_RANK_ROTATION,
                S_RANK_ROTATION,
            };

            baseRankSize = (float)rank.Get(FONT_SIZE_PATH);
        }

		private void SettingInitialPositions()
		{
            background.Position = new Vector2(screenSize.X / 2 - background.Size.X/2, -screenSize.Y * 2);

			menu.Position = new Vector2(screenSize.X + sideFactor,	screenSize.Y / 2 + sideFactor) - menu.Size/2;

			highScore.Position = new Vector2(screenSize.X + sideFactor, screenSize.Y / 2) - highScore.Size/2;

            ScoreFinal.Scale = Vector2.Zero;

            
            congratulationText.PivotOffset = congratulationText.Size / 2;
            congratulationText.Scale = Vector2.Zero;
        }

        private void Animations()
        {
            Tween lTween = CreateTween();

            lTween.SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Expo);

            lTween.TweenProperty(background, TweenProp.POSITION, screenSize / 2 - background.Size / 2, tweenDuration);
            lTween.SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Bounce);
            lTween.TweenCallback
                    (
                    Callable.From(() =>
                         background.PivotOffset = new Vector2(background.Size.X / 2, background.Size.Y))
                    );
            lTween.TweenProperty(background, TweenProp.SCALE, new Vector2(1, 0.1f), tweenDuration / 2);
            lTween.Parallel().TweenProperty(background, TweenProp.SCALE, new Vector2(2, 0.1f), tweenDuration / 2);
            lTween.TweenProperty(background, TweenProp.SCALE, Vector2.One, tweenDuration / 4).SetDelay(tweenDuration / 4);


            lTween.SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Back);
            lTween.TweenProperty(menu, TweenProp.POSITION, screenSize / 2 + Vector2.Down * sideFactor - menu.Size / 2, tweenDuration).SetDelay(tweenDuration / 4);

            lTween.Parallel().TweenProperty(highScore, TweenProp.POSITION, screenSize / 2 - highScore.Size / 2, tweenDuration).SetDelay(tweenDuration / 4);

            lTween.Parallel().TweenProperty(ScoreFinal, TweenProp.POSITION, screenSize / 2 + Vector2.Up * sideFactor/1.25f - ScoreFinal.Size / 2, tweenDuration).SetDelay(tweenDuration / 4);


            lTween.SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Expo);
            lTween.TweenCallback
                    (
            Callable.From(() =>
                         menu.PivotOffset = new Vector2(0, menu.Size.Y))
                    ); 
            lTween.TweenCallback
                    (
            Callable.From(() =>
                         highScore.PivotOffset = new Vector2(0, highScore.Size.Y))
                    );

            lTween.TweenProperty(menu, TweenProp.ROTATION, -Mathf.Pi/2 , tweenDuration);

            lTween.Parallel().TweenProperty(highScore, TweenProp.ROTATION, -Mathf.Pi / 2, tweenDuration);


            lTween.SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Back);

            lTween.TweenProperty(menu, TweenProp.ROTATION, 0, tweenDuration/2);

            lTween.Parallel().TweenProperty(highScore, TweenProp.ROTATION, 0, tweenDuration/2);

            lTween.Parallel().TweenProperty(ScoreFinal, TweenProp.ROTATION, 0, tweenDuration / 2);


            lTween.TweenCallback
                   (
           Callable.From(() =>
                        ScoreFinal.PivotOffset = ScoreFinal.Size/2)
                   );
            lTween.SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Elastic);
            lTween.TweenProperty(ScoreFinal, TweenProp.SCALE, Vector2.One, tweenDuration).SetDelay(tweenDuration/2);
            lTween.Parallel().TweenProperty(ScoreFinal, TweenProp.ROTATION, Mathf.Pi * 8, tweenDuration).SetDelay(tweenDuration/2);


            lTween.SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Quad);

            lTween.TweenProperty(ScoreFinal, TweenProp.SCALE, Vector2.One, tweenDuration).SetDelay(tweenDuration / 2);

            lTween.TweenCallback
                    (
            Callable.From(() =>
                         showScore = true)
                    );
        }

        private void Congratulations()
        {
            fireworksPos.Position = screenSize / 2;

            FireWork.CreateMult(Vector2.Zero, fireworksPos);
            FireWork.CreateMult(Vector2.Zero, fireworksPos);
            FireWork.CreateMult(Vector2.Zero, fireworksPos);

            Tween lTween = CreateTween()
                .SetTrans(Tween.TransitionType.Elastic)
                .SetEase(Tween.EaseType.Out);

            lTween.TweenProperty(congratulationText, TweenProp.SCALE, Vector2.One, tweenDuration).SetDelay(tweenDuration / 2);

            lTween.TweenCallback
                    (
            Callable.From(() =>
                         FinalRanking())
                    );
        }

        private void FinalRanking()
        {
            foreach (GpuParticles2D lParticles in explosionParticlesParent.GetChildren()) lParticles.Emitting = true;

            Tween lTween = CreateTween()
                .SetTrans(Tween.TransitionType.Elastic)
                .SetEase(Tween.EaseType.Out);

            lTween.TweenProperty(rank, TweenProp.SCALE, Vector2.One, tweenDuration).SetDelay(tweenDuration / 3);

                lTween.SetTrans(Tween.TransitionType.Circ)
                .SetEase(Tween.EaseType.Out);

            lTween.Parallel().TweenProperty(rank, TweenProp.ROTATION, Mathf.Pi * 50, tweenDuration * 4);

            lTween.TweenCallback
                    (
            Callable.From(() =>
                         TestRank(currentScore))
                    );
        }

        private void TestRank(int pScore)
        {
            //string[] lArray = rank.Theme.GetFontTypeList();
            //GD.Print(lArray);

            Tween lTween = CreateTween()
                .SetTrans(Tween.TransitionType.Elastic)
                .SetEase(Tween.EaseType.Out);

            for (int i = 0; i < rankThresholds.Count; i++)
            {
                if (currentScore >= rankThresholds[i])
                {
                    if (i > 0)
                    {
                        lTween.Parallel().TweenProperty(rank, TweenProp.TEXT, rankLetters[i], 0).SetDelay(tweenDuration);

                        lTween.TweenProperty(rank, FONT_SIZE_PATH, rankSizes[i], tweenDuration / 2);


                    }

                    else
                        lTween.TweenProperty(rank, TweenProp.TEXT, rankLetters[i], 0);


                }
            }

            
        }
        public override void _Process(double delta)
        {
            base._Process(delta);
			screenSize = GetViewportRect().Size;


            if(showScore) {
            time += (float)delta;

                if (currentScore != scoreToReach)
                {
                    currentScore = (int)Tween.InterpolateValue(currentScore, scoreToReach - currentScore, time, tweenDuration * 3, Tween.TransitionType.Quad, Tween.EaseType.In);

                    if (currentScore >= scoreToReach)
                    {
                        currentScore = scoreToReach;
                        showScore = false;
                        Congratulations();
                    }

                    ScoreFinal.Text = SCORE + currentScore;
                }
            }

        }
    }
}
