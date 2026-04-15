using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class CameraManager : Node2D
	{
		static private CameraManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Manager/CameraManager.tscn");

		[Export] public Camera2D camera;
		[Export] private Timer moveCameraTimer;
		[Export] private Timer moveBackToStartTimer;
		[Export] private Timer waitTimer;
		[Export] private Timer heldInputTime;

		private const string MOVE_CAM_INPUT = "leftClick";

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

		public void Zoom(Vector2 pWhereToZoom, float pZoomStrength, float pTweenDuration, bool pGoBackToPos = false)
		{

			Tween lTween = camera.CreateTween()
				.SetTrans(Tween.TransitionType.Quint)
				.SetEase(Tween.EaseType.Out);

			lTween.TweenProperty(camera, TweenProp.ZOOM, Vector2.One * pZoomStrength, pTweenDuration);
			lTween.Parallel().TweenProperty(camera, TweenProp.POSITION, pWhereToZoom, pTweenDuration);

			lTween.TweenProperty(camera, TweenProp.ZOOM, Vector2.One, 0);
			lTween.TweenCallback(
					Callable.From(() =>
						CenterCameraOnCurrentLevel())
					);


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
		

        public void CenterCameraOnCurrentLevel()
        {
            if (GameManager.GetInstance().currentLevel != null)
            {
                camera.GlobalPosition = GameManager.GetInstance().currentLevel.Size * Map.DISTANCE_RANGE / 2;
            }
        }
        protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
