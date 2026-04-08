using Godot;
using System.Collections.Generic;
using System.Linq;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban
{
    public partial class GameManager : Node2D
    {
        [Export] public Node2D bombCollectibleContainer;


        [Export] public Node2D gameOverExplosionContainer;

        [Export] public Node2D bombExplosionContainer;
        [Export] public Node2D fireworkContainer;


        private Timer waitBeforeBoxSignal = new Timer();
        private float durationBeforeBoxSignal = 0.5f;
        List<Vector2I> positionForBoxSignal = new List<Vector2I>();
        [Export] public Node2D boxSignalContainer;

        static public GameManager instance = null;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Manager/GameManager.tscn");

        private PackedScene bombCollectible = GD.Load<PackedScene>("res://Scenes/Gameplay/Bomb/BombCollectible.tscn");


        private List<BombCollectible> levelBombCollectibles = new List<BombCollectible>();

        public Level currentLevel;
        
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
            

        public List<Vector2I> neighborsCoor = new List<Vector2I>
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


        public static bool IsManagerValid()
        {
            return instance!= null && instance.IsInsideTree();
        }

        public override void _Ready()
        {

            waitBeforeBoxSignal.WaitTime = durationBeforeBoxSignal;
            waitBeforeBoxSignal.Autostart = false;
            waitBeforeBoxSignal.OneShot = true;
            waitBeforeBoxSignal.Timeout += CreateBoxSignal;
            AddChild(waitBeforeBoxSignal);

            GridManager.GetInstance().ChangeLevel(UIManager.GetInstance().levelIndex);
            currentLevel = GridManager.GetInstance().CurrentLevel;


            currentPosition = new HistoricHeap(currentLevel);

            tileMap = Map.Create();

            AddChild(tileMap);
            AddChild(Player.GetInstance());

            int lNumberOfBombs = currentLevel.bombs.Count;

            for (int i = 0; i < lNumberOfBombs; i++)
            {
                levelBombCollectibles.Add(BombCollectible.Create(currentLevel.bombs[i], currentLevel.bombsPos[i]));
            }


            ChargeMapFromCurrentLevel();
        }

        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }

        public void ChargeMapFromCurrentLevel()
        {
            tileMap.Clear();

            int lYCurrentPositionSize = currentPosition.value.Size.Y;
            int lXCurrentPositionSize = currentPosition.value.Size.X;

            for (int i = 0; i < lYCurrentPositionSize; i++)
            {
                for (int j = 0; j < lXCurrentPositionSize; j++)
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


            List<Vector2I> lTargetPosList = currentPosition.value.targetsPos;

            foreach (Vector2I lTargetPos in lTargetPosList)
            {
                tileMap.SetCell((int)Map.LevelLayer.Target, lTargetPos, 0, objectPositionOnTileSet[ObjectChar.TARGET]);
            }


            List<Node> lBombsCollectible = bombCollectibleContainer.GetChildren().ToList();

            foreach (Node2D lBombCollectible in lBombsCollectible)
            {
                lBombCollectible.QueueFree();
            }


            List<int> lIndexOfAvalaibleBobms = currentPosition.value.indexOfAvalaibleBombs;

            foreach (int i in lIndexOfAvalaibleBobms)
            {
                bombCollectibleContainer.AddChild(levelBombCollectibles[i].Duplicate());
            }


            Vector2I lPlayerPosition = currentPosition.value.playerPosition;

            Player.GetInstance().Position = lPlayerPosition * Map.DISTANCE_RANGE;
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


        public void RemoveBombAtIndex(int lIndex)
        {
            currentPosition.value.indexOfAvalaibleBombs.Remove(lIndex);
        }

        private Level SaveMapAsLevel()
        {
            List<string> lRes = new List<string>();
            string lRow;

            int lYCurrentPositionSize = currentPosition.value.Size.Y;
            int lXCurrentPositionSize = currentPosition.value.Size.X;

            for (int i = 0; i < lYCurrentPositionSize; i++)
            {
                lRow = "";
                for (int j = 0; j < lXCurrentPositionSize; j++)
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

        public void UpdateAfterAction()
        {
            CurrentPar++;
            SaveScreenshotGame();

            CheckAndDoWin();
        }


        public void CheckAndDoWin()
        {
            if (CheckWin())
            {
                Player.GetInstance().canInput = false;
                JuicinessManager.GetInstance().timeBeforeBanderoles.Start();
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


        public void QuickResetInit()
        {


            Player.GetInstance().canInput = true;
            Player.GetInstance().Visible = true;

            JuicinessManager.GetInstance().StopExplosion();


            EmptyBombExplosionContainer();
            EmptyBoxSignalContainer();
            EmptyFireworkContainer();
        }

        public void EmptyFireworkContainer()
        {
            List<Node> lListFireworks = fireworkContainer.GetChildren().ToList();


            foreach (Node lFirework in lListFireworks)
            {
                lFirework.QueueFree();
            }
        }

        public void EmptyBombExplosionContainer()
        {
            List<Node> lListBombExplosions = bombExplosionContainer.GetChildren().ToList();

            foreach (Node lBombExplosion in lListBombExplosions)
            {
                lBombExplosion.QueueFree();
            }
        }

        public void EmptyBoxSignalContainer()
        {
            waitBeforeBoxSignal.Stop();

            List<Node> lListBoxSignal = boxSignalContainer.GetChildren().ToList();

            foreach (BoxSignal lBoxSignal in lListBoxSignal)
            {
                lBoxSignal.Destroy();
            }
        }

        public void MoveBackInTime()
        {
            if (Player.GetInstance().hasBoxToPush || Box.animPlaying) return;

            if (currentPosition.previousValue == null)
            {
                GD.Print("Can't go back in time !");
                return;
            }

            QuickResetInit();


            CurrentPar--;

            currentPosition = currentPosition.previousValue;
            ChargeMapFromCurrentLevel();
        }

        public void MoveForwardInTime()
        {
            if (currentPosition.nextValue == null)
            {
                GD.Print("Can't go forward in time !");
                return;
            }

            QuickResetInit();


            CurrentPar++;

            currentPosition = currentPosition.nextValue;
            ChargeMapFromCurrentLevel();
        }


        private List<Vector2I> GetTargetWithoutBoxPosition()
        {
            List<Vector2I> lListOfPos = new List<Vector2I>();


            int lYCurrentLevelSize = currentLevel.Size.Y;
            int lXCurrentLevelSize = currentLevel.Size.X;

            for (int i = 0; i < lYCurrentLevelSize; i++)
            {
                for (int j = 0; j < lXCurrentLevelSize; j++)
                {
                    if (
                         (tileMap.GetCellTileData((int)Map.LevelLayer.Target, new Vector2I(j, i)) != null &&
                    (bool)tileMap.GetCellTileData((int)Map.LevelLayer.Target, new Vector2I(j, i)).GetCustomData(Map.TARGET)) &&

                         (tileMap.GetCellTileData((int)Map.LevelLayer.Playground, new Vector2I(j, i)) == null ||
                   !(bool)tileMap.GetCellTileData((int)Map.LevelLayer.Playground, new Vector2I(j, i)).GetCustomData(Map.BOX))
                          )
                    {
                            lListOfPos.Add(new Vector2I(j, i));   
                    }
                }
            }

            return lListOfPos;
        }

        private List<Vector2I> GetBoxWithoutTargetPosition()
        {
            List<Vector2I> lListOfPos = new List<Vector2I>();

            int lYCurrentLevelSize = currentLevel.Size.Y;
            int lXCurrentLevelSize = currentLevel.Size.X;

            for (int i = 0; i < lYCurrentLevelSize; i++)
            {
                for (int j = 0; j < lXCurrentLevelSize; j++)
                {
                    if (
                            tileMap.GetCellTileData((int)Map.LevelLayer.Playground, new Vector2I(j, i)) != null &&
                      (bool)tileMap.GetCellTileData((int)Map.LevelLayer.Playground, new Vector2I(j, i)).GetCustomData(Map.BOX) &&

                           (tileMap.GetCellTileData((int)Map.LevelLayer.Target, new Vector2I(j, i)) == null ||
                     !(bool)tileMap.GetCellTileData((int)Map.LevelLayer.Target, new Vector2I(j, i)).GetCustomData(Map.TARGET)))
                    {
                        
                            lListOfPos.Add(new Vector2I(j, i));
                        
                    }
                }
            }

            return lListOfPos;
        }

        private bool CheckWin()
        {
            List<Vector2I> lListOfTargetWihtoutContainer = GetTargetWithoutBoxPosition();
            positionForBoxSignal = GetBoxWithoutTargetPosition();

            if (0 < lListOfTargetWihtoutContainer.Count) { return false; }



            if (positionForBoxSignal.Count > 0 && 
                lListOfTargetWihtoutContainer.Count==0 && 
                currentPosition.value.targetsPos.Count == currentLevel.targetsPos.Count)
            {
                waitBeforeBoxSignal.Start();
                return false;
            }

            waitBeforeBoxSignal.Stop();


            return currentPosition.value.targetsPos.Count == currentLevel.targetsPos.Count;
        }


        private void CreateBoxSignal()
        {
            foreach (Vector2I lBoxPosition in positionForBoxSignal)
            {
                BoxSignal.Create(lBoxPosition);
            }
        }
    }
}
