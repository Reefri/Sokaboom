using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban 
{
	public partial class Map : TileMap
	{
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Level.tscn");
        private AStarGrid2D aStarGrid = new AStarGrid2D();
        public Array<Vector2I> cells ;
		private Array<Vector2I> groundCells ;


		public static string CONTAINER = "Container";
		public static string WALL = "Wall";
		public static string INTERACTABLE = "Interactable";
		private const string PATH_FINDING_INPUT = "leftClick";

		private static int mapCounter = 0;

		private int myInt;

		public Map()
		{
			myInt = mapCounter;
			mapCounter++;
		}

		public override void _Ready()
		{
			base._Ready();
			cells = GetUsedCells(1);
			groundCells = GetUsedCells(0);


			aStarGrid.Region = new Rect2I(-1,-1,50,50);
			aStarGrid.CellSize = new Vector2I(States.DISTANCE_RANGE, States.DISTANCE_RANGE);
			aStarGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
			aStarGrid.Update();


			foreach(Vector2I cell in cells)
			{
				if ((bool)(GetCellTileData(1, cell).GetCustomData(WALL)) || (bool)(GetCellTileData(1,cell).GetCustomData(CONTAINER))) aStarGrid.SetPointSolid(cell);
			}


        }

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;

			if (Input.IsActionJustPressed(PATH_FINDING_INPUT))
			{
				Vector2 lCellClicked =  new Vector2I((int)GetGlobalMousePosition().X/States.DISTANCE_RANGE, (int)GetGlobalMousePosition().Y/States.DISTANCE_RANGE);
				foreach(Vector2I cell in groundCells)
				{

					if (lCellClicked.DistanceTo(cell ) < 1)
					{
						if ((bool)(GetCellTileData(1, cell) == null || !(bool)(GetCellTileData(1, cell).GetCustomData(INTERACTABLE))))
						{
                            GD.Print(cell);
							CreatePathFinding((Vector2I)Player.GetInstance().Position/States.DISTANCE_RANGE, cell);
                            return;
                        }
                    
						else if ((bool)(GetCellTileData(1, cell).GetCustomData(WALL)) || (bool)(GetCellTileData(1, cell).GetCustomData(CONTAINER)))
						{
							return;
						}
					}

				}
			}
		}

		private void CreatePathFinding(Vector2I pBeginning, Vector2I pDestination)
		{
            List<Vector2I> lPlayersPath = new List<Vector2I>();
            Array<Vector2I> lPath = aStarGrid.GetIdPath(pBeginning, pDestination);
            foreach (Vector2I cellOnPath in lPath)
            {
				Player.GetInstance().path.Add(cellOnPath);
            }
			//Player.GetInstance().MovingOnPath(lPlayersPath);
        }

	}
}
