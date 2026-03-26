using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class HoverEffect : Node2D
	{

		private float amplitude = 25;
		private float speed = 1.5f;


        private Vector2 basePosition;

        public override void _Ready()
        {
            base._Ready();

            basePosition = Position + Vector2.Up*amplitude;

            Position = basePosition + Vector2.Up * amplitude * Mathf.Sin(JuicinessManager.GetInstance().GlobalTime);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            Position = basePosition + Vector2.Up*amplitude*Mathf.Sin(JuicinessManager.GetInstance().GlobalTime);

        }


	}
}
