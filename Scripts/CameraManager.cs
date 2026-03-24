using Com.IsartDigital.Utils.Tweens;
using Godot;
using System;
using static Godot.CameraFeed;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class CameraManager : Node2D
	{
		static private CameraManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/CameraManager.tscn");

		[Export] public Camera2D camera;
		[Export] private Timer moveCameraTimer;
		[Export] private Timer moveBackToStartTimer;
		[Export] private Timer waitTimer;

		private Vector2 startPosition;

		private bool isMoving = false;

		private CameraManager():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(CameraManager) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public CameraManager GetInstance()
		{
			if (instance == null) instance = (CameraManager)factory.Instantiate();
			return instance;
		}

		public override void _Ready()
		{
			base._Ready();

            moveCameraTimer.Timeout += WaitAtEndPos;
            moveBackToStartTimer.Timeout += StopMoving;
            waitTimer.Timeout += moveBackCameraToStart;
		}

        private void WaitAtEndPos()
        {
            waitTimer.Start();
        }

        private void StopMoving()
        {
            isMoving = false ;
        }

        private void moveBackCameraToStart()
        {
			moveBackToStartTimer.Start();

            Tween lTween = CreateTween()
                .SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.InOut);
            lTween.TweenProperty(camera, TweenProp.GLOBAL_POSITION, startPosition, (double)moveBackToStartTimer.WaitTime);
        }

        public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;

			if (Input.IsActionJustPressed("leftClick"))
			{
				MoveCameraFromAToB(camera.GlobalPosition, GetGlobalMousePosition());
			}
		}

		

		public void MoveCameraFromAToB(Vector2 pStartPosition, Vector2 pEndPosition, float pTime = 1)
		{
			

			if (!isMoving && GameManager.GetInstance().currentLevel != null)
			{
				isMoving = true;

                startPosition = pStartPosition;

                moveCameraTimer.WaitTime = pTime;
				moveCameraTimer.Start();

				Tween lTween = CreateTween()
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut);
				lTween.TweenProperty(camera, TweenProp.GLOBAL_POSITION, pEndPosition, (double)pTime);
			}
		}

        public void CenterCameraOnCurrentLevel()
        {
            if (GameManager.GetInstance().currentLevel != null)
            {
                camera.GlobalPosition = GameManager.GetInstance().currentLevel.Size * States.DISTANCE_RANGE / 2;
            }

        }
        protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
