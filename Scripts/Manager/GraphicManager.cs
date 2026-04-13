using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class GraphicManager
	{

		private static bool isOld = false;
		public static bool IsOld 
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
        private static Texture2D newGraphics = (Texture2D)GD.Load("res://Assets/NewAssets/Holy_SpriteSheet.png");


		private static Texture2D oldBox = (Texture2D)GD.Load("res://Assets/kenney_sokoban-pack/PNG/Default_size/Crates/crate_02.png");//NE PAS TOUCHER
        private static Texture2D newBox = (Texture2D)GD.Load("res://Assets/NewAssets/Box_Alone.png");


		private static Texture2D oldWall = (Texture2D)GD.Load("res://Assets/Wall.png");
		private static Texture2D newWall = (Texture2D)GD.Load("res://Assets/NewAssets/Wall_Alone.png");

		private static Texture2D oldTarget = (Texture2D)GD.Load("res://Assets/Target.png");
		private static Texture2D newTarget = (Texture2D)GD.Load("res://Assets/NewAssets/Target_Alone.png");




		public static void Update()
		{
			((TileSetAtlasSource)theHolyTileSet.GetSource(0)).Texture = (IsOld ? oldGraphics : newGraphics);

			Player.GetInstance().UpdateTexture(IsOld);
            Box.texture = (IsOld ? oldBox : newBox);
			Box.UpdateTexture();
			Wall.texture = (IsOld ? oldWall : newWall);
			Target.texture = (IsOld ? oldTarget : newTarget);

        }

        public static void ToggleGraphics()
		{
			IsOld = !IsOld;
		}

		

		
	}
}
