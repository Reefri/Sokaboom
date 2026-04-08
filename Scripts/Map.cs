using Godot;
using Godot.Collections;
using System;
using SysDict = System.Collections.Generic;
using System.IO;
using System.Data;
using System.Collections.Generic;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban 
{
	public partial class Map : TileMap
	{
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Level.tscn");
        private AStarGrid2D aStarGrid = new AStarGrid2D();
        public Array<Vector2I> cells ;
		private Array<Vector2I> groundCells ;


        public const int DISTANCE_RANGE = 64;
        public const int HALF_RANGE = 32;


        public const string PLAY_OBJECT = "PlayObject";
		public const string BOX = "Container";
		public const string WALL = "Wall";
		public const string INTERACTABLE = "Interactable";
		public const string TARGET = "Target";
		public const string BORDER = "Border";
		public const string GROUND = "Ground";

        public const string CLICK_INPUT = "leftClick";

        public static Vector2I boxOrWallClickedOn;
		public static Vector2I lastDirectionBeforePushing;
		public enum LevelLayer
		{
			Ground = 0,
			Target = 1,
			Playground = 2,
		};


        public static SysDict.Dictionary<string, ObjectChar> interactableToObjectChar = new SysDict.Dictionary<string, ObjectChar>
		{
			{ BOX,ObjectChar.BOX},
			{ WALL,ObjectChar.WALL},
			{ TARGET,ObjectChar.TARGET},
			{ BORDER,ObjectChar.BORDER},
		};



        public static Map Create()
		{
			return (Map)factory.Instantiate();
		}

		public override void _Ready()
		{
			cells = GetUsedCells((int)LevelLayer.Playground);
			groundCells = GetUsedCells((int)LevelLayer.Ground);

			aStarGrid = new AStarGrid2D();
			aStarGrid.Region = new Rect2I(-1,-1,20,20);
			aStarGrid.CellSize = new Vector2I(DISTANCE_RANGE, DISTANCE_RANGE);
			aStarGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
			aStarGrid.Update();

        }

		public override void _Process(double pDelta)
		{

            if (!Player.GetInstance().canInput || Player.GetInstance().hasBoxToPush || Box.animPlaying) return;

            else if (Input.IsActionJustPressed(CLICK_INPUT))
			{
                GameManager.GetInstance().EmptyBoxSignalContainer();

				SoundManager.GetInstance().PlayStartPathFindind();
		
				Player.GetInstance().path.Clear();

                UpdateAndClearPath();

				Vector2 lCellClicked = new Vector2I((int)((GetGlobalMousePosition().X + HALF_RANGE) / DISTANCE_RANGE), (int)((GetGlobalMousePosition().Y + HALF_RANGE) / DISTANCE_RANGE)) ;

				if (GetCellTileData((int)LevelLayer.Ground, (Vector2I)lCellClicked) == null) return;
				OnClick.Create(lCellClicked, GameManager.GetInstance());

				foreach(Vector2I lCell in groundCells)
				{
                    if (lCellClicked.DistanceTo(lCell ) < 1)
					{
						if (lCell == Player.GetInstance().GetPositionToVector2I()) { return; }

						if (GetCellTileData((int)LevelLayer.Playground, lCell) == null )
						{
                            CreatePathFinding(Player.GetInstance().GetPositionToVector2I(), lCell);
                        }
                    
						else if ((bool)(GetCellTileData((int)LevelLayer.Playground, lCell).GetCustomData(WALL)))
						{
							boxOrWallClickedOn = lCell;
                            WallOrBoxChosen(WALL, lCell);
						}

                        else if ((bool)(GetCellTileData((int)LevelLayer.Playground, lCell).GetCustomData(BOX)))
						{
                            boxOrWallClickedOn = lCell;
							WallOrBoxChosen(BOX, lCell);
						}
                    }

				}
			}
		}


		private void UpdateAndClearPath()
		{
            groundCells = GetUsedCells((int)LevelLayer.Ground);
            cells = GetUsedCells((int)LevelLayer.Playground);
            aStarGrid.Update();

            foreach (Vector2I cell in cells)
			{
                if ((bool)GetCellTileData((int)LevelLayer.Playground, cell).GetCustomData(INTERACTABLE))aStarGrid.SetPointSolid(cell);
            }
		}


		private void WallOrBoxChosen(string pWallOrBox, Vector2I pCell)
		{
			List<Vector2I> lAlternativeCells = new List<Vector2I>();
			List<float> lDistanceBetweenCells = new List<float>();

			int indexOfClosestCell;

			foreach (Vector2I lVector in Player.GetInstance().nameOfAnimation.Keys)
			{
				Vector2I lPossibleCell = pCell + lVector;
				UpdateAndClearPath();


				if ((GetCellTileData((int)LevelLayer.Playground, lPossibleCell) == null) && 
					aStarGrid.GetIdPath(Player.GetInstance().GetPositionToVector2I(), lPossibleCell).Count != 0)
				{

					float lClosestCell = Player.GetInstance().GlobalPosition.DistanceTo(lPossibleCell * DISTANCE_RANGE);

					lAlternativeCells.Add(lPossibleCell);
					lDistanceBetweenCells.Add(lClosestCell);
				}
			}

            if (lAlternativeCells.Count == 0)
            {
                if (Player.GetInstance().bombInHand == null || boxOrWallClickedOn == Vector2I.Zero)
                {
                    Player.GetInstance().animPlayer.Play(Player.ANIM_BLOCKED);
					Player.GetInstance().animatedSprite.GlobalPosition = Player.GetInstance().GlobalPosition;
                    return;
                }
                return; 
			}



            float lTheClosestCell = lDistanceBetweenCells[0];
			indexOfClosestCell = 0;


			for (int i = lDistanceBetweenCells.Count - 1; i > 0; i--)
			{
				if (lTheClosestCell > lDistanceBetweenCells[i])
				{
					lTheClosestCell = lDistanceBetweenCells[i];
					indexOfClosestCell = i;
				}

			}

			
			Player.GetInstance().hasBoxToPush = (pWallOrBox == BOX);
				
            CreatePathFinding(Player.GetInstance().GetPositionToVector2I(), lAlternativeCells[indexOfClosestCell]);
		}



		private void CreatePathFinding(Vector2I pBeginning, Vector2I pDestination)
		{

            Array<Vector2I> lPath = aStarGrid.GetIdPath(pBeginning, pDestination);

            if (Player.GetInstance().hasBoxToPush && (boxOrWallClickedOn - pBeginning).LengthSquared() <= 1)
            {

				Player.GetInstance().AdjacentToInteractable(boxOrWallClickedOn - pBeginning);

                Player.GetInstance().hasBoxToPush = false;
                return;
                

            }

            if (lPath.Count == 0 && (Player.GetInstance().bombInHand == null || boxOrWallClickedOn == Vector2I.Zero))
            {
                Player.GetInstance().animPlayer.Play(Player.ANIM_BLOCKED);
                Player.GetInstance().animatedSprite.GlobalPosition = Player.GetInstance().GlobalPosition;
                return;
            }



            foreach (Vector2I cellOnPath in lPath)
			{
				Player.GetInstance().path.Add(cellOnPath);
			}

		}
	
	}
}
