using Godot;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombCollectiblePattern : Area2D
	{
        private static PackedScene factory = GD.Load<PackedScene>("res://Scenes/Gameplay/Bomb/CollectiblePattern.tscn");

        [Export] private CollisionShape2D collisionShape;
        public static BombCollectiblePattern Create(Vector2 pPosition, Color pColor, bool pBool = false, float pScale = 1)
        {
            BombCollectiblePattern lPattern = (BombCollectiblePattern)factory.Instantiate();
            lPattern.GlobalPosition = pPosition;
            lPattern.Modulate = pColor;
            lPattern.collisionShape.Disabled = pBool;
            lPattern.Scale = new Vector2(pScale, pScale);
            return lPattern;
        }
        public override void _Ready()
		{

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
