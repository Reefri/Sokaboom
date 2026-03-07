using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class GameManager : Node2D
	{
		static private GameManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/GameManager.tscn");

		private PackedScene levelFactory = GD.Load<PackedScene>("res://Scenes/Level.tscn");

		private Level currentLevel;

		private List<Vector2I> neighborsCoor = new List<Vector2I>
		{
			Vector2I.Right,
			Vector2I.Left,
			Vector2I.Up,
			Vector2I.Down,
        };

		private Dictionary<ObjectChar, Vector2I> objectPositionOnTileSet = new Dictionary<ObjectChar, Vector2I>
		{
			{ ObjectChar.BOX , new Vector2I(6,0) },
			{ ObjectChar.WALL , new Vector2I(7,7) },
			{ ObjectChar.TARGET , new Vector2I(1,3) },
			{ ObjectChar.EMPTY , new Vector2I(11,6) },
		};
		
		private TileMap tileMap;

		private GameManager():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(GameManager) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public GameManager GetInstance()
		{
			if (instance == null) instance = (GameManager)factory.Instantiate();
			return instance;
		}

		public override void _Ready()
		{
			GridManager.GetInstance().ChangeLevel(0);
			currentLevel = GridManager.GetInstance().CurrentLevel;
			
			CreateLevel();
		}


		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}

		private void CreateLevel()
		{
			TileMap lLevel = (TileMap)levelFactory.Instantiate();

			Vector2I lPlayerPosition = new Vector2I(0,0);

			bool isGroundLayer;

			for (int i=0; i < currentLevel.Map.Count; i++)
			{
				for (int j=0; j < currentLevel.Map[0].Length; j++)
				{

					switch (currentLevel.Map[i][j])
					{
						case (char)ObjectChar.EMPTY:
							continue;
		
						case (char)ObjectChar.PLAYER:
							lPlayerPosition = new Vector2I(j, i);
							continue;

						case (char)ObjectChar.WALL:
                            lLevel.SetCell(0, new Vector2I(j, i), 0, objectPositionOnTileSet[ObjectChar.EMPTY]);
                            break;
					}

                    lLevel.SetCell(1, new Vector2I(j, i), 0,
                            objectPositionOnTileSet[(ObjectChar)currentLevel.Map[i][j]]
                            );

                }
			}

			tileMap = lLevel;
			FillGroundTiles(lPlayerPosition);

			AddChild(lLevel);
		}

		private void FillGroundTiles(Vector2I pStartCoor)
		{
			tileMap.SetCell(0, pStartCoor, 0, objectPositionOnTileSet[ObjectChar.EMPTY]);

			foreach (Vector2I lNeighbor in neighborsCoor)
			{
				if (tileMap.GetCellAtlasCoords(0, pStartCoor + lNeighbor) != objectPositionOnTileSet[ObjectChar.EMPTY])
				{
					FillGroundTiles(pStartCoor+lNeighbor);
				}
			}
		}

	}
}
