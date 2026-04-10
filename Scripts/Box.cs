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

		private static PackedScene packedBox = (PackedScene)ResourceLoader.Load("res://Scenes/Gameplay/Box.tscn");

		public static bool animPlaying = false;
		private static string animToPlay;
		public static bool hasABoxToCheck = false;



		private static List<Box> instances = new List<Box>();


		public static Texture2D texture ;

		public static void UpdateTexture()
		{


			foreach (Box lBox in instances)
			{
                GD.Print("ha");

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

			SoundManager.GetInstance().PlayBoxMove();
			animPlaying = true;
            anim.Play(animToPlay);
            moveDust.Emitting = true;
            anim.AnimationFinished += EndOfAnimation;
        }

        private void EndOfAnimation(StringName pAnimName)
        {
			Vector2I lFinalPos = (Vector2I)(Position / Map.DISTANCE_RANGE) + Player.GetInstance().lastDirection;


            GameManager.GetInstance().tileMap.SetCell((int)Map.LevelLayer.Playground, lFinalPos, 0, GameManager.GetInstance().objectPositionOnTileSet[ObjectChar.BOX]);

			if (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Target, lFinalPos) != null)
				SoundManager.GetInstance().PlayBoxValid();


            animPlaying = false;

			GameManager.GetInstance().UpdateAfterAction();
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
