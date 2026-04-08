using Com.IsartDigital.Utils.Tweens;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban 
{
	public partial class Banderole : Control
	{
		static private Banderole instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/Transitions/BanderoleTransition.tscn");


		[Export] private Control banderoleNode;

		[Export] private Array<Marker2D> downMarkers;
		[Export] private Array<Marker2D> upMarkers;

		private int indexOfBanderoles = 4;

		private Timer timer = new Timer();

		private Timer timerBetweenTransitions = new Timer();
		private const float TIME_BETWEEN_TRANSITIONS = 0.2f;

		private const float FIRST_DURATION = 0.509f;
		private const float LAST_DURATION = 0.5f;

		private List<TextureRect> banderoles ;

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
			banderoles = banderoleNode.GetChildren().OfType<TextureRect>().ToList();
			Visible = false;
		}

		
		public void StartTransitionToWin()
		{
			Visible = true;
			Tween lTween = CreateTween().SetParallel().SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);

			for (int i = downMarkers.Count - 1; i >= 0; i--)
            {
				lTween.TweenProperty(banderoleNode.GetChild(i), TweenProp.POSITION, downMarkers[i].GlobalPosition, FIRST_DURATION).SetDelay(i * TIME_BETWEEN_TRANSITIONS);
            }

            lTween.Finished += EndTransitionToWin;

        }


        private void EndTransitionToWin()
		{
			UIManager.GetInstance().GoToWin();
            Tween lTween = CreateTween().SetParallel().SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
            for (int i = upMarkers.Count - 1; i >= 0; i--)
            {

                lTween.TweenProperty(banderoles[i], TweenProp.POSITION, upMarkers[i].GlobalPosition, LAST_DURATION);

            }
			lTween.Finished += HideBanderoles;
        }

		private void HideBanderoles()
		{
			Visible = false;
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
