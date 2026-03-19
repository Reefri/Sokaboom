using Godot;

// Author : Ethan FRENARD

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombCollectible : Area2D
	{

		private const string BOMB_COLLECTIBLE_PATH = "res://Scenes/BombCollectible.tscn";

        private static PackedScene bombCollectible = GD.Load<PackedScene>(BOMB_COLLECTIBLE_PATH);

        public static Vector2 sizeBomb;

        public Bomb bomb;
        public override void _Ready()
		{
			AreaEntered += BombCollectibleAreaEntered;

            Sprite2D lSpriteBomb = (Sprite2D)GetChild(1);
			sizeBomb = lSpriteBomb.Texture.GetSize();
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

		public static void Create(Bomb pBomb, Vector2I pPosition)
		{
			BombCollectible lBombCollectible = (BombCollectible)bombCollectible.Instantiate();
			lBombCollectible.Position = (Vector2.One / 2 + pPosition) * States.DISTANCE_RANGE;
			lBombCollectible.ZIndex = 1;

			lBombCollectible.bomb = pBomb;
			GameManager.GetInstance().bombCollectibleContainer.AddChild(lBombCollectible);
		}
		protected override void Dispose(bool pDisposing)
		{

		}
	}
}
