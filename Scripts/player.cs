using Godot;
using System;
using System.Collections.Generic;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban
{
    public partial class Player : Area2D
    {
        static private Player instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Player.tscn");

        private const float ANIM_TIME = 0.15f;
        private const string PLAYER_ACTION_RIGHT = "right";
        private const string PLAYER_ACTION_LEFT = "left";
        private const string PLAYER_ACTION_UP = "up";
        private const string PLAYER_ACTION_DOWN = "down";


        public static Vector2I up = (Vector2I)Vector2.Up * States.DISTANCE_RANGE;
        public static Vector2I down = (Vector2I)Vector2.Down * States.DISTANCE_RANGE;
        public static Vector2I left = (Vector2I)Vector2.Left * States.DISTANCE_RANGE;
        public static Vector2I right = (Vector2I)Vector2.Right * States.DISTANCE_RANGE;



        public static Vector2I lastDirection;

        private List<Vector2> historicPositions = new List<Vector2>();
        private Timer timer = new Timer();
        
        private Dictionary<string,Vector2I> nameOfVector = new Dictionary<string, Vector2I>
        {
            { PLAYER_ACTION_RIGHT , Vector2I.Right },
            { PLAYER_ACTION_LEFT , Vector2I.Left },
            { PLAYER_ACTION_UP , Vector2I.Up },
            { PLAYER_ACTION_DOWN , Vector2I.Down },
        };

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
            timer.OneShot = true;
            Player.GetInstance().AddChild(timer);
            timer.Timeout += AnimFinishedMove;
        }

        private void AnimFinishedMove()
        {
            Position += lastDirection;
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
            if (Box.animPlaying) return;


            foreach (string lActionName in nameOfVector.Keys)
            {
                if (Input.IsActionJustPressed(lActionName) && !CheckTheMove(nameOfVector[lActionName]))
                {
                    if (Box.animPlaying)
                    {
                        lastDirection = nameOfVector[lActionName] * States.DISTANCE_RANGE;
                        timer.Start();

                    }
                    else
                    {
                        Position += nameOfVector[lActionName] * States.DISTANCE_RANGE;
                        GameManager.GetInstance().ScreenshotGame();
                    }

                }
            }


        }

        public void GoTo(Vector2I pPosition)
        {
            Position = (pPosition+Vector2.One/2)*States.DISTANCE_RANGE;
        }


        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
