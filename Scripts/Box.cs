using Com.IsartDigital.Utils.Tweens;
using Godot;


// author : Cayot Daniel

namespace Com.IsartDigital.Sokoban
{
	public partial class Box : Node2D
	{
		[Export] private AnimationPlayer anim;
		[Export] private GpuParticles2D moveDust;

		private static PackedScene packedBox = (PackedScene)ResourceLoader.Load("res://Scenes/Gameplay/Box.tscn");

		public static bool animPlaying = false;
		private static string animToPlay;
		public static bool hasABoxToCheck = false;

		private bool startAnimation;

		public override void _Ready()
		{
			if (!startAnimation)
			{
                SoundManager.GetInstance().PlayBoxMove();
                animPlaying = true;
                anim.Play(animToPlay);
				moveDust.Emitting = true;
				anim.AnimationFinished += EndOfAnimationWalk;
			}
        }

        private void EndOfAnimationWalk(StringName pAnimName)
        {
			Vector2I lFinalPos = (Vector2I)(Position / Map.DISTANCE_RANGE) + Player.GetInstance().lastDirection;

            GameManager.GetInstance().tileMap.SetCell((int)Map.LevelLayer.Playground, lFinalPos, 0, GameManager.GetInstance().objectPositionOnTileSet[ObjectChar.BOX]);

            if (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Target, lFinalPos) != null)
				SoundManager.GetInstance().PlayBoxValid();


            animPlaying = false;

			GameManager.GetInstance().UpdateAfterAction();
            QueueFree();
        }

        private void EndOfStartAnimation()
        {
			Vector2I lFinalPos = (Vector2I)(Position / Map.DISTANCE_RANGE);

            GameManager.GetInstance().tileMap.SetCell((int)Map.LevelLayer.Playground, lFinalPos, 0, GameManager.GetInstance().objectPositionOnTileSet[ObjectChar.BOX]);

            QueueFree();
        }

        public static bool CanBoxBePushed(Vector2I pDirection, Vector2I pCellPosition)
        {
            hasABoxToCheck = (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, pCellPosition + pDirection) == null);
          
			return hasABoxToCheck;
        }

        public static Box Create(Vector2I pPosition, Vector2I pDirection)
		{
            GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Playground, pPosition);
            Box lBox = (Box)packedBox.Instantiate();
			BoxAnimation(pDirection);
            lBox.GlobalPosition = pPosition * (Map.DISTANCE_RANGE);
			GameManager.GetInstance().tileMap.AddChild(lBox);

            return lBox;
		}

        public static Box CreateAnimation(Vector2I pPosition, Tween pTween)
        {
            GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Playground, pPosition);
            Box lBox = (Box)packedBox.Instantiate();
            lBox.startAnimation = true;
            lBox.GlobalPosition = pPosition * (Map.DISTANCE_RANGE);

            StartAnimation(lBox, pTween);

            GameManager.GetInstance().tileMap.AddChild(lBox);
            return lBox;
        }

        public static void StartAnimation(Box pBox, Tween pTween)
        {
            pTween.TweenProperty(pBox, TweenProp.ROTATION, Mathf.DegToRad(360), 1f);
            pTween.Finished += pBox.EndOfStartAnimation;
        }

        public static void BoxAnimation(Vector2I pDirection)
		{
			animToPlay = Player.GetInstance().nameOfAnimation[pDirection];
        }

		public override void _Process(double delta)
		{
			if (anim.IsPlaying()) animPlaying = true;
			else { animPlaying = false; }
		}
	}
}
