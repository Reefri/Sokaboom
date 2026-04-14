using Com.IsartDigital.Utils.Tweens;
using Godot;
using System;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class MenuTransition : Control
	{
		private  static PackedScene factory = (PackedScene)GD.Load("res://Scenes/UI/Transitions/MenuTransition.tscn");

		[Export] private Control TransitionZone;
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
			screenSize = GetRect().Size;
			doubleHeight = screenSize.Y * 2;
			doubleWidth = screenSize.X * 2;
			TransitionZone.Position = new Vector2(doubleWidth, -doubleHeight);
			Vector2 lFinalPos = new Vector2(-screenSize.Y, doubleHeight);

			SoundManager.GetInstance().PlayCloud();

			Tween lTween = TransitionZone.CreateTween();
			lTween.TweenProperty(TransitionZone, TweenProp.POSITION,lFinalPos, tweenDuration);



		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			time += lDelta;
			if (time > tweenDuration) QueueFree();
		}

		public static void Create(Action pSomething)
		{
			MenuTransition lTransition = (MenuTransition)factory.Instantiate();
            lTransition.whenToPlayAnim.Timeout += pSomething;

			UIManager.GetInstance().AddSibling(lTransition);
		}
	}
}
