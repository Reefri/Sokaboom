using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public abstract partial class ToPlaceOnPatterneNodeTwoD : Node2D 
	{
		public static ToPlaceOnPatterneNodeTwoD Create(PackedScene pFactory,Vector2 pPosition, Color pColor, float pScale=1)
		{
            ToPlaceOnPatterneNodeTwoD lPattern = (ToPlaceOnPatterneNodeTwoD)pFactory.Instantiate();
            lPattern.GlobalPosition = pPosition;
            lPattern.Modulate = pColor;
            lPattern.Scale = new Vector2(pScale, pScale);
            return lPattern;
        }


	}
}
