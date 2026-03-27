using Godot;
using System.Collections.Generic;

// Author : Ethan FRENARD

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombCollectible : Area2D
	{
        [Export] Node2D renderer;

		private const string BOMB_COLLECTIBLE_PATH = "res://Scenes/Gameplay/Bomb/BombCollectible.tscn";

        private static PackedScene bombCollectible = GD.Load<PackedScene>(BOMB_COLLECTIBLE_PATH);

        private PrevisualisationBomb previsualisationBomb = (PrevisualisationBomb)GD.Load<PackedScene>("res://Scenes/UI/PrevisualisationBomb.tscn").Instantiate();

        public Bomb bomb;

        private Vector2 previsualisationOriginPos;
        private Vector2 rightCornerOfCollectible = new Vector2(25, -25);
        private float previsualisationScale = 0.3f;
        private float downFactor = 10;
        private float sideFactor = 0;
        private bool OriginOnTop;
        public override void _Ready()
		{


            previsualisationBomb.explosionMatrix = bomb.explosionMatrix;

            Node2D lNode = new Node2D();

            previsualisationOriginPos = (new BombPatterne(lNode, false, bomb.explosionMatrix)).originePos;

            lNode.Scale = Vector2.One * 0.3f;
            renderer.AddChild(lNode);
            lNode.GlobalPosition = renderer.GlobalPosition + rightCornerOfCollectible;



            AreaEntered += BombCollectibleAreaEntered;


            InputPickable = true;
            MouseEntered += InBomb;
            MouseExited += OutBomb;
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
            UIManager.GetInstance().AddChild(previsualisationBomb);
        }
        private void OutBomb()
        {
            UIManager.GetInstance().RemoveChild(previsualisationBomb);
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
            previsualisationBomb.QueueFree();
        }
	}
}
