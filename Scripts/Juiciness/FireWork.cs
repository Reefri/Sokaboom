using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class FireWork : Node2D
	{
		private static PackedScene factory = (PackedScene)GD.Load("res://Scenes/Juiciness/Firework.tscn");

		private const string EXPLOSIONCONTAINER_NODE_PATH = "ExplosionContainer";

		private const int MIN_FIREWORK_NUMBER = 4;
		private const int MAX_FIREWORK_NUMBER = 6;


		private float pullTowardCenter = 1f;

		private bool disableProcess = false;

		private float speed = 400;
		private float acceleration = -200;
		private float rangeAcceleration = 30;

		private Vector2 direction = Vector2.Up;

		private float maxRandCoef = 3f;
		private float randCoef=0;

		private Timer timer = new Timer();
		private float timerDuration = 0.5f;

		public override void _Ready()
		{
			foreach (GpuParticles2D lGPUPart in GetNode(EXPLOSIONCONTAINER_NODE_PATH).GetChildren())
			{
				lGPUPart.OneShot = true;
			}

			timer.WaitTime = timerDuration;
			timer.Autostart = true;
			timer.OneShot = false;
			timer.Timeout += () => randCoef = (float)GD.RandRange(-maxRandCoef, maxRandCoef)/(Mathf.Pow(maxRandCoef*1.01f-Mathf.Abs(randCoef),0.5f));
			AddChild(timer);

			acceleration += (float)GD.RandRange(-rangeAcceleration, rangeAcceleration);
		}

		public override void _Process(double pDelta)
		{

            if (disableProcess) return;

            float lDelta = (float)pDelta;

			direction = direction.Rotated(Mathf.Tau*randCoef*lDelta);

            Move(lDelta);

			if (speed < 0) { Explode(); }

        }

		protected override void Dispose(bool pDisposing)
		{

		}

		private void Move(float pDelta)
		{
			Rotate(direction.Angle() - Rotation);
			Position += speed * pDelta * direction;

			Vector2 lVector = (GetViewportRect().Size / 2 - GlobalPosition);

            GlobalPosition += pullTowardCenter * lVector.Normalized() * Mathf.Pow(lVector.Length(),0.5f) *pDelta;



			speed += acceleration*pDelta;

		}

		private void Explode()
		{
            disableProcess = true;


            ((Node2D)GetNode("Renderer")).Visible = false;

			Node2D lExplosionContainer = (Node2D)GetNode(EXPLOSIONCONTAINER_NODE_PATH);


            int lExplosionIndex = GD.RandRange(0, lExplosionContainer.GetChildCount()-1);

			GpuParticles2D lExplosionPart = (GpuParticles2D)lExplosionContainer.GetChild(lExplosionIndex);

			
			float lRandFloat = GD.Randf();

			((ParticleProcessMaterial)lExplosionPart.ProcessMaterial).Color = Color.FromHsv(lRandFloat,1,1);


			
			lExplosionPart.Emitting = true;
			lExplosionPart.Finished += QueueFree;

		}


		public static FireWork Create(Vector2 pPosition)
		{
			FireWork lNewFireWork = (FireWork)factory.Instantiate();
			lNewFireWork.GlobalPosition = pPosition;

            return lNewFireWork;
		}

        public static void CreateMult(Vector2 pPosition,Node2D pParent)
        {
			FireWork lCurrentFirework;
			int lRandNumberOfFirework = GD.RandRange(MIN_FIREWORK_NUMBER, MAX_FIREWORK_NUMBER); //GERE ICI

			for (int i = 0; i < lRandNumberOfFirework; i++)
			{
				lCurrentFirework = Create(pPosition);

				lCurrentFirework.direction = Vector2.Up.Rotated(Mathf.Tau/3*(lRandNumberOfFirework / 2-i)/ lRandNumberOfFirework);

                pParent.AddChild(lCurrentFirework);
			}
        }

    }
}
