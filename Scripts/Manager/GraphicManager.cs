using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class GraphicManager
	{

		private static bool isOld = false;
		private static bool IsOld 
		{
			get {return isOld;}
			set 
			{
				isOld = value;
				Update();
			}
		}

		private static TileSet theHolyTileSet = (TileSet)GD.Load("res://Ressources/TopDown.tres"); //NE PAS TOUCHER



		private static Texture2D oldGraphics = (Texture2D)GD.Load("res://Assets/sokoban_tilesheet.png");//NE PAS TOUCHER
        private static Texture2D newGraphics = (Texture2D)GD.Load("res://Assets/kenney_sokoban-pack/Spritesheet/sokoban_spritesheet.png");


		private static Texture2D oldBox = (Texture2D)GD.Load("res://Assets/kenney_sokoban-pack/PNG/Default_size/Crates/crate_02.png");//NE PAS TOUCHER
        private static Texture2D newBox = (Texture2D)GD.Load("res://Assets/kenney_sokoban-pack/PNG/Default_size/Crates/crate_01.png");




		
		public static void Update()
		{
			((TileSetAtlasSource)theHolyTileSet.GetSource(0)).Texture = (IsOld ? oldGraphics : newGraphics);

			Player.GetInstance().UpdateTexture(IsOld);
            Box.texture = (IsOld ? oldBox : newBox);
			Box.UpdateTexture();

        }

        public static void ToggleGraphics()
		{
			IsOld = !IsOld;
		}

		

		
	}
}
