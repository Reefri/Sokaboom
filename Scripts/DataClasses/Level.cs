using Godot;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class Level 
	{
        /*
         * Seuls les champs :
         *  -Size;
         *  -playerPosition;
         *  -targetsPos;
         *  -Map;
         *  -bombs;
         *  -bombsPos;
         *  -Par
         *  
         *  sont destinés à être lue en dehors de la classe.
         *  
         *  Les autres champs public le sont pour des raisons de parse avec le JSON.
         */


        private bool isJSONParse = true;

        public Vector2I Size { get; private set; }
        public Vector2I playerPosition;

		public int Par { get; set; }

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
                    GD.Print("Ce champs et à présent en privé !");
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
                    GD.Print("WARNING : Vous avez " + bombs.Count + " bombes définies pour "+bombsPos.Count+" positions définies !");

                    GD.Print("Liste des bombes : ");
                    Main.GetInstance().PrintList(bombs);
                    GD.Print("Liste des positions de bombes : ");
                    Main.GetInstance().PrintList(bombsPos);
                }



            }
        }

        public List<Bomb> bombs;

        public List<Vector2I> bombsPos;


        public Level Duplicate()
        {
            Level lNewLevel = new Level();
            lNewLevel.Size = Size;
            lNewLevel.map = map;
            lNewLevel.bombs = bombs;
            lNewLevel.bombsPos = bombsPos;
            lNewLevel.targetsPos = targetsPos;

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

            lString += "\nCible : \n\n";

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

            for (int i = 0; i < Map.Count; i++)
            {
                for (int j = 0; j < Map[i].Length; j++)
                {
                    if (Map[i][j] == (int)ObjectChar.PLAYER)
                    {
                        playerPosition = new Vector2I(j, i);
                        Map[i] = Map[i].Substr(0, j) + " " + Map[i].Substr(j + 1, Map[i].Length);
                    }
                    if (Map[i][j] == (int)ObjectChar.TARGET)
                    {
                        targetsPos.Add(new Vector2I(j, i));
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
