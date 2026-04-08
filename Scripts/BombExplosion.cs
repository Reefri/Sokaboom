using Godot;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban {
	public partial class BombExplosion : Node2D
	{
        private float timeUntilFade = 1.5f;
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
            JuicinessManager.GetInstance().simpleBombShaker.Start();
            SoundManager.GetInstance().PlayExplosion();


            originPos = (new BombPattern(
                this, 
                explosionMatrix, 
                BombPattern.EnumOfExplosionPattern.Bomb
                )
                ).originePos;



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

                                JuicinessManager.GetInstance().ExplodeAllBorders(posInGrid + new Vector2I(j, i) - originPos);
                                Player.GetInstance().canInput = false;

                                JuicinessManager.GetInstance().gameOverShaker.Start();

                                return;
                            }
                            else
                            {

                                if ((bool)lCurrentTileData.GetCustomData(Map.BOX))
                                {
                                    SoundManager.GetInstance().PlayBoxExplosion();

                                    FireWork.CreateMult((posInGrid + new Vector2I(j, i) - originPos) *States.DISTANCE_RANGE,GameManager.GetInstance());
                                }
                                if ((bool)lCurrentTileData.GetCustomData(Map.WALL))
                                {
                                    SoundManager.GetInstance().PlayWallExplosion();
                                }
                                GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Playground, posInGrid + new Vector2I(j, i) - originPos);
                            }
                                
                        }

                        if (lTileMap.GetCellTileData((int)Map.LevelLayer.Target, posInGrid + new Vector2I(j, i) - originPos) != null)
                        {
                            lTileMap.SetCell((int)Map.LevelLayer.Target, posInGrid + new Vector2I(j, i) - originPos, -1);
                            GameManager.GetInstance().currentPosition.value.targetsPos.Remove(posInGrid + new Vector2I(j, i) - originPos);

                            GD.Print("tu viens de détruire une cible !");
                        }
                    }
                }
            }


            GameManager.GetInstance().UpdateCurrentPosition();

            GameManager.GetInstance().CheckAndDoWin();
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
			BombExplosion lBombPattern = new BombExplosion();

            lBombPattern.explosionMatrix = Main.RotateMatrix(pExplosionMatrix,pRotationVector);


            lBombPattern.Position = (pPosition )* States.DISTANCE_RANGE /2; // pourquoi distance/2 ??
            lBombPattern.posInGrid = pPosition;


			GameManager.GetInstance().gameOverExplosionContainer.CallDeferred("add_child", lBombPattern);

		}
	}
}
