using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban
{
    public partial class GameManager : Node2D
    {
        [Export] Label par;

        static private GameManager instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/GameManager.tscn");


        private Level currentLevel;

        private List<LevelScreenShot> gameScreenshot = new List<LevelScreenShot>();
        private HistoricHeap currentPosition;


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
            GridManager.GetInstance().ChangeLevel(1);
            currentLevel = GridManager.GetInstance().CurrentLevel;
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
                if (Box.animPlaying) { return; }
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


                        case (char)ObjectChar.WALL:
                            tileMap.SetCell(0, new Vector2I(j, i), 0, objectPositionOnTileSet[ObjectChar.EMPTY]);
                            break;
                    }


                    tileMap.SetCell(1, new Vector2I(j, i), 0,
                            objectPositionOnTileSet[(ObjectChar)currentPosition.value.Map[i][j]]
                            );

                }
            }

            Vector2I lPlayerPosition = currentPosition.value.playerPosition;


         
            Player.GetInstance().GoTo(lPlayerPosition);

            FillGroundTiles(lPlayerPosition);
            tileMap.UpdateTheMap();
        }

        private void FillGroundTiles(Vector2I pStartCoor)
        {
            tileMap.SetCell(0, pStartCoor, 0, objectPositionOnTileSet[ObjectChar.EMPTY]);

            foreach (Vector2I lNeighbor in neighborsCoor)
            {
                if (tileMap.GetCellAtlasCoords(0, pStartCoor + lNeighbor) != objectPositionOnTileSet[ObjectChar.EMPTY])
                {
                    FillGroundTiles(pStartCoor + lNeighbor);
                }
            }
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

                    if(tileMap.GetCellTileData(1, new Vector2I(j, i))==null)
                    {
                        lRow += (char)ObjectChar.EMPTY;
                        continue;
                    }


                    if ((bool)tileMap.GetCellTileData(1, new Vector2I(j, i)).GetCustomData(Map.PLAY_OBJECT))
                    {
                        foreach (string lKey in Map.interactableToObjectChar.Keys)
                        {
                            if ((bool)tileMap.GetCellTileData(1, new Vector2I(j, i)).GetCustomData(lKey))
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


        public void SaveScreenshotGame()
        {
            currentPosition.nextValue = GetScreenshotGame();
            currentPosition.nextValue.previousValue = currentPosition;
            currentPosition = currentPosition.nextValue;
            tileMap.UpdateTheMap();
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

            currentPosition = currentPosition.nextValue;
            ChargeMapFromCurrentLevel();
        }
    }
}
