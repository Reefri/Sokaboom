using Godot;

// Author : Ethan FRENARD

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombCollectible : Area2D
	{

		private const string BOMB_COLLECTIBLE_PATH = "res://Scenes/BombCollectible.tscn";

        private static PackedScene bombCollectible = GD.Load<PackedScene>(BOMB_COLLECTIBLE_PATH);

		private Bomb bomb;
        public override void _Ready()
		{
			AreaEntered += BombCollectible_AreaEntered;
        }

        private void BombCollectible_AreaEntered(Area2D pArea)
        {
			if(pArea == Player.GetInstance() && Player.GetInstance().bombInHand == null)
			{			
				GD.Print(" Player Picks up the bomb with this pattern : \n" + bomb);

				Player.GetInstance().bombInHand = bomb;
				Player.GetInstance().holdingBomb = true;

                QueueFree();
			}
        }

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

		}

		public static void Create(Bomb pBomb, Vector2I pPosition)
		{
			BombCollectible lBombCollectible = (BombCollectible)bombCollectible.Instantiate();
			lBombCollectible.Position = (Vector2.One / 2 + pPosition) * States.DISTANCE_RANGE;
			lBombCollectible.ZIndex = 1;

			lBombCollectible.bomb = pBomb;
			GameManager.GetInstance().AddChild(lBombCollectible);
		}
		protected override void Dispose(bool pDisposing)
		{

		}
	}
}
