using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class Main : Node2D
	{
		static private Main instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Main.tscn");



		private Main():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(Main) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public Main GetInstance()
		{
			if (instance == null) instance = (Main)factory.Instantiate();
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

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
