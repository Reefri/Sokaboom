using Godot;
using Godot.Collections;
using System;
using SysDict = System.Collections.Generic;
using System.IO;
using System.Data;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban 
{
	public partial class Map : TileMap
	{
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Level.tscn");
        private AStarGrid2D aStarGrid = new AStarGrid2D();
        public Array<Vector2I> cells ;
		private Array<Vector2I> groundCells ;

		public const string PLAY_OBJECT = "PlayObject";
		public const string CONTAINER = "Container";
		public const string WALL = "Wall";
		public const string INTERACTABLE = "Interactable";
		public const string TARGET = "Target";
		public const string BORDER = "Border";
		public const string GROUND = "Ground";
		
		public enum LevelLayer
		{
			Ground = 0,
			Target = 1,
			Playground = 2,
		};


        public static SysDict.Dictionary<string, ObjectChar> interactableToObjectChar = new SysDict.Dictionary<string, ObjectChar>
		{
			{ CONTAINER,ObjectChar.BOX},
			{ WALL,ObjectChar.WALL},
			{ TARGET,ObjectChar.TARGET},
			{ BORDER,ObjectChar.BORDER},
		};

        private const string PATH_FINDING_INPUT = "leftClick";

        private static int mapCounter = 0;


        public static Map Create()
		{
			return (Map)factory.Instantiate();
		}

		public override void _Ready()
		{
			base._Ready();
			cells = GetUsedCells((int)LevelLayer.Playground);
			groundCells = GetUsedCells((int)LevelLayer.Ground);


			aStarGrid.Region = new Rect2I(-1,-1,50,50);
			aStarGrid.CellSize = new Vector2I(States.DISTANCE_RANGE, States.DISTANCE_RANGE);
			aStarGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
			aStarGrid.Update();


			foreach(Vector2I cell in cells)
			{
				if ((bool)(GetCellTileData((int)LevelLayer.Playground, cell).GetCustomData(WALL)) || 
					(bool)(GetCellTileData((int)LevelLayer.Playground, cell).GetCustomData(CONTAINER))) 
					aStarGrid.SetPointSolid(cell);
			}


        }

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;

			if (Input.IsActionJustPressed(PATH_FINDING_INPUT))
			{
				UpdateAndClearPath();
                Vector2 lCellClicked =  new Vector2I((int)GetGlobalMousePosition().X/States.DISTANCE_RANGE, (int)GetGlobalMousePosition().Y/States.DISTANCE_RANGE);
				foreach(Vector2I cell in groundCells)
				{
                    GD.Print("Yahouu");
                    if (lCellClicked.DistanceTo(cell ) < 1)
					{
						if ((GetCellTileData((int)LevelLayer.Playground, cell) == null || 
							!(bool)(GetCellTileData((int)LevelLayer.Playground, cell).GetCustomData(INTERACTABLE))))
						{
							CreatePathFinding((Vector2I)Player.GetInstance().Position/States.DISTANCE_RANGE, cell);
                            return;
                        }
                    
						else if ((bool)(GetCellTileData((int)LevelLayer.Playground, cell).GetCustomData(WALL)) || 
							(bool)(GetCellTileData((int)LevelLayer.Playground, cell).GetCustomData(CONTAINER)))
						{
							return;
						}
					}

				}
			}
		}


		private void UpdateAndClearPath()
		{
            groundCells = GetUsedCells(0);
            cells = GetUsedCells(1);
            foreach (Vector2I cell in cells)
            {
                if ((bool)(GetCellTileData(1, cell).GetCustomData(WALL)) || (bool)(GetCellTileData(1, cell).GetCustomData(CONTAINER))) aStarGrid.SetPointSolid(cell);
            }
        }

		private void CreatePathFinding(Vector2I pBeginning, Vector2I pDestination)
		{
            SysDict.List<Vector2I> lPlayersPath = new SysDict.List<Vector2I>();
            Array<Vector2I> lPath = aStarGrid.GetIdPath(pBeginning, pDestination);
            foreach (Vector2I cellOnPath in lPath)
            {
				Player.GetInstance().path.Add(cellOnPath);
            }
			//Player.GetInstance().MovingOnPath(lPlayersPath);
        }
	
	}
}
