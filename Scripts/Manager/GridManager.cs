using Godot;
using System.Collections.Generic;
using GodotDict = Godot.Collections.Dictionary;
using GodotArray = Godot.Collections.Array;
using Godot.Collections;
//using System.IO;
// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class GridManager 
	{
		static private GridManager instance;

		private Level currentLevel;

		private const string JSON_PATH = "res://Json/Levels.json";

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
			currentLevel = GetLevel(pIndex);
		}

		private Level GetLevel(int pIndex)
		{
			Level lLevel =	JsonReader.ReadJson<Level>(JSON_PATH)[pIndex];


			return lLevel;
		}

	}
}
