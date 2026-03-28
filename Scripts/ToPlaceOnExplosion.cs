using Godot;
using System;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban {
	
	public partial class ToPlaceOnExplosion : Area2D
	{
        private static PackedScene factory = GD.Load<PackedScene>("res://Scenes/ToPlaceOnExplosions.tscn");

        [Export] private CollisionShape2D collisionShape;

		[Export] private GpuParticles2D particles;

        public static ToPlaceOnExplosion Create(Vector2 pPosition, Color pColor, bool pDoesExplose = false, float pScale = 1)
        {
            ToPlaceOnExplosion lPattern = (ToPlaceOnExplosion)factory.Instantiate();
            lPattern.GlobalPosition = pPosition;
            lPattern.Modulate = pColor;
			lPattern.collisionShape.Disabled = !pDoesExplose;
			lPattern.Scale = new Vector2(pScale, pScale);
			return lPattern;
        }
        
		public override void _Ready()
		{
            AreaEntered += CheckForChainReaction;

			particles.Emitting = true;
			particles.Finished += QueueFree;
		}

        private void CheckForChainReaction(Area2D pArea)
        {

            if (pArea is BombCollectible)
			{
                
				BombCollectible lBombCollec = (BombCollectible)pArea;
				lBombCollec.bomb.Explode((Vector2I)lBombCollec.Position / States.DISTANCE_RANGE	, Vector2I.Up);
				lBombCollec.QueueFree();

				GameManager.GetInstance().RemoveBomb(lBombCollec.bomb);
                GameManager.GetInstance().UpdateCurrentPosition();
            }
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
