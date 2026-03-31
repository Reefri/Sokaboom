using Godot;

// Author : Ethan Masse

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

			foreach (GpuParticles2D lPart in partContainer.GetChildren())
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

		protected override void Dispose(bool pDisposing)
		{

		}


		public static BorderExplosion Create(Vector2I pPosition)
		{


            BorderExplosion lExplosion = (BorderExplosion)borderExplosion.Instantiate();
            lExplosion.GlobalPosition = (pPosition + Vector2.One / 2) * States.DISTANCE_RANGE;
            //GameManager.GetInstance().gameOverExplosionContainer.AddChild(lExplosion);
            GameManager.GetInstance().AddChild(lExplosion);


			return lExplosion;

        }

        public void Stop()
		{
            EmitSignal(SignalName.Finished);

            QueueFree();

		}

	}
}
