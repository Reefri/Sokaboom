using Godot;
using System.Collections.Generic;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class PrevisualisationBomb : Control
	{
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/PrevisualisationBomb.tscn");

        [Export] Vector2 ajustement;

        public static void CreateInstance(List<List<int>> pExplosionMatrix)
        {
            PrevisualisationBomb lPrevisualisationBomb = (PrevisualisationBomb)factory.Instantiate();
            lPrevisualisationBomb.explosionMatrix = pExplosionMatrix;
            UIManager.GetInstance().AddChild(lPrevisualisationBomb);
        }

        [Export] Label arrow;
        
        private Vector2I originPos;
        public List<List<int>> explosionMatrix;
        private static PackedScene toPlaceOnExplosion = GD.Load<PackedScene>("res://Scenes/ToPlaceOnExplosions.tscn");
        
        public override void _Ready()
		{
			base._Ready();

            Node2D lNode = new Node2D();

            new BombPattern(
                lNode,
                explosionMatrix,
                BombPattern.EnumOfExplosionPattern.Mouse,
                true,
                null,
                2,
                2
                );
            lNode.GlobalPosition = arrow.GlobalPosition * 2 - ajustement;


            AddChild(lNode);
        }
	}
}
