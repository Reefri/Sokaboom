using Godot;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class Level 
	{
		public int Par { get; set; }

        private List<string> map;
		public List<string> Map 
        {
            get { return map; }
            set 
            {
                map = value;
                targets = GetTarget();
            } 
        }

		public List<(int,int)> targets;

		public Level() { }

        public override string ToString()
        {
            string lString = "Par : " + Par;

            lString += "\nMap : \n";
            foreach (string l in Map)
            {
                lString += l + "\n";
            }

            lString += "\nCible : \n\n";

            foreach ((int, int) lPos in targets)
            {
                lString += lPos + "\n";
            }

            return lString;

        }

        public List<(int, int)> GetTarget()
        {
            List<(int, int)> lTargetPos = new List<(int, int)>();

            for (int i = 0; i < Map.Count; i++)
            {
                for (int j = 0; j < Map[i].Length; j++)
                {
                    if (Map[i][j] == ObjectChar.TARGET)
                    {
                        lTargetPos.Add((i, j));
                    }
                }
            }

            return lTargetPos;
        }
    }
}
