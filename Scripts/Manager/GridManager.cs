using Godot;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class GridManager 
	{
		static private GridManager instance;



        public int numberOfLevel = JsonReaderWriter.ReadJsonToList<Level>(JSON_PATH).Count;

        public List<int> levelOrder = new List<int>
            {
                0,
                4,
                9,
                11,
                12,
                1,
                2,
                7,
                3,
                10,
                5,
                6,
                8,
            };

        public Level CurrentLevel
		{
			get;
			private set;
		}

		public int CurrentLevelIndex
		{
			get;
			private set;
		}

		public const string JSON_PATH = "Levels.json";
		
		
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
			CurrentLevel = GetLevel(levelOrder[pIndex]);
			CurrentLevelIndex = pIndex;
			GameManager.GetInstance().CurrentPar = 0;
		}

		private Level GetLevel(int pIndex)
		{
			Level lLevel =	JsonReaderWriter.ReadJsonToList<Level>(JSON_PATH)[pIndex];

			return lLevel;
		}
	}
}
