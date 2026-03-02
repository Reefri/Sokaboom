using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

// author : Ethan Frenard

namespace Com.IsartDigital.Sokoban
{
    public partial class Bomb : Node2D
    {
        private Vector2I explosionOriginPos = new Vector2I(0, 0);

        private List<Vector2I> explosionTilesPos = new List<Vector2I>
        {
            new Vector2I(1, 0),
            new Vector2I(2, 0),
            new Vector2I(0, -1),
            new Vector2I(0, -2),
            new Vector2I(0, -3),
        };

        private List<List<int>> explosionMatrix = new List<List<int>> { };

        private int minX = 0;
        private int maxX = 0;

        private int minY = 0;
        private int maxY = 0;

        private Vector2I gridSize = new Vector2I(0, 0);
        public override void _Ready()
        {
            Explosion();
        }

        public override void _Process(double pDelta)
        {
            float lDelta = (float)pDelta;
        }

        private void Explosion()
        {
            GetMinMax(explosionTilesPos);
            GetGridSize();
            CreateExplosionMatrice();
            PutExplosionInMatrix();
            PrintListOfList(explosionMatrix);
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

        private void PutExplosionInMatrix()
        {
            Vector2I lOriginPos = new Vector2I(Mathf.Abs(minX), Mathf.Abs(minY));
            //GD.Print(lOriginPos);

            //GD.Print(explosionMatrice[lOriginPos.X][lOriginPos.Y]);

            explosionMatrix[lOriginPos.Y][lOriginPos.X] = 2;

            foreach (Vector2I lPos in explosionTilesPos)
            {
                explosionMatrix[lOriginPos.Y + lPos.Y][lOriginPos.X + lPos.X] = 1;
            }
        }

        private void GetGridSize()
        {
            gridSize = new Vector2I(1, 1) - new Vector2I(minX, minY) + new Vector2I(maxX, maxY);

            GD.Print("The minimum size of the grid is " + gridSize);
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

            GD.Print("\nmin x : " + minX, "\nmax x : " + maxX, "\nmin y : " + minY, "\nmax y : " + maxY);

        }

        public void PrintListOfList<T>(List<List<T>> pListOfList)
        {
            foreach (List<T> lRow in pListOfList)
            {
                string lRes = "\n";
                foreach (T lCell in lRow)
                {
                    lRes += lCell.ToString() + " ";
                }
                GD.Print(lRes);
            }
        }
        protected override void Dispose(bool pDisposing)
        {

        }
    }
}
