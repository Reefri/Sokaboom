using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class ToPlaceOnPlayer : ToPlaceOnPatterneNodeTwoD
	{
        protected static PackedScene factory = GD.Load<PackedScene>("res://Scenes/Juiciness/BombPatterne/ToPlaceOnPlayer.tscn");


        public static ToPlaceOnPlayer Create(Vector2 pPosition, Color pColor, float pScale = 1)
        {
            GD.Print("created player");

            return (ToPlaceOnPlayer)Create(factory,pPosition, pColor, pScale);
        }

    }
}
