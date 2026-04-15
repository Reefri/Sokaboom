using Godot;
using System.Collections.Generic;

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

        public const string ADD_CHILD_DEFERED = "add_child";


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




            int lYExplosionMatrixSize = explosionMatrix.Count;
            int lXExplosionMatrixSize = explosionMatrix[0].Count;


            for (int i = 0; i < lYExplosionMatrixSize; i++)
            {
                for (int j = 0; j < lXExplosionMatrixSize; j++)
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

                                JuicinessManager.GetInstance().ExplodeAllBorders(posInGrid + new Vector2I(j, i) - originPos);
                                Player.GetInstance().canInput = false;

                                JuicinessManager.GetInstance().gameOverShaker.Start();
                                //JuicinessManager.GetInstance().gameOverShaker.amplitude = new Vector2(15,15);
                                Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Circ).SetEase(Tween.EaseType.In);
                                lTween.TweenProperty(JuicinessManager.GetInstance().gameOverShaker, "amplitude", Vector2.Zero, 3f);
                                lTween.Finished += JuicinessManager.GetInstance().gameOverShaker.Stop;


                                return;
                            }
                            else
                            {

                                if ((bool)lCurrentTileData.GetCustomData(Map.BOX))
                                {
                                    SoundManager.GetInstance().PlayBoxExplosion();

                                    FireWork.CreateMult((posInGrid + new Vector2I(j, i) - originPos) * Map.DISTANCE_RANGE,GameManager.GetInstance());
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


            lBombPattern.Position = (pPosition )* Map.DISTANCE_RANGE /2; 
            lBombPattern.posInGrid = pPosition;


			GameManager.GetInstance().gameOverExplosionContainer.CallDeferred(ADD_CHILD_DEFERED, lBombPattern);
		}
	}
}
