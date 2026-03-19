using Godot;
using System;
using System.Collections.Generic;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class PrevisualisationBomb : Control
	{
		static public PrevisualisationBomb instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/previsualisation_bomb.tscn");

        public static void CreateInstance()
        {
            PrevisualisationBomb lPrevisualisationBomb = (PrevisualisationBomb)factory.Instantiate();
            Main.GetInstance().AddChild(lPrevisualisationBomb); //UIManager.GetInstance().AddChild(lPrevisualisationBomb);
        }

        private PrevisualisationBomb():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(PrevisualisationBomb) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public PrevisualisationBomb GetInstance()
		{
			if (instance == null) instance = (PrevisualisationBomb)factory.Instantiate();
			return instance;
		}

        
        private Vector2I originPos;
        public List<List<int>> explosionMatrix;
        private static PackedScene toPlaceOnExplosion = GD.Load<PackedScene>("res://Scenes/ToPlaceOnExplosions.tscn");
        
        public override void _Ready()
		{
			base._Ready();

            GlobalPosition = new Vector2(1152, 324) / 2; //à redesigner plus tard

            explosionMatrix = Main.GetInstance().explosionMatrix;

            for (int i = 0; i < explosionMatrix.Count; i++)
            {
                for (int j = 0; j < explosionMatrix[i].Count; j++)
                {
                    if (explosionMatrix[i][j] == 2)
                    {
                        originPos = new Vector2I(j, i);

                        Node2D lPattern = (Node2D)toPlaceOnExplosion.Instantiate();
                        lPattern.Position = Position;
                        lPattern.Modulate = new Color(1, 0, 0);
                        AddChild(lPattern);

                    }
                }
            }

            for (int i = 0; i < explosionMatrix.Count; i++)
            {
                for (int j = 0; j < explosionMatrix[i].Count; j++)
                {

                    if (explosionMatrix[i][j] == 1)
                    {

                        Node2D lPattern = (Node2D)toPlaceOnExplosion.Instantiate();
                        lPattern.Position = Position + (new Vector2(j, i) - originPos) * States.DISTANCE_RANGE;
                        AddChild(lPattern);
                    }
                }
            }
        }
        

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
