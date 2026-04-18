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

		private const float TIME_BETWEEN_TRANSITIONS = 0.2f;

		private const float FIRST_DURATION = 0.509f;
		private const float LAST_DURATION = 0.5f;

		private List<TextureRect> banderoles;
        private List<Vector2> banderolesOriginPos = new List<Vector2>();

        public bool winFinal;

		int lIndex;

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

            lIndex = banderoles.Count;

			VisibilityChanged += VerifyPos;

            for (int i = 0; i < lIndex; i++)
            {
				banderolesOriginPos.Add(banderoles[i].Position);
            }
        }

        private void VerifyPos()
        {
            for (int i = 0; i < lIndex; i++)
            {
				banderolesOriginPos[i] = banderoles[i].Position;
            }
        }

        public void StartTransitionToWin()
		{
			Visible = true;

            SoundManager.GetInstance().PlayRuban();

            Tween lTween = CreateTween().SetParallel().SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);

            for (int i = 0; i < lIndex; i++)
            {
                lTween.TweenProperty(banderoleNode.GetChild(i), TweenProp.POSITION_X, 0, FIRST_DURATION).SetDelay(i * TIME_BETWEEN_TRANSITIONS);
            }

            lTween.Finished += EndTransitionToWin;
        }


        private void EndTransitionToWin()
		{
            if (!winFinal)UIManager.GetInstance().GoToWin();
            else UIManager.GetInstance().GoToWinFinal();

            Tween lTween = CreateTween().SetParallel().SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);

            for (int i = 0; i < lIndex; i++)
            {
                lTween.TweenProperty(banderoles[i], TweenProp.POSITION, banderolesOriginPos[i], LAST_DURATION);
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
