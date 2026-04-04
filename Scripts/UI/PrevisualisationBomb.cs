using Godot;
using System;
using System.Collections.Generic;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class PrevisualisationBomb : Control
	{
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/previsualisation_bomb.tscn");

        public static void CreateInstance(List<List<int>> pExplosionMatrix)
        {
            PrevisualisationBomb lPrevisualisationBomb = (PrevisualisationBomb)factory.Instantiate();
            lPrevisualisationBomb.explosionMatrix = pExplosionMatrix;
            UIManager.GetInstance().AddChild(lPrevisualisationBomb);
        }
        
        private Vector2I originPos;
        public List<List<int>> explosionMatrix;
        private static PackedScene toPlaceOnExplosion = GD.Load<PackedScene>("res://Scenes/ToPlaceOnExplosions.tscn");
        
        public override void _Ready()
		{
			base._Ready();

            GlobalPosition = new Vector2(1152, 324) / 2; //à redesigner plus tard

            Node2D lNode = new Node2D();



            new BombPattern(
                lNode,
                explosionMatrix,
                BombPattern.EnumOfExplosionPattern.Mouse
                );
            lNode.GlobalPosition = GlobalPosition;


            AddChild(lNode);

        }
	}
}
