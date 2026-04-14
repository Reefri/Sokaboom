using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class MiniCloud : Control
	{
		private float radius;
		private const float MIN_RADIUS = 50;
		private const float MAX_RADIUS = 200;

		private float rotationSpeed;
		private const float MIN_ROTATION_SPEED = 5.0f;
		private const float MAX_ROTATION_SPEED = 30.0f;

		private int rotationDirection;

		private float offSet;

		private Vector2 basePosition;

		private float timeProgression = 0;

		public override void _Ready()
		{
			rotationDirection = GD.Randf() > 0.5f ? 1:-1;

			radius = MIN_RADIUS + GD.Randf() * (MAX_RADIUS - MIN_RADIUS);
			rotationSpeed = MIN_ROTATION_SPEED + GD.Randf()*(MAX_ROTATION_SPEED-MIN_ROTATION_SPEED);

			offSet = Mathf.Tau * GD.Randf();

			basePosition = Position;

			GD.Print(radius);
			GD.Print(rotationSpeed);
			GD.Print(offSet);

			Position = basePosition + (new Vector2(
				Mathf.Cos(offSet),Mathf.Sin(offSet)
				)) * radius;

		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			timeProgression += lDelta;

            Position = basePosition + (new Vector2(
				Mathf.Cos(offSet + rotationSpeed*timeProgression),Mathf.Sin(offSet + rotationSpeed * timeProgression)
				)) * radius;

			GD.Print((new Vector2(
                Mathf.Cos(offSet + rotationSpeed * timeProgression), Mathf.Sin(offSet + rotationSpeed * timeProgression)
                )) * radius);

		}

		
	}
}
