using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class TitleDoors : Control
	{
        static private TitleDoors instance;
        private static PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/Transitions/TitleDoors.tscn");
		
        [Export] private TextureRect leftDoor;
		[Export] private TextureRect rightDoor;

        [Export] private ColorRect blockingMouse;

        [Export] private float tweenDuration = 1;

        [Export] public Timer whenToPlayAnim;
        [Export] private Timer goToLevel;
        [Export] private Timer doorsStillTimer;

		private float margin;
        private float sideFactor;

		private Vector2 screenSize;

        private bool doorsClosed = false;

        public bool goingToLevel = false;

        public bool animationFinished = false;

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
            sideFactor = screenSize.X / 25;
            whenToPlayAnim.WaitTime = tweenDuration + doorsStillTimer.WaitTime;
            whenToPlayAnim.Timeout += UIManager.GetInstance().ContinueToLevelSelect;
            goToLevel.WaitTime = whenToPlayAnim.WaitTime;
            goToLevel.Timeout += UIManager.GetInstance().ContinueToLevel;

            SetDoorsClosed();
            OpenDoors();

            doorsStillTimer.Timeout += ContinueDoorsMovement;
        }



		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

            screenSize = GetViewportRect().Size;
            if (GameManager.GetInstance().currentLevel != null)
            {

                leftDoor.Position = new Vector2(-screenSize.X / 2, 0);
                rightDoor.Position = new Vector2(screenSize.X * 1.5f, 0);
            }
            else if(animationFinished) SetDoorsOpen();
            if (animationFinished)
            {
                blockingMouse.MouseFilter = MouseFilterEnum.Ignore;
            }
            else blockingMouse.MouseFilter = MouseFilterEnum.Stop;

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

            Tween lDoorsTween = CreateTween()
                .SetTrans(Tween.TransitionType.Bounce)
                       .SetEase(Tween.EaseType.Out);

            ClosingDoorsTweens(lDoorsTween);

            lDoorsTween.SetTrans(Tween.TransitionType.Quad)
                        .SetEase(Tween.EaseType.OutIn);

            OpeningDoorsTween(lDoorsTween);

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
            if (doorsClosed) 
            {
                OpeningDoorsTween();
            }
            else 
            {
                ClosingDoorsTweens();
            }
        }

        private void ClosingDoorsTweens(Tween pTween = null)
        {

            if (pTween == null)
            {
                pTween = CreateTween()
                       .SetTrans(Tween.TransitionType.Bounce)
                       .SetEase(Tween.EaseType.Out);
            }
                
                pTween.TweenCallback
                    (
                    Callable.From(() =>
                        doorsClosed = true)
                    );
            animationFinished = false;

            pTween.TweenProperty(leftDoor, TweenProp.POSITION,
                new Vector2(screenSize.X / 2, 0), tweenDuration);

            pTween.Parallel().TweenProperty(rightDoor, TweenProp.POSITION,
                new Vector2(screenSize.X / 2, 0), tweenDuration);
        }

        private void OpeningDoorsTween(Tween pTween = null)
        {

            if (pTween == null)
            {
                pTween = CreateTween()
                        .SetTrans(Tween.TransitionType.Quad)
                        .SetEase(Tween.EaseType.OutIn);
            }

            pTween.TweenProperty(leftDoor, TweenProp.POSITION,
                new Vector2(sideFactor, 0), tweenDuration).SetDelay(doorsStillTimer.WaitTime);

            pTween.Parallel().TweenProperty(rightDoor, TweenProp.POSITION,
               new Vector2(screenSize.X - sideFactor, 0), tweenDuration).SetDelay(doorsStillTimer.WaitTime);

            pTween.TweenCallback
                (
                Callable.From(() =>
                    doorsClosed = false)
                );

            if (goingToLevel)
            {
                pTween.TweenProperty(rightDoor, TweenProp.POSITION, new Vector2(screenSize.X, 0), tweenDuration);
                pTween.Parallel().TweenProperty(leftDoor, TweenProp.POSITION, new Vector2(0, 0), tweenDuration);
            }

            pTween.TweenCallback
                (
                Callable.From(() =>
                    animationFinished = true)
                );
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
            leftDoor.Position = new Vector2(screenSize.X / 2, 0);
            rightDoor.Position = new Vector2(screenSize.X / 2, 0);
        }
        private void SetDoorsOpen()
        {
            doorsClosed = false;
            leftDoor.Position = new Vector2(sideFactor, 0);
            rightDoor.Position = new Vector2(screenSize.X - sideFactor, 0);
        }

	}
}
