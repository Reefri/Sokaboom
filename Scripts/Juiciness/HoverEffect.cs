using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class HoverEffect : Node2D
	{

		public static float amplitude = 20;
		public static float speed = 0.1f;


        private Vector2 basePosition;

        public override void _Ready()
        {
            base._Ready();

            basePosition = Position + Vector2.Up * amplitude;

            SetPosition();
        }

        public override void _Process(double pDelta)
        {


            SetPosition();
        }

        private void SetPosition()
        {
            Position = basePosition + Vector2.Up * amplitude * Mathf.Sin( GlobalPosition.X/States.DISTANCE_RANGE * Mathf.Tau/3+JuicinessManager.GetInstance().GlobalTime * Mathf.Tau * speed);

        }


    }
}
