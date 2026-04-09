using Godot;
using System.Transactions;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class VictoryAnimation : Node2D
	{
		private static PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/VictoryAnimation.tscn");

		[Export] private GpuParticles2D particles;

		[Export] private float victoryZoomStrength = 5;
		[Export] private float victoryZoomDuration = 3;

		private Vector2 whereIsWin;
		public override void _Ready()
		{
			base._Ready();

			whereIsWin = Player.GetInstance().Position;

			particles.Position = whereIsWin;
			particles.Emitting = true;

			CameraManager.GetInstance().Zoom(whereIsWin, victoryZoomStrength, victoryZoomDuration);
		}

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;

		}

		public static void Create()
		{
			VictoryAnimation lAnim = (VictoryAnimation)factory.Instantiate();
			UIManager.GetInstance().AddSibling(lAnim);
		}

	}
}
