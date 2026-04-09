using Godot;
using Com.IsartDigital.Utils.Tweens;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban.UI
{
	public partial class TitleCard : Control
	{
        [Export] private Control title;
        [Export] private Label so;
        [Export] private Label letterK;
        [Export] private Label letterA;
        [Export] private Sprite2D explosion;
        [Export] private Control boum;
        [Export] private Label letterB;
        [Export] private Label letterO;
        [Export] private Label letterU;
        [Export] private Label letterM;
        [Export] private Label exclamationMark;

        [Export] private Button hightScore;
        [Export] private Button play;

        private Timer timer = new Timer();

        private Vector2 finalPositionOfSo;

        private Vector2 posInitK;
        private Vector2 posInitA;

        private bool animationIsRunning = false;
        private bool animationWantToStop = false;

        private Tween tween;

        private const string ENGLISH = "en";
        private const string FRENCH = "fr";
        private const string TWEEN_FOR_O = "o";


        private const float WAIT_TIME_START = 0.05f;
        private const float LONG_DELAY = 1.5f;
        private const float SHORT_DELAY = 0.2f;
        private const float EXPLOSION_SCALE = 0.85f;
        private const float EXPLOSION_SKEW = 23.2f;

        private const int INITIAL_Y_POSITION = 265;
        private const int NUMBER_OF_O = 18;
        private const int MARGIN_BETWEEN_O = 25;
        private const int TOTAL_SPACE_BETWEEN_O = 200;
        private Vector2 soSize = new Vector2(116, 137);
        private const int ROTATION_FOR_TWEEN = 6;

        public override void _Ready()
		{
            finalPositionOfSo = so.Position;

            so.Position = new Vector2(title.Size.X / 2, NUMBER_OF_O);
            posInitK = letterK.Position;
            posInitA = letterA.Position;

            timer.WaitTime = WAIT_TIME_START;
            timer.Timeout += MoreO;
            AddChild(timer);
            StartTimer();

            letterK.Visible = false;
            letterA.Visible = false;
            boum.Visible = false;
            exclamationMark.Visible = false;
            explosion.Visible = false;

            hightScore.Pressed += UIManager.GetInstance().GoToHightScore;
            play.Pressed += UIManager.GetInstance().GoToLevelSelect;
        }

        public override void _Input(InputEvent pEvent)
        {
            if (animationIsRunning && !animationWantToStop && pEvent is InputEventMouseButton lMouseEvent && lMouseEvent.Pressed && lMouseEvent.ButtonIndex == MouseButton.Left)
            {
                animationWantToStop = true;
                FinishAnimation();
                if (tween != null) tween.Stop();
            }
        }

        private void StartTimer()
        {
            timer.Start();
            animationIsRunning = true;
        }

        private void MoreO()
        {
            while (so.Size.X < title.Size.X - TOTAL_SPACE_BETWEEN_O)
            {
                so.Text += TWEEN_FOR_O;
                return;
            }
            timer.Timeout -= MoreO;
            timer.Timeout += LessO;
        }

        private void LessO()
        {
            while (so.Text.Length > 2)
            {
                so.Text = so.Text.Left(so.Text.Length - 1);
                if (so.Position.X > finalPositionOfSo.X - MARGIN_BETWEEN_O) so.Position = finalPositionOfSo;
                return;
            }
            so.Size = soSize;
            timer.QueueFree();
            Kaboum();
        }

        private void Kaboum()
        {
            letterK.Visible = true;

            tween = CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out).SetParallel();
            
            tween.TweenProperty(letterK, TweenProp.POSITION, letterK.Position, 2f).From(new Vector2(letterK.Position.X, INITIAL_Y_POSITION));

            tween.TweenProperty(letterA, TweenProp.VISIBLE, true, 0f).SetDelay(SHORT_DELAY);
            tween.TweenProperty(letterA, TweenProp.POSITION, letterA.Position, 2f).From(new Vector2(letterA.Position.X, INITIAL_Y_POSITION)).SetDelay(SHORT_DELAY);

            tween.TweenProperty(explosion, TweenProp.VISIBLE, true, 0f).SetDelay(1f);
            tween.TweenProperty(explosion, TweenProp.SCALE, explosion.Scale, 1f).From(Vector2.Zero).SetDelay(1f);
            tween.TweenProperty(explosion, TweenProp.ROTATION, explosion.Rotation, 1f).From(-explosion.Rotation).SetDelay(1f);
            tween.TweenProperty(explosion, TweenProp.SKEW, explosion.Skew, 1f).From(-explosion.Skew).SetDelay(1f);

            tween.TweenProperty(boum, TweenProp.VISIBLE, true, 0f).SetDelay(LONG_DELAY);
            tween.TweenProperty(boum, TweenProp.SCALE, boum.Scale, 1f).From(Vector2.Zero).SetDelay(LONG_DELAY);
            tween.TweenProperty(boum, TweenProp.ROTATION, boum.Rotation, 1f).From(-boum.Rotation).SetDelay(LONG_DELAY);

            tween.Finished += AnimationIsFinish;
        }

        private void AnimationIsFinish()
        {
            animationIsRunning = false;

            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out).SetParallel();
            lTween.TweenProperty(exclamationMark, TweenProp.VISIBLE, true, 0f).SetDelay(SHORT_DELAY);
            lTween.TweenProperty(exclamationMark, TweenProp.POSITION, exclamationMark.Position, 2f).From(new Vector2(exclamationMark.Position.X, INITIAL_Y_POSITION)).SetDelay(SHORT_DELAY);
        }

        private void FinishAnimation()
        {
            if (IsInstanceValid(timer)) 
            {
                timer.QueueFree();
                so.Position = finalPositionOfSo;
                so.Size = soSize;
                so.Text = so.Text.Left(2);
            }
            
            letterK.Visible = true;
            letterA.Visible = true;
            explosion.Visible = true;
            boum.Visible = true;

            letterK.Position = posInitK;
            letterA.Position = posInitA;

            explosion.Scale = new Vector2(EXPLOSION_SCALE, EXPLOSION_SCALE);
            boum.Scale = Vector2.One;

            explosion.Rotation = Mathf.DegToRad(ROTATION_FOR_TWEEN);
            explosion.Skew = Mathf.DegToRad(EXPLOSION_SKEW);

            boum.Rotation = Mathf.DegToRad(ROTATION_FOR_TWEEN);

            animationWantToStop = false;
            AnimationIsFinish();
        }

        private void HelpPressed()
        {
            UIManager.GetInstance().comeToMenu = true;
            UIManager.GetInstance().GoToHelp();
        }

        public void Langage()
        {
            TranslationServer.SetLocale(TranslationServer.GetLocale() == ENGLISH ? FRENCH : ENGLISH);
        }
    }
}
