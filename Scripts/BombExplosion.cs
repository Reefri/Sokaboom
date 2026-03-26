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


            originPos = (new BombPatterne(this, true, explosionMatrix)).originePos;



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

		public static void Create(List<List<int>> pExplosionMatrix,Vector2I pPosition, Vector2I pRotationVector)
		{
			BombPattern lBombPattern = new BombPattern();

            lBombPattern.explosionMatrix = Main.GetInstance().RotateMatrix(pExplosionMatrix,pRotationVector);

            //lBombPattern.RotateMatrix(lBombPattern, Main.GetInstance().DuplicateListOfList(pExplosionMatrix), pRotationVector);

            lBombPattern.Position = (Vector2.One/2 + pPosition )* States.DISTANCE_RANGE /2; // pourquoi distance/2 ??
            lBombPattern.posInGrid = pPosition;

			Main.GetInstance().CallDeferred("add_child", lBombPattern);

		}
	}
}
