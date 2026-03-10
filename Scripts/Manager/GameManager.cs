using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban
{
    public partial class GameManager : Node2D
    {
        static private GameManager instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/GameManager.tscn");

        private PackedScene levelFactory = GD.Load<PackedScene>("res://Scenes/Level.tscn");

        private Level currentLevel;

        private List<LevelScreenShot> gameScreenshot = new List<LevelScreenShot>();
        private int currentPositionInTime = -1;

        private List<Vector2I> neighborsCoor = new List<Vector2I>
        {
            Vector2I.Right,
            Vector2I.Left,
            Vector2I.Up,
            Vector2I.Down,
        };

        private Dictionary<ObjectChar, Vector2I> objectPositionOnTileSet = new Dictionary<ObjectChar, Vector2I>
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

            CreateLevel();
        }


        public override void _Process(double pDelta)
        {
            base._Process(pDelta);
            float lDelta = (float)pDelta;

            if (Input.IsActionJustPressed("TimeMinus"))
            {
                MoveInTime(-1);
            }
            if (Input.IsActionJustPressed("TimePlus"))
            {
                foreach (LevelScreenShot lLevel in gameScreenshot)
                {
                    GD.Print(lLevel.tileMap);
                }

            }
        }

        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }

        private void CreateLevel()
        {
            tileMap = (Map)levelFactory.Instantiate();

            Vector2I lPlayerPosition = new Vector2I(0, 0);


            for (int i = 0; i < currentLevel.Map.Count; i++)
            {
                for (int j = 0; j < currentLevel.Map[0].Length; j++)
                {

                    switch (currentLevel.Map[i][j])
                    {
                        case (char)ObjectChar.EMPTY:
                            continue;

                        case (char)ObjectChar.PLAYER:
                            lPlayerPosition = new Vector2I(j, i);
                            continue;

                        case (char)ObjectChar.WALL:
                            tileMap.SetCell(0, new Vector2I(j, i), 0, objectPositionOnTileSet[ObjectChar.EMPTY]);
                            break;
                    }

                    tileMap.SetCell(1, new Vector2I(j, i), 0,
                            objectPositionOnTileSet[(ObjectChar)currentLevel.Map[i][j]]
                            );

                }
            }

            //tileMap = lLevel;
            FillGroundTiles(lPlayerPosition);

            AddChild(Player.GetInstance());
            AddChild(tileMap);


            Player.GetInstance().GoTo(lPlayerPosition);

            ScreenshotGame();

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



        public void ScreenshotGame()
        {
            GD.Print("Say cheese !");

            int lHistoriqueLenght = gameScreenshot.Count;

            for (int i = currentPositionInTime+1; i<lHistoriqueLenght; i++) 
            {
                gameScreenshot.RemoveAt(currentPositionInTime);
            }
            
            gameScreenshot.Add(LevelScreenShot.DEFAULT);
            currentPositionInTime++;

            gameScreenshot[currentPositionInTime] = new LevelScreenShot((Map)tileMap.Duplicate(), Player.GetInstance().GetPositionToVector2I());
        }

 
        public void MoveInTime(int i)
        {
            if (currentPositionInTime + i >= gameScreenshot.Count || currentPositionInTime + i < 0)
            {
                GD.Print("Can't go " + i + " steps."+ currentPositionInTime + i + " : " + gameScreenshot.Count);
            }
            else
            {
                currentPositionInTime += i;
                ChargeCurrentTimeLevel();
            }
        }

        private void ChargeCurrentTimeLevel()
        {
            GD.Print("Charger la position " + currentPositionInTime);


            LevelScreenShot lLevelScreenShot = gameScreenshot[currentPositionInTime];

            RemoveChild(tileMap);
            tileMap = lLevelScreenShot.tileMap;
            AddChild(tileMap);

            Player.GetInstance().GoTo(lLevelScreenShot.playerPosition);
        }

    }
}
