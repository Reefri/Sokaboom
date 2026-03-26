using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class JuicinessManager : Node2D
	{
		static private JuicinessManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Manager/JuicinessManager.tscn");


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
		}

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;

			GlobalTime += lDelta;
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
