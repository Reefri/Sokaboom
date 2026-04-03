using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class ToPlaceOnPatterneControl : Control
	{


        public static ToPlaceOnPatterneControl Create(PackedScene pFactory, Vector2 pPosition, Color pColor, float pScale=1)
        {
            ToPlaceOnPatterneControl lPattern = (ToPlaceOnPatterneControl)pFactory.Instantiate();
            lPattern.GlobalPosition = pPosition;
            lPattern.Modulate = pColor;
            lPattern.Scale = new Vector2(pScale, pScale);
            return lPattern;
        }


    }
}
