using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class WinFinal : Control
	{
		[Export] private TextureRect background;

        [Export] private Label congratulationText;
		[Export] Label ScoreFinal;

		[Export] Button highScore;
		[Export] Button menu;

        [Export] private Node2D fireworksPos;

        private const string SCORE = "Score Total : ";
        private float currentScore = 0;
        private float scoreToReach = 1;

		private float sideFactor = 200f;
		[Export] private float tweenDuration = 0.75f;
		private float time = 0;

		private Vector2 screenSize;

        private bool showScore = false;

        public override void _Ready()
		{
			screenSize = GetViewportRect().Size;

            ScoreFinal.Text = SCORE;
            scoreToReach = AccountManager.GetInstance().currentAccount.FinalScore();

			menu.Pressed += UIManager.GetInstance().GoToTitle;
			highScore.Pressed += UIManager.GetInstance().GoToHightScore;

			SettingInitialPositions();

			Animations();

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
