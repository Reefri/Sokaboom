using Godot;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban {
	public partial class BombPattern : Node2D
	{
        [Export] private float timeUntilFade = 0.2f;
		private float time = 0;

        private float borderScreenShakePower = 25;
        private float borderScreenShakeTime = 2;

        private float explosionScreenShakePower = 15;
        private float explosionScreenShakeTime = 0.25f;

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
                        AddChild(ToPlaceOnExplosion.Create(GlobalPosition, new Color(1, 0, 0)));
                    }
                }
            }

            for (int i = 0; i < explosionMatrix.Count; i++)
			{
				for (int j = 0; j < explosionMatrix[i].Count; j++)
				{

					if (explosionMatrix[i][j] == 1)
					{
                        Vector2 lPosition = GlobalPosition + (new Vector2(j, i) - originPos) * States.DISTANCE_RANGE;
                        AddChild(ToPlaceOnExplosion.Create(lPosition, new Color(1, 1, 1)));

                        //positions differentes de celles de la grille ??
                    }
                }
			}
           
            for (int i = 0; i < explosionMatrix.Count; i++)
            {
                for (int j = 0; j < explosionMatrix[i].Count; j++)
                {
                    if (explosionMatrix[i][j] != 0)
                    {

                        TileMap lTileMap = GameManager.GetInstance().tileMap;

                        TileData lCurrentTileData = lTileMap.GetCellTileData((int)Map.LevelLayer.Playground, posInGrid + new Vector2I(j, i) - originPos);

                        if ( lCurrentTileData != null && 
                            (bool)lCurrentTileData.GetCustomData(Map.INTERACTABLE))
                        {

                            if ((bool)lCurrentTileData.GetCustomData(Map.BORDER))
                            {
                                GD.Print("GameOver");
                                //Put Game Over screen here
                                CameraManager.GetInstance().ShakeScreen(borderScreenShakePower, borderScreenShakeTime);
                            }
                            else
                            {
                                CameraManager.GetInstance().ShakeScreen(explosionScreenShakePower, explosionScreenShakeTime);

                                if ((bool)lCurrentTileData.GetCustomData(Map.CONTAINER))
                                {
                                    FireWork.CreateMult((posInGrid + new Vector2I(j, i) - originPos+Vector2.One/2) *States.DISTANCE_RANGE,GameManager.GetInstance());
                                }
								GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Playground, posInGrid + new Vector2I(j, i) - originPos);
                            }
                                
                        }

                        if (lTileMap.GetCellTileData((int)Map.LevelLayer.Target, posInGrid + new Vector2I(j, i) - originPos) != null)
                        {
                            lTileMap.SetCell((int)Map.LevelLayer.Target, posInGrid + new Vector2I(j, i) - originPos, -1);
                            GD.Print("tu viens de détruire une cible !");
                        }
                    }
                }
            }

            GameManager.GetInstance().UpdateCurrentPosition();
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

            lBombPattern.RotateMatrix(lBombPattern, Main.GetInstance().DuplicateListOfList(pExplosionMatrix), pRotationVector);

            lBombPattern.Position = (Vector2.One/2 + pPosition )* States.DISTANCE_RANGE /2; // pourquoi distance/2 ??
            lBombPattern.posInGrid = pPosition;

			Main.GetInstance().CallDeferred("add_child", lBombPattern);

		}
	}
}
