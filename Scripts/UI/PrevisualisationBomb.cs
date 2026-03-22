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

        public static void CreateInstance(List<List<int>> pExplosionMatrix)
        {
            PrevisualisationBomb lPrevisualisationBomb = (PrevisualisationBomb)factory.Instantiate();
            lPrevisualisationBomb.explosionMatrix = pExplosionMatrix;
            UIManager.GetInstance().AddChild(lPrevisualisationBomb);
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

            for (int i = 0; i < explosionMatrix.Count; i++)
            {
                for (int j = 0; j < explosionMatrix[i].Count; j++)
                {
                    if (explosionMatrix[i][j] == 2)
                    {
                        originPos = new Vector2I(j, i);
                        AddChild(ToPlaceOnExplosion.Create(GlobalPosition, new Color(1, 0, 0), true));
                    }
                }
            }

            for (int i = 0; i < explosionMatrix.Count; i++)
            {
                for (int j = 0; j < explosionMatrix[i].Count; j++)
                {

                    if (explosionMatrix[i][j] == 1)
                    {
                        Vector2 lPosition = GlobalPosition + (new Vector2(j, i) - originPos) * States.DISTANCE_RANGE;
                        AddChild(ToPlaceOnExplosion.Create(lPosition, new Color(1, 1, 1), true));
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
