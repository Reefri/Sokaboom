using Godot;
using Godot.Collections;
using System;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban 
{
	public partial class Map : TileMap
	{
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Level.tscn");
        private AStarGrid2D aStarGrid = new AStarGrid2D();
        public Array<Vector2I> cells ;

		public const string CONTAINER = "Container";
		public const string WALL = "Wall";
		public const string INTERACTABLE = "Interactable";

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
        }


        public override string ToString()
        {
			
            return myInt.ToString();
        }

	}
}
