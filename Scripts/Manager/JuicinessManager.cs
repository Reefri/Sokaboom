using Godot;
using System;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class JuicinessManager : Node2D
	{
		static private JuicinessManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Manager/JuicinessManager.tscn");

		private PackedScene borderExplosion = GD.Load<PackedScene>("res://Scenes/Juiciness/BorderExplosion.tscn");
		private Material bombShadow = (GD.Load<Material>("res://Ressources/Shaders/Materials/BombShadowShader.tres"));
		private const string bombShadowSpeedParameter = "speed";


        private float hoverAmplitude = 20;
        private float hoverSpeed = 0.1f;

        public float GlobalTime { private set; get; } = 0;

		private JuicinessManager():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(JuicinessManager) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public JuicinessManager GetInstance()
		{
			if (instance == null) instance = (JuicinessManager)factory.Instantiate();
			return instance;
		}

		public override void _Ready()
		{
			base._Ready();

			HoverEffect.amplitude = hoverAmplitude;
			HoverEffect.speed = hoverSpeed;

			((ShaderMaterial)bombShadow).SetShaderParameter(bombShadowSpeedParameter, hoverSpeed);

		}

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;

			GlobalTime += lDelta;
		}

		public void ExplodeAllBorders(Vector2 pBorderOriginPos)
		{
			GpuParticles2D lExplosion = (GpuParticles2D)borderExplosion.Instantiate();
			lExplosion.Position = pBorderOriginPos;
			lExplosion.Emitting = true;
			lExplosion.Finished += QueueFree;
			GameManager.GetInstance().AddChild(lExplosion);
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
