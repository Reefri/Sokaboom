using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class LevelScreenShot
	{
		public static LevelScreenShot DEFAULT
		{  get; private set; } = new LevelScreenShot(new Map(), Vector2I.Zero);

		public LevelScreenShot(Map pTileMap, Vector2I pPlayerPosition) 
		{
			tileMap = pTileMap;
			playerPosition = pPlayerPosition;
		}

		public Map tileMap;
		public Vector2I playerPosition;




	}
}
