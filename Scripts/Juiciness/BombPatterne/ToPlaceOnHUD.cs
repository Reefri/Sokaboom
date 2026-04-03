using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class ToPlaceOnHUD : ToPlaceOnPatterneControl
	{
        protected static PackedScene factory = GD.Load<PackedScene>("res://Scenes/Juiciness/BombPatterne/ToPlaceOnHUD.tscn");


        public static ToPlaceOnHUD Create(Vector2 pPosition, Color pColor, float pScale = 1)
        {
            return (ToPlaceOnHUD)Create(factory,pPosition, pColor, pScale);
        }

    }
}
