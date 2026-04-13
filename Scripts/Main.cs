using Godot;
using System.Collections.Generic;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class Main : Node2D
	{

        [Export] public Node postProcessingNode;

		static private Main instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Main.tscn");

        [Export] public bool noLogin = true;

        public Vector2 screenSize;

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

        public override void _Process(double delta)
        {
            base._Process(delta);
            screenSize = GetViewportRect().Size;

            if (Input.IsActionJustPressed("SwapGraphics"))
            {
                GraphicManager.ToggleGraphics();
            }
        }
		static public Main GetInstance()
		{
			if (instance == null) instance = (Main)factory.Instantiate();
			return instance;
		}

        


		public override void _Ready()
		{
			base._Ready();

            GraphicManager.Update();
        }

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}

		public static List<T> DuplicateList<T>(List<T> pList)
		{
			List<T> lList = new List<T>();
			foreach (T lElement in pList)
			{
				lList.Add(lElement);
			}
			return lList;
		}

		public static List<List<T>> DuplicateListOfList<T>(List<List<T>> pListOfList)
		{
			List<List<T>> lList = new List<List<T>>();
			foreach (List<T> lElement in pListOfList)
			{
				lList.Add(DuplicateList(lElement));
			}
			return lList;
		}

        public static void PrintListOfList<T>(List<List<T>> pListOfList, char pSeparator = ';', bool pDoLigneBreak=true)
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

		public static void PrintList<T>(List<T> pList, char pSeparator = ';', bool pDoLigneBreak = false)
		{
			if (pList.Count == 0)
			{
				GD.Print("List of type " + typeof(T) + " is empty.");
				return;
			}

            string lRes = "";
            foreach (T lCell in pList)
            {
                lRes += lCell.ToString() + pSeparator + (pDoLigneBreak?"\n":"");
            }
            GD.Print(lRes);
        }


        public static List<List<int>> RotateMatrix(List<List<int>> pMatrix, Vector2I pDirection)
        {

            List<List<int>> lDuplicatedMatrix = DuplicateListOfList(pMatrix);

            if (pDirection == Vector2I.Up)
            {
                return lDuplicatedMatrix;

            }

            else if (pDirection == Vector2I.Down)
            {
                foreach (List<int> pRow in lDuplicatedMatrix)
                {
                    pRow.Reverse();
                }
                lDuplicatedMatrix.Reverse();

                return lDuplicatedMatrix;
            }


            else if (pDirection == Vector2I.Right)
            {
                lDuplicatedMatrix.Reverse();

                List<List<int>> lResMatrix = new List<List<int>>();

                int lYSizeOfMatrix = pMatrix[0].Count;
                int lXSizeOfMatrix = pMatrix.Count;

                for (int i = 0; i < lYSizeOfMatrix; i++)
                {
                    List<int> lCollumn = new List<int>();
                    for (int j = 0; j < lXSizeOfMatrix; j++)
                    {
                        lCollumn.Add(lDuplicatedMatrix[j][i]);

                    }
                    lResMatrix.Add(lCollumn);
                }

                return lResMatrix;
            }

            else if (pDirection == Vector2I.Left)
            {
                foreach (List<int> pRow in lDuplicatedMatrix)
                {
                    pRow.Reverse();
                }

                List<List<int>> lResMatrix = new List<List<int>>();


                int lYSizeOfMatrix = lDuplicatedMatrix[0].Count;
                int lXSizeOfMatrix = lDuplicatedMatrix.Count;

                for (int i = 0; i < lYSizeOfMatrix; i++)
                {
                    List<int> lCollumn = new List<int>();
                    for (int j = 0; j < lXSizeOfMatrix; j++)
                    {
                        lCollumn.Add(lDuplicatedMatrix[j][i]);

                    }
                    lResMatrix.Add(lCollumn);

                }

                return lResMatrix;
            }

            GD.Print("Selected direction doesn't match : "+pDirection);

            return lDuplicatedMatrix;
        }
    }
}
