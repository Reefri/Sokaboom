using Godot;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban {
	public partial class BombPattern : Node2D
	{
        [Export] private float timeUntilFade = 10;
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

					if (explosionMatrix[i][j] == 1)
					{

                        Node2D lPattern = (Node2D)toPlaceOnExplosion.Instantiate();
                        lPattern.Position = Position + (new Vector2(j, i) - originPos) * States.DISTANCE_RANGE;
                        AddChild(lPattern);
                    }
                }
			}

            if (!(bool)GameManager.GetInstance().tileMap.GetCellTileData(1, posInGrid).GetCustomData("Border"))
            {
                GameManager.GetInstance().tileMap.EraseCell(1, posInGrid);
            }
            else
            {
                GD.Print("GameOver");
                //put Game Over Screen here
            }

            for (int i = 0; i < explosionMatrix.Count; i++)
            {
                for (int j = 0; j < explosionMatrix[i].Count; j++)
                {
                    if (explosionMatrix[i][j] == 1)
                    {
                        if (GameManager.GetInstance().tileMap.GetCellTileData(1, posInGrid + new Vector2I(j, i) - originPos) != null
                            && (bool)GameManager.GetInstance().tileMap.GetCellTileData(1, posInGrid + new Vector2I(j, i) - originPos).GetCustomData("Interactable"))
                        {
                            //GD.Print(Map.GetInstance().GetCellTileData(1, posInGrid + new Vector2I(j, i) - originPos).GetCustomData("Interactable"));
                            //GD.Print("exploded an interactable");

                            if ((bool)GameManager.GetInstance().tileMap.GetCellTileData(1, posInGrid + new Vector2I(j, i) - originPos).GetCustomData("Border"))
                            {
                                GD.Print("GameOver");
                                //Put Game Over screen here
                            }
                            else
                                GameManager.GetInstance().tileMap.SetCell(1, posInGrid + new Vector2I(j, i) - originPos, -1, new Vector2I(0, 0));
                        }
                    }
                    //GD.Print(originPos);
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

        private void RotateMatrix(BombPattern pBombPattern, List<List<int>> pExplosionMatrix, Vector2I pRotationVector)
        {
            if (pRotationVector == Vector2I.Up)
            {
                pBombPattern.explosionMatrix = pExplosionMatrix;

                return;
            }

            else if (pRotationVector == Vector2I.Down)
            {
                foreach(List<int> pRow in pExplosionMatrix)
                {
                    pRow.Reverse();
                }
                pExplosionMatrix.Reverse();

                pBombPattern.explosionMatrix = pExplosionMatrix;

                return;
            }

            else if (pRotationVector == Vector2I.Right)
            {
                //TO DO
                //Rotate pExplosionMatrix to the right

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

                return;
            }

            else if (pRotationVector == Vector2I.Left)
            {
                //Rotate pExplosionMatrix to the left

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

                return;
            }
        }

		public static void Create(List<List<int>> pExplosionMatrix,Vector2I pPosition, Vector2I pRotationVector)
		{
			BombPattern lBombPattern = new BombPattern();

            lBombPattern.RotateMatrix(lBombPattern, pExplosionMatrix, pRotationVector);

            //lBombPattern.explosionMatrix = pExplosionMatrix;
            lBombPattern.Position = (Vector2.One * States.DISTANCE_RANGE/2 + pPosition * States.DISTANCE_RANGE)/2;
            lBombPattern.posInGrid = pPosition;

			Main.GetInstance().CallDeferred("add_child", lBombPattern);

            Main.GetInstance().PrintListOfList(lBombPattern.explosionMatrix,',');
		}
	}
}
