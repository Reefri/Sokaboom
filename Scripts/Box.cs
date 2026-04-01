using Godot;
using System;
using static Godot.TextServer;


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

		public override void _Ready()
		{
			animPlaying = true;
            anim.Play(animToPlay);
            moveDust.Emitting = true;
            anim.AnimationFinished += EndOfAnimation;
        }

        private void EndOfAnimation(StringName pAnimName)
        {
			GameManager.GetInstance().tileMap.SetCell((int)Map.LevelLayer.Playground, (Vector2I)(Position / States.DISTANCE_RANGE) + Player.GetInstance().lastDirection, 0, GameManager.GetInstance().objectPositionOnTileSet[ObjectChar.BOX]);
            animPlaying = false;

            GetParent().RemoveChild(this);

            QueueFree();
        }

        public static bool CanBoxBePushed(Vector2I pDirection, Vector2I pCellPosition)
		{
			hasABoxToCheck = false;
			if (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, pCellPosition + pDirection) == null)
			{
				//Not the origin of the ghost box problem
				hasABoxToCheck = true;
                return true;
			}

			
			else if ( (bool)GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, pCellPosition + pDirection).GetCustomData(Map.INTERACTABLE)) { 
				return false;
			}

			else
			{
                GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Playground, pCellPosition);
				//A box will be instantiated in this situation
				hasABoxToCheck = true;
                return true;
			}
		}

		public static Box Create(Vector2I pPosition, Vector2I pDirection)
		{
            GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Playground, pPosition);
            Box lBox = (Box)packedBox.Instantiate();
            BoxAnimation(pDirection);
            lBox.GlobalPosition = (pPosition) * (States.DISTANCE_RANGE);
			GameManager.GetInstance().tileMap.AddChild(lBox);
			return lBox;

		}

		public static void BoxAnimation(Vector2I pDirection)
		{
			foreach (Vector2I lVector in Player.GetInstance().nameOfAnimation.Keys)
			{
				if (pDirection == lVector) animToPlay = Player.GetInstance().nameOfAnimation[lVector];

            }
        }

		public override void _Process(double delta)
		{
			if (anim.IsPlaying()) animPlaying = true;
			else { animPlaying = false; }

		}
	}
}
