using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class GridManager 
	{
		static private GridManager instance;

		public Level CurrentLevel
		{
			get;
			private set;
		}

		public const string JSON_PATH = "Levels.json";

		public int levelIndex = 0;

		
        private GridManager():base() 
		{
			if (instance != null)
			{
				GD.Print(nameof(GridManager) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;	
		}

		static public GridManager GetInstance()
		{
			if (instance == null) instance = new GridManager();
			return instance;
		}


		public void ChangeLevel(int pIndex)
		{
			CurrentLevel = GetLevel(pIndex);
			GameManager.GetInstance().CurrentPar = 0;
		}

		private Level GetLevel(int pIndex)
		{
			Level lLevel =	JsonReaderWriter.ReadJsonToList<Level>(JSON_PATH)[pIndex];

			return lLevel;
		}

	}
}
