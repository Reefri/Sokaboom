using Com.IsartDigital.Utils.Effects;
using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class ToPlaceOnMousePrevisu : ToPlaceOnPatterneControl
	{
        protected static PackedScene factory = GD.Load<PackedScene>("res://Scenes/Juiciness/BombPatterne/ToPlaceOnMousePrevisu.tscn");

        [Export] Shaker shaker;
        Timer timer = new Timer();
        float waitTime = 2f;

        [Export] TextureRect explosion;
        RandomNumberGenerator lRand = new RandomNumberGenerator();

        public static ToPlaceOnMousePrevisu Create(Vector2 pPosition, Color pColor, float pScale = 1)
        {
            return (ToPlaceOnMousePrevisu)Create(factory, pPosition, pColor, pScale);
        }

        public override void _Ready()
        {
            timer.WaitTime = waitTime;
            timer.Timeout += shaker.Start;
            AddChild(timer);
            timer.Start();

            explosion.RotationDegrees = lRand.RandiRange(0, 360);
        }
    }
}
