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



        public static Map Create()
		{
			return (Map)factory.Instantiate();
		}

		public override void _Ready()
		{
			base._Ready();
			cells = GetUsedCells(1);
        }



	}
}
