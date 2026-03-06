using Godot;
using Godot.Collections;
using System;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban 
{
	public partial class Map : TileMap
	{
		static private Map instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Map.tscn");
        private AStarGrid2D aStarGrid = new AStarGrid2D();
        public Array<Vector2I> cells ;

        private Map():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(Map) + " Instance already exist, destroying the last added.");
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
	}
}
