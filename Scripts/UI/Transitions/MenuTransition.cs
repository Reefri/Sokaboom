using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class MenuTransition : Control
	{
		[Export] private Node2D TransitionZone;
		[Export] private float tweenDuration = 1;

		private float time = 0;

		private Vector2 screenSize;
		public override void _Ready()
		{
			screenSize = GetRect().Size;
			TransitionZone.Position = Vector2.Right * screenSize.X * 2 + Vector2.Up * screenSize.Y * 2;
			Vector2 lFinalPos = Vector2.Down * screenSize.Y * 2 + Vector2.Left * screenSize.Y;

			Tween lTween = TransitionZone.CreateTween();
			lTween.TweenProperty(TransitionZone, TweenProp.POSITION,lFinalPos, tweenDuration);
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			time += lDelta;
			if (time > tweenDuration) QueueFree();
		}

		protected override void Dispose(bool pDisposing)
		{

		}
	}
}
