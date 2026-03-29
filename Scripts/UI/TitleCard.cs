using Godot;
using System;
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

        private Timer timer = new Timer();

        private Vector2 finalPositionOfSo;

        private Vector2 posInitK;
        private Vector2 posInitA;

        private bool animationIsRunning = false;
        private bool animationWantToStop = false;

        private Tween tween;

        public override void _Ready()
		{
            finalPositionOfSo = so.Position;

            so.Position = new Vector2(title.Size.X / 2, 18);
            posInitK = letterK.Position;
            posInitA = letterA.Position;

            timer.WaitTime = 0.05f;
            timer.Timeout += MoreO;
            AddChild(timer);
            StartTimer();

            letterK.Visible = false;
            letterA.Visible = false;
            boum.Visible = false;
            exclamationMark.Visible = false;
            explosion.Visible = false;
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
            while (so.Size.X < title.Size.X - 200)
            {
                so.Text += "o";
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
                if (so.Position.X > finalPositionOfSo.X - 25) so.Position = finalPositionOfSo;
                return;
            }
            so.Size = new Vector2(116, 137);
            timer.QueueFree();
            Kaboum();
        }

        private void Kaboum()
        {
            letterK.Visible = true;

            tween = CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out).SetParallel();
            
            tween.TweenProperty(letterK, TweenProp.POSITION, letterK.Position, 2f).From(new Vector2(letterK.Position.X, -265));

            tween.TweenProperty(letterA, TweenProp.VISIBLE, true, 0f).SetDelay(0.2f);
            tween.TweenProperty(letterA, TweenProp.POSITION, letterA.Position, 2f).From(new Vector2(letterA.Position.X, -265)).SetDelay(0.2f);

            tween.TweenProperty(explosion, TweenProp.VISIBLE, true, 0f).SetDelay(1f);
            tween.TweenProperty(explosion, TweenProp.SCALE, explosion.Scale, 1f).From(Vector2.Zero).SetDelay(1f);
            tween.TweenProperty(explosion, TweenProp.ROTATION, explosion.Rotation, 1f).From(-explosion.Rotation).SetDelay(1f);
            tween.TweenProperty(explosion, TweenProp.SKEW, explosion.Skew, 1f).From(-explosion.Skew).SetDelay(1f);

            tween.TweenProperty(boum, TweenProp.VISIBLE, true, 0f).SetDelay(1.5f);
            tween.TweenProperty(boum, TweenProp.SCALE, boum.Scale, 1f).From(Vector2.Zero).SetDelay(1.5f);
            tween.TweenProperty(boum, TweenProp.ROTATION, boum.Rotation, 1f).From(-boum.Rotation).SetDelay(1.5f);

            tween.Finished += AnimationIsFinish;
        }

        private void AnimationIsFinish()
        {
            animationIsRunning = false;

            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Elastic).SetEase(Tween.EaseType.Out).SetParallel();
            lTween.TweenProperty(exclamationMark, TweenProp.VISIBLE, true, 0f).SetDelay(0.2f);
            lTween.TweenProperty(exclamationMark, TweenProp.POSITION, exclamationMark.Position, 2f).From(new Vector2(exclamationMark.Position.X, -265)).SetDelay(0.2f);
        }

        private void FinishAnimation()
        {
            if (IsInstanceValid(timer)) 
            {
                timer.QueueFree();
                so.Position = finalPositionOfSo;
                so.Size = new Vector2(116, 137);
                so.Text = so.Text.Left(2);
            }
            
            letterK.Visible = true;
            letterA.Visible = true;
            explosion.Visible = true;
            boum.Visible = true;

            letterK.Position = posInitK;
            letterA.Position = posInitA;

            explosion.Scale = new Vector2(0.85f, 0.85f);
            boum.Scale = new Vector2(1, 1);

            explosion.Rotation = Mathf.DegToRad(6);
            explosion.Skew = Mathf.DegToRad(23.2f);

            boum.Rotation = Mathf.DegToRad(6);

            animationWantToStop = false;
            AnimationIsFinish();
        }

        private void PlayPressed()
        {
            UIManager.GetInstance().GoToLevelSelect();
        }

        private void HelpPressed()
        {
            UIManager.GetInstance().GoToHelp();
            UIManager.GetInstance().comeToMenu = true;
        }

        public void Langage()
        {
            TranslationServer.SetLocale(TranslationServer.GetLocale() == "en" ? "fr" : "en");
        }
    }
}
