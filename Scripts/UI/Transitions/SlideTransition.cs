using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Net;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class SlideTransition : Control
	{
		[Export] private Node2D leftDoor;
		[Export] private Node2D rightDoor;

		[Export] private float tweenDuration = 1;

		[Export] private Timer animationTimer;
		[Export] private float doorClosedDuration = 0.5f;

		private float startingPosFactor;
		private float time = 0;

		private float animationDuration;
		public float canChangeLevel;

        public override void _Ready()
		{
            canChangeLevel = tweenDuration + doorClosedDuration;
            animationDuration = canChangeLevel + tweenDuration;

            animationTimer.WaitTime = tweenDuration + doorClosedDuration;
			animationTimer.Start();
            animationTimer.Timeout += ReverseAnimation;

			startingPosFactor = GetRect().Size.X / 2;

			leftDoor.Position += Vector2.Left * startingPosFactor;
			rightDoor.Position += Vector2.Right * startingPosFactor;

			Tween lLeftDoorTween = leftDoor.CreateTween()
                .SetTrans(Tween.TransitionType.Bounce)
                .SetEase(Tween.EaseType.Out);
			lLeftDoorTween.TweenProperty(leftDoor, TweenProp.POSITION,
				GetRect().Size/2 + Vector2.Left * startingPosFactor / 2, tweenDuration);

            Tween lRightDoorTween = rightDoor.CreateTween()
                .SetTrans(Tween.TransitionType.Bounce)
                .SetEase(Tween.EaseType.Out);
            lRightDoorTween.TweenProperty(rightDoor, TweenProp.POSITION,
                GetRect().Size / 2 + Vector2.Right * startingPosFactor/2, tweenDuration);
        }

        private void ReverseAnimation()
        {
            Tween lLeftDoorTween = leftDoor.CreateTween()
                .SetTrans(Tween.TransitionType.Expo)
                .SetEase(Tween.EaseType.In);
            lLeftDoorTween.TweenProperty(leftDoor, TweenProp.POSITION,
                Vector2.Down * GetRect().Size / 2 + Vector2.Left * startingPosFactor, tweenDuration);

            Tween lRightDoorTween = rightDoor.CreateTween()
                .SetTrans(Tween.TransitionType.Expo)
                .SetEase(Tween.EaseType.In);
            lRightDoorTween.TweenProperty(rightDoor, TweenProp.POSITION,
                (Vector2.Down/2 + Vector2.Right) * GetRect().Size + Vector2.Right * startingPosFactor, tweenDuration);
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			time += lDelta;

			if (time > animationDuration) QueueFree();
		}
		protected override void Dispose(bool pDisposing)
		{

		}
	}
}
