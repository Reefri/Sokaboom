using Godot;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombCollectiblePatron
	{
		public Bomb bomb;
		public Vector2I positionInGrid;

		public Color mainColor;
		public Color secondaryColor;


        private static float mainColorSaturation = 0.8f;
        private static float mainColorValue = 0.8f;

        public static float secondaryColorSaturation = 0.4f;
        public static float secondaryColorValue = 0.4f;


        private static List<string> bodyTextureChoices = new List<string>()
        {
            "bubble",
            "diamond",
            "star",
            "triangle",
            "wave"
        };

        public string texturePath;


        public BombCollectiblePatron(Bomb pBomb, Vector2I pPosition)
		{
			bomb = pBomb;
			positionInGrid = pPosition;


            mainColor = Color.FromHsv(GD.Randf(), mainColorSaturation, mainColorValue);
            secondaryColor = Color.FromHsv(GD.Randf(), secondaryColorSaturation, secondaryColorValue);

            texturePath = bodyTextureChoices[GD.RandRange(0, bodyTextureChoices.Count - 1)];

        }
    }
}
