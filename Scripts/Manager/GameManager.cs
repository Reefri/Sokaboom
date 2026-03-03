using Godot;
using System;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class GameManager : Node
	{
		static private GameManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/GameManager.tscn");



		private GameManager():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(GameManager) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public GameManager GetInstance()
		{
			if (instance == null) instance = (GameManager)factory.Instantiate();
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
