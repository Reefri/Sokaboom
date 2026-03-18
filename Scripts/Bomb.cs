using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

// author : Ethan Frenard

namespace Com.IsartDigital.Sokoban
{
    public partial class Bomb
    {
        private Vector2I explosionOriginPos = new Vector2I(0, 0);

        

        private List<List<int>> explosionMatrix = new List<List<int>> { };

        private int minX = 0;
        private int maxX = 0;

        private int minY = 0;
        private int maxY = 0;

        private Vector2I gridSize = new Vector2I(0, 0);
        

        public Bomb(List<Vector2I> lExplosionTilesPos)
        {
            GetMinMax(lExplosionTilesPos);
            GetGridSize();
            CreateExplosionMatrice();
            PutExplosionInMatrix(lExplosionTilesPos);
        }
       

        private void CreateExplosionMatrice()
        {
            for (int i = 0; i < gridSize.Y; i++)
            {
                List<int> lRow = new List<int>();

                for (int j = 0; j < gridSize.X; j++)
                {
                    lRow.Add(0);
                }

                explosionMatrix.Add(lRow);
            }
        }

        private void PutExplosionInMatrix(List<Vector2I> pExplosionTilePos)
        {
            Vector2I lOriginPos = new Vector2I(Mathf.Abs(minX), Mathf.Abs(minY));

            foreach (Vector2I lPos in pExplosionTilePos)
            {
                explosionMatrix[lOriginPos.Y + lPos.Y][lOriginPos.X + lPos.X] = 1;
            }

            explosionMatrix[lOriginPos.Y][lOriginPos.X] = 2;

        }

        private void GetGridSize()
        {
            gridSize = new Vector2I(1, 1) - new Vector2I(minX, minY) + new Vector2I(maxX, maxY);
        }

        private void GetMinMax(List<Vector2I> pList)
        {
            foreach (Vector2I lPos in pList)
            {
                if (lPos.X < minX) minX = lPos.X;
                if (lPos.X > maxX) maxX = lPos.X;

                if (lPos.Y < minY) minY = lPos.Y;
                if (lPos.Y > maxY) maxY = lPos.Y;
            }
        }

        public void Explode(Vector2I pPosition, Vector2I pRotationVector)
        {
            BombPattern.Create(explosionMatrix,pPosition, pRotationVector);
            //GD.Print(this);
        }


        public override string ToString()
        {
            string lRes = "";

            foreach (List<int> lRow in explosionMatrix)
            {
                string lRowRes = "";
                foreach (int lCell in lRow)
                {
                    lRowRes += lCell + ",";
                }
                lRes += lRowRes + "\n";
            }

            return lRes;
        }
    }
}
