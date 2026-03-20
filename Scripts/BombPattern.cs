using Godot;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban {
	public partial class BombPattern : Node2D
	{
        [Export] private float timeUntilFade = 0.2f;
		private float time = 0;

		private const string TO_PLACE_ON_EXPLOSION_PATH = "res://Scenes/ToPlaceOnExplosions.tscn";
        private static PackedScene toPlaceOnExplosion = GD.Load<PackedScene>(TO_PLACE_ON_EXPLOSION_PATH);

        private const string BOMB_PATTERN_PATH = "res://Scenes/BombPattern.tscn";
        private static PackedScene pattern = GD.Load<PackedScene>(BOMB_PATTERN_PATH);

		private Vector2I originPos;
        protected Vector2I posInGrid;

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

                        Area2D lPattern = (Area2D)toPlaceOnExplosion.Instantiate();
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

					if (explosionMatrix[i][j] == 1)
					{

                        Area2D lPattern = (Area2D)toPlaceOnExplosion.Instantiate();
                        lPattern.Position = Position + (new Vector2(j, i) - originPos) * States.DISTANCE_RANGE;
                        //positions differentes de celles de la grille ??
                        AddChild(lPattern);


                    }
                }
			}

           
            for (int i = 0; i < explosionMatrix.Count; i++)
            {
                for (int j = 0; j < explosionMatrix[i].Count; j++)
                {
                    if (explosionMatrix[i][j] != 0)
                    {
                        if (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, posInGrid + new Vector2I(j, i) - originPos) != null && 
                            (bool)GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, posInGrid + new Vector2I(j, i) - originPos).GetCustomData("Interactable"))
                        {
                           
                            if ((bool)GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, posInGrid + new Vector2I(j, i) - originPos).GetCustomData("Border"))
                            {
                                GD.Print("GameOver");
                                //Put Game Over screen here
                            }
                            else
                                GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Playground, posInGrid + new Vector2I(j, i) - originPos);
                        }

                        if (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Target, posInGrid + new Vector2I(j, i) - originPos) != null)
                        {
                            GameManager.GetInstance().tileMap.SetCell((int)Map.LevelLayer.Target, posInGrid + new Vector2I(j, i) - originPos, -1);
                            GD.Print("tu viens de détruire une cible !");
                        }
                    }
                }
            }


        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

            time += lDelta;
            if(time >= timeUntilFade)
            {
                QueueFree();
            }
		}

		protected override void Dispose(bool pDisposing)
		{

		}

        private void ChainReaction(Node2D pExplosion)
        {
            
        }

        private void RotateMatrix(BombPattern pBombPattern, List<List<int>> pExplosionMatrix, Vector2I pRotationVector)
        {

            if (pRotationVector == Vector2I.Up)
            {
                pBombPattern.explosionMatrix = pExplosionMatrix;

            }

            else if (pRotationVector == Vector2I.Down)
            {
                foreach(List<int> pRow in pExplosionMatrix)
                {
                    pRow.Reverse();
                }
                pExplosionMatrix.Reverse();

                pBombPattern.explosionMatrix = pExplosionMatrix;

            }

            else if (pRotationVector == Vector2I.Right)
            {
                pExplosionMatrix.Reverse();
                List<List<int>> lRotatedMatrix = new List<List<int>>();

                for (int i = 0; i < pExplosionMatrix[0].Count; i++)
                {
                    List<int> lCollumn = new List<int>();
                    for (int j = 0; j < pExplosionMatrix.Count; j++)
                    {
                        lCollumn.Add(pExplosionMatrix[j][i]);

                    }
                    lRotatedMatrix.Add(lCollumn);

                }

                pBombPattern.explosionMatrix = lRotatedMatrix;

            }

            else if (pRotationVector == Vector2I.Left)
            {

                foreach (List<int> pRow in pExplosionMatrix)
                {
                    pRow.Reverse();
                }

                List<List<int>> lRotatedMatrix = new List<List<int>>();


                for (int i = 0; i < pExplosionMatrix[0].Count; i++)
                {
                    List<int> lCollumn = new List<int>();
                    for (int j = 0; j < pExplosionMatrix.Count; j++)
                    {
                        lCollumn.Add(pExplosionMatrix[j][i]);

                    }
                    lRotatedMatrix.Add(lCollumn);

                }

                pBombPattern.explosionMatrix = lRotatedMatrix;

            }
        }

		public static void Create(List<List<int>> pExplosionMatrix,Vector2I pPosition, Vector2I pRotationVector)
		{
			BombPattern lBombPattern = new BombPattern();

            lBombPattern.RotateMatrix(lBombPattern, pExplosionMatrix, pRotationVector);

            lBombPattern.Position = (Vector2.One/2 + pPosition )* States.DISTANCE_RANGE /2; // pourquoi distance/2 ??
            lBombPattern.posInGrid = pPosition;

			Main.GetInstance().CallDeferred("add_child", lBombPattern);

		}
	}
}
