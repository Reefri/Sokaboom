using Com.IsartDigital.Utils.Effects;
using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class ColorBlind : CanvasLayer
	{
		[Export] private ColorRect colorRect;

        private ShaderMaterial material;

        private const string COLOR_NAME = "Color";

        private static ColorBlind instance;

        private static PackedScene factory = (PackedScene)GD.Load("res://Scenes/Accessibilite/ColorBlind.tscn");

        public ColorBlind()
        {
            if (instance != null)
            {
                GD.Print("An instance of "+typeof(ColorBlind)+" already exist. \nDeleting the last one added");    
                QueueFree();
                return;
            }
            instance = this;

        }

        public static ColorBlind GetInstance()
        {
            if (instance == null) { instance = (ColorBlind)factory.Instantiate(); }
            return instance;
        }

        public override void _Ready()
        {
            base._Ready();
            material = (ShaderMaterial)colorRect.Material;
        }

        public void SetShaderColor(Color pColor)
        {
            material.SetShaderParameter("Color", pColor);
        }

    }
}
