using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class Main : Node2D
	{
		static private Main instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Main.tscn");

		[Export] private bool testOnlyGameFeature = true;
		[Export(PropertyHint.Range, "0, 12")] private int levelAtTest;

        private Main():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(Main) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public Main GetInstance()
		{
			if (instance == null) instance = (Main)factory.Instantiate();
			return instance;
		}

		public override void _Ready()
		{
			//GridManager.GetInstance().ChangeLevel(0);
			//GD.Print(GridManager.GetInstance().CurrentLevel);
			base._Ready();

			//Bomb bomb = GridManager.GetInstance().CurrentLevel.bombs[0];
			//bomb.Explode(new Vector2I(3, 3));

			if (testOnlyGameFeature)
			{
                UIManager.GetInstance().levelIndex = levelAtTest;
                AddChild(GameManager.GetInstance());
                RemoveChild(UIManager.GetInstance());
            }
        }



        public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}


		public List<T> DuplicateList<T>(List<T> pList)
		{
			List<T> lList = new List<T>();
			foreach (T lElement in pList)
			{
				lList.Add(lElement);
			}
			return lList;
		}

		public List<List<T>> DuplicateListOfList<T>(List<List<T>> pListOfList)
		{
			List<List<T>> lList = new List<List<T>>();
			foreach (List<T> lElement in pListOfList)
			{
				lList.Add(DuplicateList(lElement));
			}
			return lList;
		}

        public void PrintListOfList<T>(List<List<T>> pListOfList, char pSeparator = ';', bool pDoLigneBreak=true)
        {
			string lRes = "";
			string lTempRes;

            foreach (List<T> lRow in pListOfList)
            {
                lTempRes = "";
                foreach (T lCell in lRow)
                {
                    lRes += lCell.ToString()+pSeparator;
                }
				lRes += lTempRes + (pDoLigneBreak?"\n":"");
            }
			GD.Print(lRes);
        }

		public void PrintList<T>(List<T> pList, char pSeparator = ';', bool pDoLigneBreak = false)
		{
			if (pList.Count == 0)
			{
				GD.Print("Liste de type " + typeof(List<T>) + " est vide.");
				return;
			}

            string lRes = "";
            foreach (T lCell in pList)
            {
                lRes += lCell.ToString() + pSeparator + (pDoLigneBreak?"\n":"");
            }
            GD.Print(lRes);
        }
    }
}
