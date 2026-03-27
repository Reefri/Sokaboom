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

            basePosition = Position + Vector2.Up*amplitude;

            GetPosition();
        }

        public override void _Process(double pDelta)
        {


            GetPosition();
        }

        private void GetPosition()
        {
            Position = basePosition + Vector2.Up * amplitude * Mathf.Sin(JuicinessManager.GetInstance().GlobalTime * Mathf.Tau * speed);

        }


    }
}
