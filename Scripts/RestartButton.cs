using Com.IsartDigital.Utils.Tweens;
using Godot;
using static Godot.OpenXRInterface;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class RestartButton : Button
	{
        private Timer timer = new Timer();
        private float timeBetweenAnimation = 3f;

        private int rotationButton;
        private bool pressed;

        public override void _Ready()
		{
			timer.WaitTime = timeBetweenAnimation;
			timer.Timeout += () => Animation(false);
            AddChild(timer);

            MouseEntered += AnimationMouseEntered;
            MouseExited += AnimationMouseExited;
            Pressed += () => Animation(true);

            PivotOffset = Size / 2;
        }

        private void AnimationMouseEntered()
        {
            rotationButton = -5;
            timer.Start();

            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.OutIn).SetParallel();
            lTween.TweenProperty(this, TweenProp.SCALE, Vector2.One * 1.3f, 0.5f);
            lTween.TweenProperty(this, TweenProp.ROTATION, Mathf.DegToRad(rotationButton), 0.5f);
        }

        private void AnimationMouseExited()
        {
            if (pressed) return;

            rotationButton = 0;
            timer.Stop();

            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.OutIn).SetParallel();
            lTween.TweenProperty(this, TweenProp.SCALE, Vector2.One, 0.5f);
            lTween.TweenProperty(this, TweenProp.ROTATION, Mathf.DegToRad(rotationButton), 0.5f);
        }

        private void Animation(bool pPressed)
		{
            pressed = pPressed;

			Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.InOut);
            lTween.TweenProperty(this, TweenProp.ROTATION, Mathf.DegToRad(360 + rotationButton), 1f);
            lTween.TweenProperty(this, TweenProp.ROTATION, Mathf.DegToRad(rotationButton), 0f);
            lTween.Finished += RotateZero;
        }

        private void RotateZero()
        {
            if (pressed) 
            {
                pressed = false;
                AnimationMouseExited();
            }
        }
	}
}
