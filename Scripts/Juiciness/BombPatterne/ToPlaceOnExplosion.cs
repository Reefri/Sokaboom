using Godot;
using System;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban {
	
	public partial class ToPlaceOnExplosion : ToPlaceOnPatterneNodeTwoD
	{
        protected static PackedScene factory = GD.Load<PackedScene>("res://Scenes/Juiciness/BombPatterne/ToPlaceOnExplosions.tscn");


        public static ToPlaceOnExplosion Create(Vector2 pPosition, Color pColor, float pScale = 1)
        {
            return (ToPlaceOnExplosion)Create(factory,pPosition, pColor, pScale);
        }


        [Export] private Area2D collider;

        [Export] private GpuParticles2D particles;

        [Export] private Node2D redParticleContainer;

        [Export] private GpuParticles2D yellowParticle;

        public override void _Ready()
		{
            collider.AreaEntered += CheckForChainReaction;

			particles.Emitting = true;
            yellowParticle.Emitting = true;
            yellowParticle.Finished += QueueFree;
			particles.Finished += QueueFree;

            foreach(Node2D lContainer in redParticleContainer.GetChildren())
            {
                foreach(GpuParticles2D lRedParticles in lContainer.GetChildren())
                {
                    lRedParticles.Emitting = true;
                    ((ParticleProcessMaterial)lRedParticles.ProcessMaterial).Direction = new Vector3(GlobalPosition.X - lRedParticles.GlobalPosition.X, GlobalPosition.Y - lRedParticles.GlobalPosition.Y, 0);
                    lRedParticles.Finished += QueueFree;
                }
            }

		}


        private void CheckForChainReaction(Area2D pArea)
        {

            if (pArea is BombCollectible)
			{
                
				BombCollectible lBombCollec = (BombCollectible)pArea;
				lBombCollec.bomb.Explode((Vector2I)lBombCollec.Position / States.DISTANCE_RANGE	, Vector2I.Up);
				lBombCollec.QueueFree();

				GameManager.GetInstance().RemoveBombAtIndex(lBombCollec.bomb.indexInLevel);
                GameManager.GetInstance().UpdateCurrentPosition();
            }
        }

	}
}
