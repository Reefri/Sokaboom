using Godot;
using System.Transactions;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class VictoryAnimation : Node2D
	{
		private static PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/VictoryAnimation.tscn");

		[Export] private GpuParticles2D particles;

		[Export] private Node2D fireworksParent;

		[Export] private float victoryZoomStrength = 5;
		[Export] private float victoryZoomDuration = 5;

		private Vector2 whereIsWin;

		private Vector2 sideFactor = new Vector2(-150, 0);
		public override void _Ready()
		{
			base._Ready();

			whereIsWin = Player.GetInstance().Position;
            GlobalPosition = whereIsWin;

            particles.Emitting = true;
			fireworksParent.Position += sideFactor;

			CameraManager.GetInstance().Zoom(whereIsWin, victoryZoomStrength, victoryZoomDuration);

			FireWork.CreateMult(whereIsWin, fireworksParent);

			UIManager.GetInstance().instanceHud.undoButton.Disabled = true;
            UIManager.GetInstance().instanceHud.restartButton.Disabled = true;
        }

		public static void Create()
		{
			VictoryAnimation lAnim = (VictoryAnimation)factory.Instantiate();
			GameManager.GetInstance().AddChild(lAnim);
		}

	}
}
