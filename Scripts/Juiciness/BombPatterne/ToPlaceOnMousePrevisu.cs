using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class ToPlaceOnMousePrevisu : ToPlaceOnPatterneControl
	{
        protected static PackedScene factory = GD.Load<PackedScene>("res://Scenes/Juiciness/BombPatterne/ToPlaceOnMousePrevisu.tscn");


        public static ToPlaceOnMousePrevisu Create(Vector2 pPosition, Color pColor, float pScale = 1)
        {
            return (ToPlaceOnMousePrevisu)Create(factory, pPosition, pColor, pScale);
        }

    }
}
