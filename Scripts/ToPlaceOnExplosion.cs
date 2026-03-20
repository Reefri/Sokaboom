using Godot;
using System;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban {
	
	public partial class ToPlaceOnExplosion : Area2D
	{
		public override void _Ready()
		{
            AreaEntered += CheckForChainReaction;
		}

        private void CheckForChainReaction(Area2D pArea)
        {
			if(pArea is BombCollectible)
			{
				BombCollectible lBombCollec = (BombCollectible)pArea;
				lBombCollec.bomb.Explode((Vector2I)lBombCollec.Position / States.DISTANCE_RANGE	, Vector2I.Up);
				lBombCollec.QueueFree();
				GD.Print(lBombCollec.bomb);
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
