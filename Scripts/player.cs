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
        private string PLAYER_ACTION_RIGHT = "right";
        private string PLAYER_ACTION_LEFT = "left";
        private string PLAYER_ACTION_UP = "up";
        private string PLAYER_ACTION_DOWN = "down";


        public static Vector2I up = (Vector2I)Vector2.Up * States.DISTANCE_RANGE;
        public static Vector2I down = (Vector2I)Vector2.Down * States.DISTANCE_RANGE;
        public static Vector2I left = (Vector2I)Vector2.Left * States.DISTANCE_RANGE;
        public static Vector2I right = (Vector2I)Vector2.Right * States.DISTANCE_RANGE;

        private static Vector2 lastPosition;
        public static Vector2I lastDirection;

        private List<Vector2> historicPositions = new List<Vector2>();
        private Timer timer = new Timer();

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
            lastPosition = GlobalPosition;
            timer.WaitTime = ANIM_TIME;
            Player.GetInstance().AddChild(timer);
            timer.Timeout += AnimFinishedMove;
        }

        private void AnimFinishedMove()
        {
            Position += lastDirection;
            historicPositions.Add(lastPosition);
            timer.Stop();
        }

        private bool CheckTheMove(Vector2I pDirectionVector)
        {
            Vector2I lUnitaryPos = new Vector2I((int)Position.X/States.DISTANCE_RANGE, (int)Position.Y/States.DISTANCE_RANGE);

            if (Map.GetInstance().GetCellTileData(1, lUnitaryPos + pDirectionVector) == null) return false;

            else if ((bool)(Map.GetInstance().GetCellTileData(1, lUnitaryPos + pDirectionVector).GetCustomData(Map.GetInstance().INTERACTABLE)))
            {
                if ((bool)(Map.GetInstance().GetCellTileData(1, lUnitaryPos + pDirectionVector).GetCustomData(Map.GetInstance().CONTAINER)))
                {
                    return Box.CanBoxBePushed(pDirectionVector, lUnitaryPos + pDirectionVector);
                }
                // Ligne liée au bombes, à ramasser au besoin
                else return true;
            }
            else return false;
            //return (bool)Map.GetInstance().GetCellTileData(1, lUnitaryPos + pDirectionVector).GetCustomData("Wall");
        } 


        public override void _Input(InputEvent pEvent)
        {
            lastPosition = GlobalPosition;
            historicPositions.Add(lastPosition);

            if (Box.animPlaying) return;

            if (Input.IsActionJustPressed(PLAYER_ACTION_RIGHT))
            {
                lastDirection = right;

                if (CheckTheMove((Vector2I)Vector2.Right)) //if you are against a wall, or 2 consecutive boxes
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
                    Position += right;
                historicPositions.Add(lastPosition);
            }
           

            if (Input.IsActionJustPressed(PLAYER_ACTION_LEFT))
            {
                lastDirection = left;

                if (CheckTheMove((Vector2I)Vector2.Left))
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
                Position += left;
                historicPositions.Add(lastPosition);
            }
            if (Input.IsActionJustPressed(PLAYER_ACTION_UP))
            {
                lastDirection = up;

                if (CheckTheMove((Vector2I)Vector2.Up))
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
                Position += up;
                historicPositions.Add(lastPosition);
            }
            if (Input.IsActionJustPressed(PLAYER_ACTION_DOWN))
            {
                lastDirection = down;

                if (CheckTheMove((Vector2I)Vector2.Down))
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
                historicPositions.Add(lastPosition);
                Position += down;
            }

            

        }

        private void ExplodeBombInHand()
        {
            if (bombInHand == null) return;
            else

            {
                bombInHand.Explode((Vector2I)Position/ States.DISTANCE_RANGE +lastDirection/States.DISTANCE_RANGE);

                
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
