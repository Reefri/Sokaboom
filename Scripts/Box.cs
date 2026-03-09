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

		private const int ZERO = 0;
		private const int ONE = 1;

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
            Map.GetInstance().SetCell( ONE, (Vector2I)Position/States.DISTANCE_RANGE+ Player.lastDirection/States.DISTANCE_RANGE, ZERO, new Vector2I(ONE, ZERO) );
            animPlaying = false;
            QueueFree();
        }

        public static bool CanBoxBePushed(Vector2I pDirection, Vector2I pCellPosition)
		{
			if (Map.GetInstance().GetCellTileData( ONE, pCellPosition + pDirection) == null )
			{
                Map.GetInstance().SetCell( ONE, pCellPosition, ZERO, new Vector2I(ZERO, ZERO) );
                Create(pCellPosition , pDirection);
                //Map.GetInstance().SetCell(1, pCellPosition + pDirection, 0, new Vector2I(1, 0));
                return false;
			}

			
			else if ( (bool)Map.GetInstance().GetCellTileData(1, pCellPosition + pDirection).GetCustomData("Container") ||
				(bool)Map.GetInstance().GetCellTileData(1, pCellPosition + pDirection).GetCustomData("Wall") )
			{
				return true;
			}
			else
			{
                Map.GetInstance().SetCell(1, pCellPosition, ZERO, new Vector2I(ZERO, ZERO));
                Create(pCellPosition, pDirection);
                return false;
			}
		}

		public static Box Create(Vector2I pPosition, Vector2I pDirection)
		{
			Box lBox = (Box)packedBox.Instantiate();
            BoxAnimation(pDirection);
            lBox.Position =  Player.GetInstance().Position + pDirection * movingTheBox;
			Map.GetInstance().AddChild(lBox);
			return lBox;

		}

		public static void BoxAnimation(Vector2I pDirection)
		{
			if(pDirection * States.DISTANCE_RANGE == Player.up) animToPlay = GO_UP_ANIM;

            else if (pDirection * States.DISTANCE_RANGE == Player.down) animToPlay = GO_DOWN_ANIM;
			else if(pDirection * States.DISTANCE_RANGE == Player.right) animToPlay = GO_RIGHT_ANIM;
            else if (pDirection * States.DISTANCE_RANGE == Player.left) animToPlay = GO_LEFT_ANIM;
        }

		public override void _Process(double delta)
		{
			if(anim.IsPlaying()) animPlaying = true;
			else { animPlaying = false; }

		}
	}
}
