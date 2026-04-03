using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class ToPlaceOnCollectible : ToPlaceOnPatterneNodeTwoD
	{
        protected static PackedScene factory = GD.Load<PackedScene>("res://Scenes/Juiciness/BombPatterne/ToPlaceOnCollectible.tscn");


        public static ToPlaceOnCollectible Create(Vector2 pPosition, Color pColor, float pScale = 1)
        {
            return (ToPlaceOnCollectible)Create(factory,pPosition, pColor, pScale);
        }

    }
}
