using Godot;
using System;
using GodotDict = Godot.Collections.Dictionary;


// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class ConnexionManager 
	{
		static private ConnexionManager instance;

        private const string JSON_PATH = "res://Json/Accounts.json";


        private ConnexionManager():base() 
		{
			if (instance != null)
			{
				GD.Print(nameof(ConnexionManager) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public ConnexionManager GetInstance()
		{
			if (instance == null) instance = new ConnexionManager();
			return instance;
		}




	

	}
}
