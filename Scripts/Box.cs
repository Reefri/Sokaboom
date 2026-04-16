using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Collections.Generic;


// author : Cayot Daniel

namespace Com.IsartDigital.Sokoban
{
	public partial class Box : Node2D
	{
		[Export] private AnimationPlayer anim;
		[Export] private GpuParticles2D moveDust;
		[Export] private Sprite2D renderer;

        [Export] float timeOfFall = 0.75f;

		private static PackedScene packedBox = (PackedScene)ResourceLoader.Load("res://Scenes/Gameplay/Box.tscn");

		public static bool animPlaying = false;
		private static string animToPlay;
		public static bool hasABoxToCheck = false;

        private bool startAnimation;

		private static List<Box> instances = new List<Box>();


		public static Texture2D texture ;

		public static void UpdateTexture()
		{


			foreach (Box lBox in instances)
			{
                lBox.renderer.Texture = texture;
			}
		}


		protected override void Dispose(bool pDisposing)
		{
			instances.Remove(this);
		}
		

		public override void _Ready()
		{
			renderer.Texture = texture;
			
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

            Player.GetInstance().CreatePrevisualisation();

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

			instances.Add(lBox);

			GameManager.GetInstance().tileMap.AddChild(lBox);

            return lBox;
		}

        public static Box CreateAnimation(Vector2I pPosition, Tween pTween, float pInterval)
        {
            GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Playground, pPosition);
            Box lBox = (Box)packedBox.Instantiate();
            lBox.startAnimation = true;
            lBox.GlobalPosition = pPosition * (Map.DISTANCE_RANGE);

            lBox.StartAnimation(pTween, pInterval);

            GameManager.GetInstance().tileMap.AddChild(lBox);
            return lBox;
        }

        private void StartAnimation(Tween pTween, float pInterval)
        {
            pTween.TweenProperty(this, TweenProp.POSITION_Y, -Position.Y, 0);
            pTween.TweenProperty(this, TweenProp.POSITION_Y, Position.Y, timeOfFall).SetDelay(pInterval);
            pTween.TweenProperty(this, TweenProp.SCALE, new Vector2(0.5f, 1.5f), timeOfFall).SetDelay(pInterval);


            pTween.TweenCallback(Callable.From(Dust)).SetDelay(pInterval + timeOfFall);

            pTween.TweenProperty(this, TweenProp.SCALE, new Vector2(1.5f, 0.5f), 0.1f).SetDelay(pInterval + timeOfFall);
            pTween.TweenProperty(this, TweenProp.POSITION_Y, Position.Y + 16, 0.1f).SetDelay(pInterval + timeOfFall);

            pTween.TweenProperty(this, TweenProp.SCALE, new Vector2(1,1), 0.1f).SetDelay(pInterval + timeOfFall + 0.1f);
            pTween.TweenProperty(this, TweenProp.POSITION_Y, Position.Y, 0.1f).SetDelay(pInterval + timeOfFall + 0.1f);

            pTween.TweenInterval(pInterval + timeOfFall + 1f).Finished += EndOfStartAnimation;
        }

        public static void BoxAnimation(Vector2I pDirection)
		{
			animToPlay = Player.GetInstance().nameOfAnimation[pDirection];
        }

        private void Dust()
        {
            moveDust.Emitting = true;
        }

		public override void _Process(double delta)
		{
			if (anim.IsPlaying()) animPlaying = true;
			else { animPlaying = false; }
		}
	}
}
