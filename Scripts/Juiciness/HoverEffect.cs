using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class HoverEffect : Node2D
	{

		private float amplitude = 25;
		private float speed = 1.5f;


		private Tween hoverTween;

		public override void _Ready()
		{

			hoverTween = CreateTween()
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut)
				.SetLoops();


			hoverTween.TweenProperty(this, TweenProp.POSITION, Vector2.Up   * amplitude, speed).AsRelative();

			hoverTween.SetEase(Tween.EaseType.InOut);

			hoverTween.TweenProperty(this, TweenProp.POSITION, Vector2.Down * amplitude, speed).AsRelative() ;


		}


	}
}
