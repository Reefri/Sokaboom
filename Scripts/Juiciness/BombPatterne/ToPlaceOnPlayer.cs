using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class ToPlaceOnPlayer : ToPlaceOnPatterneNodeTwoD
	{
        protected static PackedScene factory = GD.Load<PackedScene>("res://Scenes/Juiciness/BombPatterne/ToPlaceOnPlayer.tscn");

        [Export] Area2D Collider;

        public static ToPlaceOnPlayer Create(Vector2 pPosition, Color pColor, float pScale = 1)
        {
            return (ToPlaceOnPlayer)Create(factory,pPosition, pColor, pScale);
        }


        public override void _Ready()
        {
            Collider.AreaEntered += OnAreaEntered;
            Collider.AreaExited += OnAreaExited;
        }

        private void OnAreaEntered(Area2D pArea)
        {
            if (pArea is BombCollectible)
            {
                ((BombCollectible)pArea).ShowChainReaction(Scale.X);
            }
        }

        private void OnAreaExited(Area2D pArea)
        {
            if (pArea is BombCollectible)
            {
                ((BombCollectible)pArea).HideChainReaction();

            }
        }

    }
}
