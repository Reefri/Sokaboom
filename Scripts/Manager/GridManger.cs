using Godot;
using System;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class GridManger
	{
		static private GridManger instance;

		public List<List<char>> levelGrid = new List<List<char>>();

		//public List<Bomb> bombList = new List<Bomb> ();
		public List<(int,int)> targetList = new List<(int,int)> ();


		LevelData level;
	


        private GridManger():base() 
		{
			if (instance != null)
			{
				GD.Print(nameof(GridManger) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public GridManger GetInstance()
		{
			if (instance == null) instance = new GridManger();
			return instance;
		}

		public void GetLevelFromJson(int pIndex)
		{
			

		}

		public bool IsWin()
		{

			foreach ((int,int) lPosition in targetList)
			{
				if (levelGrid[lPosition.Item1][lPosition.Item2] != ObjectChar.BOX) return false;
			}

			int lCountBoxes = 0;

			foreach (List<char> lRow in levelGrid)
			{
				foreach (char lElement in lRow)
				{
					if (lElement == ObjectChar.BOX)
					{
						lCountBoxes++;
					}
				}
			}

			return lCountBoxes == targetList.Count;

		}

		private void ReadJSON(int pLevelIndex)
		{

			LevelData lLevelData;


			//levelData = lLevelData;

		}

	

	}
}
