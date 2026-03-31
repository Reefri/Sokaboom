using Com.IsartDigital.Utils.Tweens;
using Godot;
using System;
using System.Net;
using System.Reflection;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class SlideTransition : Control
	{
        private static PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/Transitions/SlideTransition.tscn");

		[Export] private Node2D leftDoor;
		[Export] private Node2D rightDoor;

		[Export] private float tweenDuration = 1;

		[Export] public Timer animationTimer;
        [Export] public  Timer whenToPlayAnim;
		[Export] private float doorClosedDuration = 0.5f;

		private float startingPosFactor;
		public float time = 0;

		private float animationDuration;

        public override void _Ready()
		{
            whenToPlayAnim.WaitTime = tweenDuration + doorClosedDuration;

            animationDuration = (float)whenToPlayAnim.WaitTime + tweenDuration;

            animationTimer.WaitTime = tweenDuration + doorClosedDuration;
            animationTimer.Timeout += ReverseAnimation;

            whenToPlayAnim.Timeout += UIManager.GetInstance().ContinueToLevel;

			startingPosFactor = GetWindow().Size.X / 2;

			leftDoor.Position += Vector2.Left * startingPosFactor;
			rightDoor.Position += Vector2.Right * startingPosFactor;

			Tween lLeftDoorTween = leftDoor.CreateTween()
                .SetTrans(Tween.TransitionType.Bounce)
                .SetEase(Tween.EaseType.Out);
			lLeftDoorTween.TweenProperty(leftDoor, TweenProp.POSITION,
				GetRect().Size/2 + Vector2.Left * startingPosFactor / 2 , tweenDuration);

            Tween lRightDoorTween = rightDoor.CreateTween()
                .SetTrans(Tween.TransitionType.Bounce)
                .SetEase(Tween.EaseType.Out);
            lRightDoorTween.TweenProperty(rightDoor, TweenProp.POSITION,
                GetRect().Size / 2 + Vector2.Right * startingPosFactor / 2, tweenDuration);
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

		public static void Create()
		{
            SlideTransition lLevelOpenTransition = (SlideTransition)factory.Instantiate();
            UIManager.GetInstance().AddChild(lLevelOpenTransition);
        }
		protected override void Dispose(bool pDisposing)
		{

		}
	}
}
