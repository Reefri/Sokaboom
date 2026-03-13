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

		private static PackedScene packedBox = (PackedScene)ResourceLoader.Load("res://Scenes/Box.tscn");

		private const string GO_UP_ANIM = "goUp";
		private const string GO_DOWN_ANIM = "goDown";
		private const string GO_RIGHT_ANIM = "goRight";
		private const string GO_LEFT_ANIM = "goLeft";


		public static bool animPlaying = false;
		private static string animToPlay;
		private static Vector2 movingTheBox = new Vector2(States.DISTANCE_RANGE,States.DISTANCE_RANGE);
		public override void _Ready()
		{
			animPlaying = true;
            anim.Play(animToPlay);
            moveDust.Emitting = true;
            anim.AnimationFinished += EndOfAnimation;
        }

        private void EndOfAnimation(StringName pAnimName)
        {
            GameManager.GetInstance().tileMap.SetCell( 1, ((Vector2I)Position + Player.lastDirection)/States.DISTANCE_RANGE, 0, GameManager.GetInstance().objectPositionOnTileSet[ObjectChar.BOX]);
            animPlaying = false;

            GetParent().RemoveChild(this);

            QueueFree();
			
            GameManager.GetInstance().SaveScreenshotGame();

        }

        public static bool CanBoxBePushed(Vector2I pDirection, Vector2I pCellPosition)
		{
			if (GameManager.GetInstance().tileMap.GetCellTileData( 1, pCellPosition + pDirection) == null )
			{
                GameManager.GetInstance().tileMap.SetCell( 1, pCellPosition, -1);
                Create(pCellPosition , pDirection);
                return false;
			}

			
			else if ( (bool)GameManager.GetInstance().tileMap.GetCellTileData(1, pCellPosition + pDirection).GetCustomData("Container") ||
				(bool)GameManager.GetInstance().tileMap.GetCellTileData(1, pCellPosition + pDirection).GetCustomData("Wall") )
			{
				return true;
			}
			else
			{
                GameManager.GetInstance().tileMap.SetCell(1, pCellPosition, -1);
                Create(pCellPosition, pDirection);
                return false;
			}
		}

		public static Box Create(Vector2I pPosition, Vector2I pDirection)
		{
			Box lBox = (Box)packedBox.Instantiate();
            BoxAnimation(pDirection);
            lBox.Position =  (pPosition + Vector2.One/2) * (States.DISTANCE_RANGE);
			GameManager.GetInstance().tileMap.AddChild(lBox);
			return lBox;

		}

		public static void BoxAnimation(Vector2I pDirection)
		{
			if(pDirection == Vector2I.Up) animToPlay = GO_UP_ANIM;
            else if (pDirection == Vector2I.Down) animToPlay = GO_DOWN_ANIM;
			else if(pDirection == Vector2I.Right) animToPlay = GO_RIGHT_ANIM;
            else if (pDirection == Vector2I.Left) animToPlay = GO_LEFT_ANIM;
        }

		public override void _Process(double delta)
		{
			if(anim.IsPlaying()) animPlaying = true;
			else { animPlaying = false; }

		}
	}
}
