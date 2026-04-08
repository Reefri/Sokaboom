using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class MenuTransition : Control
	{
		private  static PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/Transitions/MenuTransition.tscn");

		[Export] private Node2D TransitionZone;
		[Export] public float tweenDuration = 1;

		private float time = 0;
		private float doubleWidth;
		private float doubleHeight;

		private Vector2 screenSize;

		[Export] private Timer whenToPlayAnim;
		public override void _Ready()
		{
			whenToPlayAnim.WaitTime = tweenDuration / 1.7f;
			whenToPlayAnim.Start();
			whenToPlayAnim.Timeout += UIManager.GetInstance().GoToTitle;
			screenSize = GetRect().Size;
			doubleHeight = screenSize.Y * 2;
			doubleWidth = screenSize.X * 2;
			TransitionZone.Position = new Vector2(doubleWidth, -doubleHeight);
			Vector2 lFinalPos = new Vector2(-screenSize.Y, doubleHeight);

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

		public static void Create()
		{
			MenuTransition lTransition = (MenuTransition)factory.Instantiate();
			UIManager.GetInstance().AddSibling(lTransition);
		}
	}
}
