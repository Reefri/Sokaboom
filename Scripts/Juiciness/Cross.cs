using Godot;
using System;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban 
{
	public partial class Cross : Area2D
	{
		static private Cross instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Juiciness/Cross.tscn");
		[Export] private AnimationPlayer animations;

		private Cross():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(Cross) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public Cross GetInstance()
		{

			if (instance != null)
			{
				instance.QueueFree();
				//instance = null;
			}
			
			instance = (Cross)factory.Instantiate();
			return instance;
		}

		public static Cross Create(Vector2I pCrossPosition, Node pParent)
		{
			Cross lCross = GetInstance();
			lCross.Position = pCrossPosition;

			pParent.AddChild(lCross);
			return lCross;

		}

		public override void _Ready()
		{
			base._Ready();
			animations.Play("apparition");
		}

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;
			if (!animations.IsPlaying())
			{
				animations.Play("movingOnPath");
			}
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
