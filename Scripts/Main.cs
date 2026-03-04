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
			base._Ready();


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


        public void PrintListOfList<T>(List<List<T>> pListOfList)
        {
            foreach (List<T> lRow in pListOfList)
            {
                string lRes = "";
                foreach (T lCell in lRow)
                {
                    lRes += lCell.ToString();
                }
                GD.Print(lRes);
            }
        }

		public void PrintList<T>(List<T> pList, char pSeparator =';')
		{
			if (pList.Count == 0)
			{
				GD.Print("Liste de type " + typeof(List<T>) + " est vide.");
				return;
			}

            string lRes = "";
            foreach (T lCell in pList)
            {
                lRes += lCell.ToString() + pSeparator;
            }
            GD.Print(lRes);
        }
    }
}
