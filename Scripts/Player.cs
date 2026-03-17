using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban
{
    public partial class Player : Area2D
    {
        static private Player instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Player.tscn");

        private const float ANIM_TIME = 0.15f;
        private const float PATH_FINDING_TIME = 0.1f;
        private const string PLAYER_ACTION_RIGHT = "right";
        private const string PLAYER_ACTION_LEFT = "left";
        private const string PLAYER_ACTION_UP = "up";
        private const string PLAYER_ACTION_DOWN = "down";



        public List<Vector2I> path = new List<Vector2I>();

        public Vector2I lastDirection;
        private static Vector2I pathPosition;

        private List<Vector2> historicPositions = new List<Vector2>();
        private Timer timer = new Timer();
        public Timer pathFindingTimer = new Timer();

        private Dictionary<string, Vector2I> nameOfVector = new Dictionary<string, Vector2I>
        {
            { PLAYER_ACTION_RIGHT , Vector2I.Right },
            { PLAYER_ACTION_LEFT , Vector2I.Left },
            { PLAYER_ACTION_UP , Vector2I.Up },
            { PLAYER_ACTION_DOWN , Vector2I.Down },
        };


        public bool holdingBomb = false;
        public Bomb bombInHand;

        private Player() : base()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(Player) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }

        static public Player GetInstance()
        {
            if (instance == null) instance = (Player)factory.Instantiate();
            return instance;
        }



        public override void _Ready()
        {
            timer.WaitTime = ANIM_TIME;
            pathFindingTimer.WaitTime = PATH_FINDING_TIME;
            timer.OneShot = true;
            AddChild(timer);

            timer.Timeout += AnimFinishedMove;
            pathFindingTimer.Timeout += MovingOnPath;
            AddChild(pathFindingTimer);
        }

        public override void _Process(double delta)
        {
            if (path.Count != 0 && pathFindingTimer.TimeLeft == 0)
            {
                pathFindingTimer.Start();
            }
            else return;

        }

        private void MovingOnPath()
        {
            GoTo(path[0]);
            GD.Print(path.Count);
            path.Remove(path[0]);
            pathFindingTimer.Stop();
        }

        private void AnimFinishedMove()
        {
            Position += lastDirection * States.DISTANCE_RANGE;
        }

        private bool CheckTheMove(Vector2I pDirectionVector)
        {


            Vector2I lUnitaryPos = GetPositionToVector2I();

            if (GameManager.GetInstance().tileMap.GetCellTileData(1, lUnitaryPos + pDirectionVector) == null) return false;

            else if ((bool)(GameManager.GetInstance().tileMap.GetCellTileData(1, lUnitaryPos + pDirectionVector).GetCustomData(Map.INTERACTABLE)))
            {
                if ((bool)(GameManager.GetInstance().tileMap.GetCellTileData(1, lUnitaryPos + pDirectionVector).GetCustomData(Map.CONTAINER)))
                {
                    return Box.CanBoxBePushed(pDirectionVector, lUnitaryPos + pDirectionVector);
                }
                else return true;
            }
            else return false;
        }

        public Vector2I GetPositionToVector2I()
        {
            return new Vector2I((int)Position.X / States.DISTANCE_RANGE, (int)Position.Y / States.DISTANCE_RANGE);
        }


        public override void _Input(InputEvent pEvent)
        {
            if (Box.animPlaying) { return; }

            foreach (string lActionName in nameOfVector.Keys)
            {

                if (Input.IsActionJustPressed(lActionName))
                {
                    lastDirection = nameOfVector[lActionName];

                    if (CheckTheMove(nameOfVector[lActionName])) //if you are against a wall, or 2 consecutive boxes
                    {
                        if (bombInHand != null)
                        {
                            ExplodeBombInHand();
                            bombInHand = null;
                        }
                        return;
                    }


                    if (Box.animPlaying)
                    {
                        timer.Start();
                        return;
                    }
                    else
                    {
                        Position += nameOfVector[lActionName] * States.DISTANCE_RANGE;
                        GameManager.GetInstance().UpdateAfterAction();
                    }
                }
            }

        }

        public void GoTo(Vector2I pPosition)
        {
            Position = (pPosition + Vector2.One / 2) * States.DISTANCE_RANGE;
        }

        private void ExplodeBombInHand()
        {
            if (bombInHand == null) return;
            else

            {
                bombInHand.Explode((Vector2I)Position / States.DISTANCE_RANGE + lastDirection, lastDirection);


            }

            //pour faire exploser les tiles, les remplacer par une tile de sol (AtlasCoords : 11, 6)
            //Map.GetInstance().SetCell(0, gridCoords, atlasCoord)
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
