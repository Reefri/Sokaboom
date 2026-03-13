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

		public const string PLAY_OBJECT = "PlayObject";
		public const string CONTAINER = "Container";
		public const string WALL = "Wall";
		public const string INTERACTABLE = "Interactable";
		public const string TARGET = "Target";



        public static Dictionary<string, ObjectChar> interactableToObjectChar = new Dictionary<string, ObjectChar>
		{
			{ CONTAINER,ObjectChar.BOX},
			{ WALL,ObjectChar.WALL},
			{ TARGET,ObjectChar.TARGET},
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

    

	}
}
