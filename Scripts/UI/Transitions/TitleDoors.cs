using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class TitleDoors : Control
	{
        static private TitleDoors instance;
        private static PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/Transitions/TitleDoors.tscn");
		
        [Export] private Node2D leftDoor;
		[Export] private Node2D rightDoor;

        [Export] private float tweenDuration = 1;

        [Export] public Timer whenToPlayAnim;
        [Export] private Timer goToLevel;
        [Export] private Timer doorsStillTimer;

		private float margin;
        private float sideFactor;

		private Vector2 screenSize;

        private bool doorsClosed = false;

        public bool goingToLevel = false;

        private TitleDoors() : base()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(TitleDoors) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }

        static public TitleDoors GetInstance()
        {
            if (instance == null) instance = (TitleDoors)factory.Instantiate();
            return instance;
        }
        public override void _Ready()
		{
            screenSize = GetViewportRect().Size;
            margin = screenSize.X / 2;
            sideFactor = screenSize.X / 1.4f;
            whenToPlayAnim.WaitTime = tweenDuration + doorsStillTimer.WaitTime;
            whenToPlayAnim.Timeout += UIManager.GetInstance().ContinueToLevelSelect;
            goToLevel.WaitTime = whenToPlayAnim.WaitTime;
            goToLevel.Timeout += UIManager.GetInstance().ContinueToLevel;

            SetDoorsClosed();
            OpenDoors();

            //whenToPlayAnim.Timeout += UIManager.GetInstance().ContinueToLevelSelect;

            doorsStillTimer.Timeout += ContinueDoorsMovement;
        }



		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

            screenSize = GetViewportRect().Size;
            if (GameManager.GetInstance().currentLevel != null)
            {
                
                leftDoor.Position = screenSize / 2 + Vector2.Left * screenSize.X;
                rightDoor.Position = screenSize / 2 + Vector2.Right * screenSize.X;
            }
		}

        public void Transition()
        {
            if (!goingToLevel)
            {
                whenToPlayAnim.Start();
            }
            else
            {
                goToLevel.Start();
            }
           Tween lDoorsTween = leftDoor.CreateTween()
                   .SetTrans(Tween.TransitionType.Bounce)
                   .SetEase(Tween.EaseType.Out);

            lDoorsTween.TweenProperty(leftDoor, TweenProp.POSITION,
                new Vector2(screenSize.X / 4, screenSize.Y / 2), tweenDuration);

            lDoorsTween.Parallel().TweenProperty(rightDoor, TweenProp.POSITION,
                new Vector2(screenSize.X - screenSize.X / 4, screenSize.Y / 2), tweenDuration);

            lDoorsTween
                    .SetTrans(Tween.TransitionType.Quad)
                    .SetEase(Tween.EaseType.OutIn);

            lDoorsTween.TweenProperty(leftDoor, TweenProp.POSITION,
                screenSize / 2 + Vector2.Left * sideFactor, tweenDuration).SetDelay(doorsStillTimer.WaitTime);



            lDoorsTween.Parallel().TweenProperty(rightDoor, TweenProp.POSITION,
                screenSize / 2 + Vector2.Right * sideFactor, tweenDuration).SetDelay(doorsStillTimer.WaitTime);

            lDoorsTween.TweenCallback
                (
                Callable.From(() =>
                    doorsClosed = false)
                );
        }

        public void ActivateDoors()
        {
            if (!doorsClosed)
                CloseDoors();
            else
                OpenDoors();
        }

        private void OpenDoors()
        {
            if (doorsStillTimer.IsStopped() && doorsClosed)
            {
                doorsStillTimer.Start();
            }
                
        }
        private void ContinueDoorsMovement()
        {
            if (doorsClosed) //opens the doors
            {
                SetDoorsClosed();

                Tween lLeftDoorTween = leftDoor.CreateTween()
                    .SetTrans(Tween.TransitionType.Quad)
                    .SetEase(Tween.EaseType.OutIn);

                lLeftDoorTween.TweenProperty(leftDoor, TweenProp.POSITION,
                    screenSize / 2 + Vector2.Left * sideFactor, tweenDuration);

                Tween lRightDoorTween = rightDoor.CreateTween()
                    .SetTrans(Tween.TransitionType.Quad)
                    .SetEase(Tween.EaseType.OutIn);
                lRightDoorTween.TweenProperty(rightDoor, TweenProp.POSITION,
                    screenSize / 2 + Vector2.Right * sideFactor, tweenDuration);

                doorsClosed = false;
            }
            else if (!doorsClosed) //closes the doors
            {
                SetDoorsOpen();

                Tween lLeftDoorTween = leftDoor.CreateTween()
                    .SetTrans(Tween.TransitionType.Bounce)
                    .SetEase(Tween.EaseType.Out);
                lLeftDoorTween.TweenProperty(leftDoor, TweenProp.POSITION,
                    new Vector2(screenSize.X / 4, screenSize.Y / 2), tweenDuration);

                Tween lRightDoorTween = rightDoor.CreateTween()
                    .SetTrans(Tween.TransitionType.Bounce)
                    .SetEase(Tween.EaseType.Out);
                lRightDoorTween.TweenProperty(rightDoor, TweenProp.POSITION,
                    new Vector2(screenSize.X - screenSize.X / 4, screenSize.Y / 2), tweenDuration);

                doorsClosed = true;
            }
        }

        private void CloseDoors()
        {
            if (doorsStillTimer.IsStopped() && !doorsClosed)
            {
                doorsStillTimer.Start();
                doorsClosed = false;
            }
        }

        private void SetDoorsClosed()
        {
            doorsClosed = true;
            leftDoor.Position = new Vector2(screenSize.X / 4, screenSize.Y / 2);
            rightDoor.Position = new Vector2(screenSize.X - screenSize.X / 4, screenSize.Y / 2);
        }
        private void SetDoorsOpen()
        {
            doorsClosed = false;
            leftDoor.Position = screenSize / 2 + Vector2.Left * sideFactor;
            rightDoor.Position = screenSize / 2 + Vector2.Right * sideFactor;
        }
		protected override void Dispose(bool pDisposing)
		{

		}

	}
}
