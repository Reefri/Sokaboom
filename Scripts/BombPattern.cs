using Godot;
using System;
using System.Collections.Generic;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban {
	public partial class BombPattern : Node2D
	{
		private const string TO_PLACE_ON_EXPLOSION_PATH = "res://Scenes/ToPlaceOnExplosions.tscn";
        private static PackedScene toPlaceOnExplosion = GD.Load<PackedScene>(TO_PLACE_ON_EXPLOSION_PATH);

        private const string BOMB_PATTERN_PATH = "res://Scenes/BombPattern.tscn";
        private static PackedScene pattern = GD.Load<PackedScene>(BOMB_PATTERN_PATH);

		private const int TILE_LENGTH = 64;

		private Vector2I originPos;

		public List<List<int>> explosionMatrix;

        public override void _Ready()
		{
            for (int i = 0; i < explosionMatrix.Count; i++)
            {
                for (int j = 0; j < explosionMatrix[i].Count; j++)
                {
                    if (explosionMatrix[i][j] == 2)
                    {
                        originPos = new Vector2I(j, i);

                        Node2D lPattern = (Node2D)toPlaceOnExplosion.Instantiate();
                        lPattern.Position = Position;
                        lPattern.Modulate = new Color(1, 0, 0);
                        AddChild(lPattern);

                    }
                }
            }


            for (int i = 0; i < explosionMatrix.Count; i++)
			{
				for (int j = 0; j < explosionMatrix[i].Count; j++)
				{
					//if (explosionMatrix[i][j] == 2)
					//{
					//	originPos = new Vector2I(j, i);

     //                   Node2D lPattern = (Node2D)toPlaceOnExplosion.Instantiate();
					//	lPattern.Position = Position;
					//	lPattern.Modulate = new Color(1, 0, 0);
     //                   AddChild(lPattern);

     //               }
					if (explosionMatrix[i][j] == 1)
					{

                        Node2D lPattern = (Node2D)toPlaceOnExplosion.Instantiate();
                        lPattern.Position = Position + (new Vector2(j, i) - originPos) * TILE_LENGTH;
                        AddChild(lPattern);
                    }
                }
			}
			
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

		}

		protected override void Dispose(bool pDisposing)
		{

		}

		public static void Create(List<List<int>> pExplosionMatrix,Vector2I pPosition)
		{
			BombPattern bombPattern = new BombPattern();
			bombPattern.explosionMatrix = pExplosionMatrix;
			bombPattern.Position = pPosition * TILE_LENGTH;

			Main.GetInstance().AddChild(bombPattern);

		}
	}
}
