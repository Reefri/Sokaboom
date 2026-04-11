using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class RestartButton : Button
	{
		Timer timer = new Timer();
		float timeBetweenAnimation = 10f;

		public override void _Ready()
		{
			timer.WaitTime = timeBetweenAnimation;
			timer.Timeout += Animation;
			timer.Autostart = true;
			AddChild(timer);
		}

		private void Animation()
		{
			Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.InOut);
			lTween.TweenProperty(this, TweenProp.ROTATION, Mathf.DegToRad(360), 2f);
            lTween.TweenProperty(this, TweenProp.ROTATION, 0, 0f);
        }
	}
}
