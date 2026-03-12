using Godot;
using Godot.Collections;
using System;
using System.IO;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban 
{
	public partial class Map : TileMap
	{
		static private Map instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Map.tscn");
        private AStarGrid2D aStarGrid = new AStarGrid2D();
        public Array<Vector2I> cells ;
		private Array<Vector2I> groundCells ;

		[Export] private Vector2I beginning;
		[Export] private Vector2I destination;

		public string CONTAINER = "Container";
		public string WALL = "Wall";
		public string INTERACTABLE = "Interactable";

        private Map():base() 
		{
			if (instance != null)
			{
				QueueFree();
				return;
			}
			instance = this;	
		}

		static public Map GetInstance()
		{
			if (instance == null) instance = (Map)factory.Instantiate();
			return instance;
		}

		public override void _Ready()
		{
			base._Ready();
			cells = GetUsedCells(1);
			groundCells = GetUsedCells(0);


			aStarGrid.Region = new Rect2I(-1,-1,50,50);
			aStarGrid.CellSize = new Vector2I(64, 64);
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

			if (Input.IsActionJustPressed("leftClick"))
			{
				Vector2 lCellClicked =  new Vector2I((int)GetGlobalMousePosition().X/64, (int)GetGlobalMousePosition().Y/64);
				foreach(Vector2I cell in groundCells)
				{

					if (lCellClicked.DistanceTo(cell ) < 1)
					{
						if ((bool)(GetCellTileData(1, cell) == null || !(bool)(GetCellTileData(1, cell).GetCustomData(INTERACTABLE))))
						{
                            GD.Print(cell);
							CreatePathFinding((Vector2I)Player.GetInstance().Position/64, cell);
                            return;
                        }
                    
						else if ((bool)(GetCellTileData(1, cell).GetCustomData(WALL)) || (bool)(GetCellTileData(1, cell).GetCustomData(CONTAINER)))
						{
							GD.Print("Can't Do");
							return;
						}
					}

				}
			}
		}

		private void CreatePathFinding(Vector2I pBeginning, Vector2I pDestination)
		{
			//aStarGrid.GetIdPath(pBeginning, pDestination);
            Array<Vector2I> lPath = aStarGrid.GetIdPath(pBeginning, pDestination);
            foreach (Vector2I cellOnPath in lPath)
            {
                SetCell(1, cellOnPath, 0, new Vector2I(4, 0));
            }
        }

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
