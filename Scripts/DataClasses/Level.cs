using Godot;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class Level 
	{
        /*
         * Only this fields :
         *  -Size;
         *  -playerPosition;
         *  -targetsPos;
         *  -Map;
         *  -bombs;
         *  -bombsPos;
         *  -Par
         *  
         *  are supposed to be read outside the class.
         *  
         *  Any other fields other public fields are public for JSON parse purposed.
         */


        private bool isJSONParse = true;

        public Vector2I Size { get; private set; }
        public Vector2I playerPosition;

		public int Par { get; set; }

        public string Author { get; set; }
        public string Title { get; set; }

        private List<string> map;
		public List<string> Map 
        {
            get { return map; }
            set 
            {
                map = value;
                Size = new Vector2I(map[0].Length, map.Count);

                FillTargetPlayerAndBombPos();
            } 
        }


		public List<Vector2I> targetsPos;

        private List<List<List<int>>> bombExplosionTilesPos;
        public List<List<List<int>>> BombExplosionTilesPos
        { 
            get 
            {
                return bombExplosionTilesPos; 
            }
            set
            {

                if (!isJSONParse)
                {
                    GD.Print("This field is now private !");
                    return;
                }

                isJSONParse = false;

                bombExplosionTilesPos = value;


                bombs = new List<Bomb>();

                foreach (List<List<int>> lListePosition in bombExplosionTilesPos)
                {
                    List<Vector2I> lListeVectorPos = new List<Vector2I>();

                    foreach (List<int> lPos in lListePosition)
                    {
                        lListeVectorPos.Add(new Vector2I(lPos[0], lPos[1]));
                    }

                    bombs.Add(new Bomb(lListeVectorPos));
                }

                if (bombsPos.Count != bombs.Count)
                {
                    GD.Print("WARNING : You have " + bombs.Count + " bombs defined but "+bombsPos.Count+" positions defined !");

                    GD.Print("List of bombs : ");
                    Main.PrintList(bombs);
                    GD.Print("List of positions of bombs : ");
                    Main.PrintList(bombsPos);
                    return;
                }

                
                int lNumberOfBombs = bombs.Count;
                for (int i = 0; i < lNumberOfBombs; i++)
                {
                    bombs[i].indexInLevel = i;
                    indexOfAvalaibleBombs.Add(i);
                }

            }
        }

        public List<Bomb> bombs;

        public List<Vector2I> bombsPos;

        public List<int> indexOfAvalaibleBombs = new List<int>();

        public Bomb currentBomb;


        public Level Duplicate()
        {
            Level lNewLevel = new Level();
            lNewLevel.Size = Size;
            lNewLevel.map = map;
            lNewLevel.bombs = bombs;
            lNewLevel.bombsPos = bombsPos;
            lNewLevel.targetsPos = Main.DuplicateList(targetsPos);
            lNewLevel.currentBomb = currentBomb;
            lNewLevel.indexOfAvalaibleBombs = Main.DuplicateList(indexOfAvalaibleBombs);

            return lNewLevel;
        }

        public Level UpdateMap(List<string> pMap)
        {
            map = pMap;
            return this;
        }

        public override string ToString()
        {
            string lString = "Information sur le niveau : \n";


            lString += "Par : " + Par;

            lString += "\nMap : \n";
            foreach (string lRow in Map)
            {
                lString += lRow + "\n";
            }

            lString += "\nCibles : \n\n";

            foreach (Vector2I lPos in targetsPos)
            {
                lString += lPos + "\n";
            }

            return lString;
        }

        private void FillTargetPlayerAndBombPos()
        {
            bombsPos = new List<Vector2I>();
            targetsPos = new List<Vector2I>();

            int lYSizeOfMap = map.Count;
            int lXSizeOfMap = map[0].Length;

            for (int i = 0; i < lYSizeOfMap; i++)
            {
                for (int j = 0; j < lXSizeOfMap; j++)
                {
                    if (Map[i][j] == (int)ObjectChar.PLAYER)
                    {
                        playerPosition = new Vector2I(j, i);
                        Map[i] = Map[i].Substr(0, j) + " " + Map[i].Substr(j + 1, Map[i].Length);
                    }
                    if (Map[i][j] == (int)ObjectChar.TARGET)
                    {
                        targetsPos.Add(new Vector2I(j, i));
                        Map[i] = Map[i].Substr(0, j) + " " + Map[i].Substr(j + 1, Map[i].Length);
                    }

                    if (Map[i][j] >= '0' && Map[i][j] <= '9')
                    {
                        int lIndex = Map[i][j] - '0';

                        while (bombsPos.Count < lIndex+1)
                        {
                            bombsPos.Add(new Vector2I(0, 0));
                        }
                        bombsPos[lIndex] = new Vector2I(j, i);
                        Map[i] = Map[i].Substr(0,j) + " " + Map[i].Substr(j+1, Map[i].Length);
                    }
                }
            }
        }
    }
}
