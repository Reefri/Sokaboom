using Com.IsartDigital.Utils.Tweens;
using Godot;
using Godot.Collections;
using System;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban 
{
	public partial class Banderole : Control
	{
		static private Banderole instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/Transitions/BanderoleTransition.tscn");


		[Export] Array<Sprite2D> banderoles;

		[Export] Array<Marker2D> downMarkers;
		[Export] Array<Marker2D> upMarkers;

		private int indexOfBanderoles = 4;

		private Timer timer = new Timer();
		private const float WAIT_TIME_WIN = 0.5f;

		private Timer timerBetweenTransitions = new Timer();
		private const float TIME_BETWEEN_TRANSITIONS = 0.4f;

        private Banderole():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(Banderole) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public Banderole GetInstance()
		{
			if (instance == null) instance = (Banderole)factory.Instantiate();
			return instance;
		}

		public override void _Ready()
		{
			Visible = false;
			timer.WaitTime = WAIT_TIME_WIN;
			timerBetweenTransitions.WaitTime = TIME_BETWEEN_TRANSITIONS;
            timerBetweenTransitions.Timeout += EndTransitionToWin;
			timer.Timeout += StartTransitionToWin;
			AddChild(timer);
			AddChild(timerBetweenTransitions);
			base._Ready();
			JustWon();

		}

        public void JustWon()
		{
			Visible = true;
			timer.Start();
		}
		
		private void StartTransitionToWin()
		{
			for (int i = downMarkers.Count - 1; i >= 0; i--)
			{

				Tween lTween = banderoles[i].CreateTween().SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
				lTween.TweenProperty(banderoles[i], TweenProp.POSITION, downMarkers[i].GlobalPosition, 0.5f).SetDelay(0.4f);
				
            }

			if (banderoles[indexOfBanderoles].GlobalPosition == downMarkers[indexOfBanderoles].GlobalPosition)
			{
				timerBetweenTransitions.Start();
				timer.Stop();
			}

        }

		private void EndTransitionToWin()
		{
            for (int i = upMarkers.Count - 1; i >= 0; i--)
            {
                Tween lTween = banderoles[i].CreateTween().SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
                lTween.TweenProperty(banderoles[i], TweenProp.POSITION, upMarkers[i].GlobalPosition, 0.5f);

            }
        }

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
