using Godot;
using System.Collections.Generic;

// Author : Ethan FRENARD

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombCollectible : Area2D
	{

		private const string BOMB_COLLECTIBLE_PATH = "res://Scenes/BombCollectible.tscn";

        private static PackedScene bombCollectible = GD.Load<PackedScene>(BOMB_COLLECTIBLE_PATH);

        public Bomb bomb;

        private Timer timerBeforePrevisualisation = new Timer();
        private int timeBeforeVisualisation = 1;

        private Vector2 previsualisationOriginPos;
        private Vector2 rightCornerOfCollectible = new Vector2(25, -25);
        private float previsualisationScale = 0.3f;
        private float downFactor = 10;
        private float sideFactor = 0;
        private bool OriginOnTop;
        public override void _Ready()
		{

            for (int i = 0; i < bomb.explosionMatrix.Count; i++)
            {
                for (int j = 0; j < bomb.explosionMatrix[i].Count; j++)
                {
                    if (bomb.explosionMatrix[i][j] == 2)
                    {
                        previsualisationOriginPos = new Vector2I(j, i);

                        if(j == 0)
                        {
                            sideFactor = (bomb.explosionMatrix[0].Count - 1) * 10;
                            GD.Print(sideFactor);
                        }

                        if (i == 0)
                        {
                            downFactor = 0;
                            AddChild(ToPlaceOnExplosion.Create(rightCornerOfCollectible + (Vector2.Left * sideFactor), new Color(1, 0, 0), true, previsualisationScale));
                        }
                        else
                            AddChild(ToPlaceOnExplosion.Create(rightCornerOfCollectible + Vector2.Down * bomb.explosionMatrix.Count * downFactor
                                + (Vector2.Left * sideFactor)
                                , new Color(1, 0, 0), true, previsualisationScale));
                    }
                }
            }

            for (int i = 0; i < bomb.explosionMatrix.Count; i++)
            {
                for (int j = 0; j < bomb.explosionMatrix[i].Count; j++)
                {

                    if (bomb.explosionMatrix[i][j] == 1)
                    {
                        Vector2 lPosition = rightCornerOfCollectible + Vector2.Down * bomb.explosionMatrix.Count * downFactor 
                            + (Vector2.Left * sideFactor)
                            + (new Vector2(j, i) - previsualisationOriginPos) * States.DISTANCE_RANGE * previsualisationScale;
                        
                        AddChild(ToPlaceOnExplosion.Create(lPosition, new Color(1, 1, 1), true, previsualisationScale));
                    }
                }
            }

            AreaEntered += BombCollectibleAreaEntered;


            InputPickable = true;
            MouseEntered += InBomb;
            MouseExited += OutBomb;
            

            timerBeforePrevisualisation.WaitTime = timeBeforeVisualisation;
            timerBeforePrevisualisation.Timeout += () => PrevisualisationBomb.CreateInstance(bomb.explosionMatrix);
            timerBeforePrevisualisation.OneShot = true;
            AddChild(timerBeforePrevisualisation);
        }

        private void BombCollectibleAreaEntered(Area2D pArea)
        {
			if(pArea == Player.GetInstance() && Player.GetInstance().bombInHand == null)
			{
				GameManager.GetInstance().RemoveBombAtIndex(bomb.indexInLevel);

				Player.GetInstance().GiveBombToPlayer(bomb);

                QueueFree();
			}
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
        }

        private void InBomb()
        {
            timerBeforePrevisualisation.Start();
        }
        private void OutBomb()
        {

            if (!timerBeforePrevisualisation.IsStopped()) timerBeforePrevisualisation.Stop();
            if (PrevisualisationBomb.instance != null) PrevisualisationBomb.GetInstance().QueueFree();
        }

        public static void Create(Bomb pBomb, Vector2I pPosition, int pIndex)
		{
			BombCollectible lBombCollectible = (BombCollectible)bombCollectible.Instantiate();
			lBombCollectible.Position = (Vector2.One / 2 + pPosition) * States.DISTANCE_RANGE;
			lBombCollectible.ZIndex = 1;

			lBombCollectible.bomb = pBomb;
			GameManager.GetInstance().bombCollectibleContainer.AddChild(lBombCollectible);
		}
		protected override void Dispose(bool pDisposing)
		{
            if (PrevisualisationBomb.instance != null && PrevisualisationBomb.GetInstance().explosionMatrix == bomb.explosionMatrix) PrevisualisationBomb.GetInstance().QueueFree();
        }
	}
}
