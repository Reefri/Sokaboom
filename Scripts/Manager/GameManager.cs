using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban
{
    public partial class GameManager : Node2D
    {
        [Export] public Node2D bombCollectibleContainer;

        static private GameManager instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/GameManager.tscn");

        private PackedScene bombCollectible = GD.Load<PackedScene>("res://Scenes/BombCollectible.tscn");


        public Level currentLevel;

        private List<LevelScreenShot> gameScreenshot = new List<LevelScreenShot>();
        public HistoricHeap currentPosition;


        private int currentPar = 0;
        public int CurrentPar
        {
            get { return currentPar;  }
            set 
            { 
                currentPar = value; 
                UIManager.GetInstance().UpdateHud();
            }
        }
            

        private List<Vector2I> neighborsCoor = new List<Vector2I>
        {
            Vector2I.Right,
            Vector2I.Left,
            Vector2I.Up,
            Vector2I.Down,
        };

        public Dictionary<ObjectChar, Vector2I> objectPositionOnTileSet = new Dictionary<ObjectChar, Vector2I>
        {
            { ObjectChar.BOX , new Vector2I(6,0) },
            { ObjectChar.WALL , new Vector2I(7,7) },
            { ObjectChar.TARGET , new Vector2I(1,3) },
            { ObjectChar.EMPTY , new Vector2I(11,6) },
            { ObjectChar.BORDER, new Vector2I(9,0) },
        };

        public Map tileMap;

        private GameManager() : base()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(GameManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }

        static public GameManager GetInstance()
        {
            if (instance == null) instance = (GameManager)factory.Instantiate();
            return instance;
        }

        public override void _Ready()
        {
            GridManager.GetInstance().ChangeLevel(UIManager.GetInstance().levelIndex);
            currentLevel = GridManager.GetInstance().CurrentLevel;

            UIManager.GetInstance().uiHUD.ResetHUD();

            GD.Print(currentLevel.Title + " : " + currentLevel.Author);

            currentPosition = new HistoricHeap(currentLevel);

            tileMap = Map.Create();

            AddChild(tileMap);
            AddChild(Player.GetInstance());

            ChargeMapFromCurrentLevel();

        }


        public override void _Process(double pDelta)
        {
            base._Process(pDelta);
            float lDelta = (float)pDelta;

            if (Input.IsActionJustPressed("TimeMinus"))
            {
                MoveBackInTime();
            }
            if (Input.IsActionJustPressed("TimePlus"))
            {
                MoveForwardInTime();

            }
        }

        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }

           
        

        private void ChargeMapFromCurrentLevel()
        {
            tileMap.Clear();

            for (int i = 0; i < currentPosition.value.Size.Y; i++)
            {
                for (int j = 0; j < currentPosition.value.Size.X; j++)
                {

                    switch (currentPosition.value.Map[i][j])
                    {
                        case (char)ObjectChar.EMPTY:
                            continue;

                        case (char)ObjectChar.BORDER:
                            tileMap.SetCell((int)Map.LevelLayer.Ground, new Vector2I(j, i), 0, objectPositionOnTileSet[ObjectChar.EMPTY]);
                            break;
                    }


                    tileMap.SetCell((int)Map.LevelLayer.Playground, new Vector2I(j, i), 0,
                            objectPositionOnTileSet[(ObjectChar)currentPosition.value.Map[i][j]]
                            );

                }
            }

            foreach (Vector2I lTargetPos in currentPosition.value.targetsPos)
            {
                tileMap.SetCell((int)Map.LevelLayer.Target, lTargetPos, 0, objectPositionOnTileSet[ObjectChar.TARGET]);
            }



            foreach (Node2D lBombCollectible in bombCollectibleContainer.GetChildren())
            {
                lBombCollectible.QueueFree();
            }

            foreach(int lIndex in currentPosition.value.indexOfAvalaibleBombs)
            {
                BombCollectible.Create(currentPosition.value.bombs[lIndex], currentPosition.value.bombsPos[lIndex], lIndex);
            }


            Vector2I lPlayerPosition = currentPosition.value.playerPosition;

            Player.GetInstance().GoTo(lPlayerPosition);
            Player.GetInstance().GiveBombToPlayer(currentPosition.value.currentBomb);

            FillGroundTiles(lPlayerPosition);
        }

        private void FillGroundTiles(Vector2I pStartCoor)
        {
            tileMap.SetCell((int)Map.LevelLayer.Ground, pStartCoor, 0, objectPositionOnTileSet[ObjectChar.EMPTY]);

            foreach (Vector2I lNeighbor in neighborsCoor)
            {
                if (tileMap.GetCellTileData((int)Map.LevelLayer.Ground, pStartCoor + lNeighbor) == null)
                {
                    FillGroundTiles(pStartCoor + lNeighbor);
                }
            }
        }

        public void RemoveBomb(Bomb pBomb)
        {
            RemoveBombAtIndex(currentPosition.value.bombs.IndexOf(pBomb));
        }

        public void RemoveBombAtIndex(int lIndex)
        {
            currentPosition.value.indexOfAvalaibleBombs.Remove(lIndex);
        }

        private Level SaveMapAsLevel()
        {
            List<string> lRes = new List<string>();
            string lRow;

            for (int i = 0; i < currentPosition.value.Size.Y; i++)
            {
                lRow = "";
                for (int j = 0; j < currentPosition.value.Size.X; j++)
                {

                    if (tileMap.GetCellTileData((int)Map.LevelLayer.Playground, new Vector2I(j, i)) == null)
                    {
                        lRow += (char)ObjectChar.EMPTY;
                        continue;
                    }


                    if ((bool)tileMap.GetCellTileData((int)Map.LevelLayer.Playground, new Vector2I(j, i)).GetCustomData(Map.PLAY_OBJECT))
                    {
                        foreach (string lKey in Map.interactableToObjectChar.Keys)
                        {
                            if ((bool)tileMap.GetCellTileData((int)Map.LevelLayer.Playground, new Vector2I(j, i)).GetCustomData(lKey))
                            {
                                lRow += (char)Map.interactableToObjectChar[lKey];
                                break;
                            }
                        }
                    }
                    else
                    {
                        lRow += (char)ObjectChar.EMPTY;

                        GD.Print("Unknow cell custom data. Please fill missing data in the Map class : " + new Vector2I(j, i));
                    }

                }
                lRes.Add(lRow);

            }

            Level lNewLevel = currentPosition.value.Duplicate();

            lNewLevel.UpdateMap(lRes);
            lNewLevel.playerPosition = Player.GetInstance().GetPositionToVector2I();

            return lNewLevel;
        }

        public void SaveBombs()
        {
            if (currentPosition.previousValue == null) return;

            currentPosition.previousValue.value.bombsPos = currentPosition.value.bombsPos;
            currentPosition.previousValue.value.bombs = currentPosition.value.bombs;
        }

        public void UpdateAfterAction()
        {

            CurrentPar++;
            SaveScreenshotGame();
            if (CheckWin())
            {
                GD.Print("You won !");
            }

        }


        public void UpdateCurrentPosition()
        {
            currentPosition.value = GetScreenshotGame().value;
        }


        public void SaveScreenshotGame()
        {
            currentPosition.nextValue = GetScreenshotGame();
            currentPosition.nextValue.previousValue = currentPosition;
            currentPosition = currentPosition.nextValue;
        }

        public HistoricHeap GetScreenshotGame()
        {
            return new HistoricHeap(SaveMapAsLevel());
        }

        public void MoveBackInTime()
        {
            if (currentPosition.previousValue == null)
            {
                GD.Print("Can't go back in time.");
                return;
            }


            CurrentPar--;

            currentPosition = currentPosition.previousValue;
            ChargeMapFromCurrentLevel();
        }

        public void MoveForwardInTime()
        {
            if (currentPosition.nextValue == null)
            {
                GD.Print("Can't go forward in time.");
                return;
            }

            CurrentPar++;

            currentPosition = currentPosition.nextValue;
            ChargeMapFromCurrentLevel();
        }

        private bool CheckWin()
        {

            for (int i = 0; i < currentLevel.Size.Y; i++)
            {
                for (int j = 0; j < currentLevel.Size.X; j++)
                { 
                    if (tileMap.GetCellTileData((int)Map.LevelLayer.Playground,new Vector2I(j,i))!=null && 
                  (bool)tileMap.GetCellTileData((int)Map.LevelLayer.Playground, new Vector2I(j, i)).GetCustomData(Map.CONTAINER))
                    {
                        if (tileMap.GetCellTileData((int)Map.LevelLayer.Target, new Vector2I(j, i)) == null || 
                     !(bool)tileMap.GetCellTileData((int)Map.LevelLayer.Target, new Vector2I(j, i)).GetCustomData(Map.TARGET))
                        {
                            return false;
                        }
                    }

                    if (tileMap.GetCellTileData((int)Map.LevelLayer.Target, new Vector2I(j, i)) != null &&
                  (bool)tileMap.GetCellTileData((int)Map.LevelLayer.Target, new Vector2I(j, i)).GetCustomData(Map.TARGET))
                    {
                        if (tileMap.GetCellTileData((int)Map.LevelLayer.Playground, new Vector2I(j, i)) == null ||
                     !(bool)tileMap.GetCellTileData((int)Map.LevelLayer.Playground, new Vector2I(j, i)).GetCustomData(Map.CONTAINER))
                        {
                            return false;
                        }
                    }


                }
            }

            return true;
        }



    }
}
