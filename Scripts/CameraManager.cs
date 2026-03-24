using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class CameraManager : Control
	{
		static private CameraManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/CameraManager.tscn");

		[Export] public Camera2D camera;

		private CameraManager():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(CameraManager) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public CameraManager GetInstance()
		{
			if (instance == null) instance = (CameraManager)factory.Instantiate();
			return instance;
		}

		public override void _Ready()
		{
			base._Ready();
			
		}

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;
		}

		public void CenterCameraOnCurrentLevel()
		{
			if (GameManager.GetInstance().currentLevel != null)
			{
                GD.Print(GameManager.GetInstance().currentLevel.Size);

				camera.GlobalPosition = GameManager.GetInstance().currentLevel.Size * States.DISTANCE_RANGE/2;
            }
				
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
