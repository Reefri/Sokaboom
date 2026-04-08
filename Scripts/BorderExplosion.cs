using Godot;
using System.Collections.Generic;
using System.Linq;

// Author : Sacha GRAMATIKOFF

namespace Com.IsartDigital.Sokoban 
{
	public partial class BorderExplosion : Node2D
	{

		[Signal] public delegate void FinishedEventHandler();

        private static PackedScene borderExplosion = GD.Load<PackedScene>("res://Scenes/Juiciness/BorderExplosion.tscn");


        private float lifeTime = 4;
		private float randomLifeTime = 0.3f;

		private float currentTime = 0;

		[Export] Node2D partContainer;

		[Export] Sprite2D groundMark;

		public override void _Ready()
		{
			lifeTime += (GD.Randf() - 0.5f) * 2 * randomLifeTime;

			List<Node> lPartContainerChildren = partContainer.GetChildren().ToList();


            foreach (GpuParticles2D lPart in lPartContainerChildren)
			{
				lPart.OneShot = true;
				lPart.Emitting = true;
			}

		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			currentTime += lDelta;

            groundMark.Modulate = new Color(1,1,1, (lifeTime-currentTime)/currentTime);

			if (currentTime > lifeTime)
			{
				Stop();
			}

		}


		public static BorderExplosion Create(Vector2I pPosition)
		{
            BorderExplosion lExplosion = (BorderExplosion)borderExplosion.Instantiate();
            lExplosion.GlobalPosition = (pPosition) * Map.DISTANCE_RANGE;
            GameManager.GetInstance().gameOverExplosionContainer.AddChild(lExplosion);


			return lExplosion;

        }

        public void Stop()
		{
            EmitSignal(SignalName.Finished);

            QueueFree();

		}

	}
}
