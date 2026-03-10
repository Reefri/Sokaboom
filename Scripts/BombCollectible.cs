using Godot;

// Author : Ethan FRENARD

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombCollectible : Area2D
	{
		[Export] private Vector2I positionOnGrid = Vector2I.Zero;

		[Export] private int bombPatternIndex = 0;

		private Bomb bomb;
		public override void _Ready()
		{
            AreaEntered += BombCollectible_AreaEntered;
	
			
			Position = Vector2.One * States.DISTANCE_RANGE/2 + positionOnGrid * States.DISTANCE_RANGE;

            GridManager.GetInstance().ChangeLevel(0);

			if (bombPatternIndex > GridManager.GetInstance().CurrentLevel.bombs.Count - 1)
			{
                GD.Print("Bomb pattern of index " + bombPatternIndex + " does not exist. the bomb has been removed \n");
				QueueFree();	
            }
			else
			{
                bomb = GridManager.GetInstance().CurrentLevel.bombs[bombPatternIndex];
                GD.Print("Bomb " + bombPatternIndex + " pattern : \n" + bomb);

                
            }

			

        }

        private void BombCollectible_AreaEntered(Area2D pArea)
        {
			if(pArea == Player.GetInstance())
			{
				GD.Print(" Player Picks up the bomb with this pattern : \n" + bomb);

				Player.GetInstance().bombInHand = bomb;
				Player.GetInstance().holdingBomb = true;

                //bomb.Explode(positionOnGrid);
			}
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

		}

		protected override void Dispose(bool pDisposing)
		{

		}
	}
}
