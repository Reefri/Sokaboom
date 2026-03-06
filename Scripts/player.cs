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

        public static Vector2I up = (Vector2I)Vector2.Up * States.DISTANCE_RANGE;
        public static Vector2I down = (Vector2I)Vector2.Down * States.DISTANCE_RANGE;
        public static Vector2I left = (Vector2I)Vector2.Left * States.DISTANCE_RANGE;
        public static Vector2I right = (Vector2I)Vector2.Right * States.DISTANCE_RANGE;

        private static Vector2 lastPosition;
        private static Vector2I lastDirection;

        private List<Vector2> historicPositions = new List<Vector2>();
        private Timer timer = new Timer();
        
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

            else if ((bool)(Map.GetInstance().GetCellTileData(1, lUnitaryPos + pDirectionVector).GetCustomData("Interactible")))
            {
                if ((bool)(Map.GetInstance().GetCellTileData(1, lUnitaryPos + pDirectionVector).GetCustomData("Container")))
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

            if (Input.IsActionJustPressed("right") && !CheckTheMove((Vector2I)Vector2.Right) )
            {
                if (Box.animPlaying)
                {
                    lastDirection = right;
                    timer.Start();
                    return;
                }
                Position += right;
                historicPositions.Add(lastPosition);
            }

            else if (Input.IsActionJustPressed("left")&& !CheckTheMove((Vector2I)Vector2.Left) )
            {
                if (Box.animPlaying)
                {
                    lastDirection = left;
                    timer.Start();
                    return;
                }
                Position += left;
                historicPositions.Add(lastPosition);
            }
            else if (Input.IsActionJustPressed("up") && !CheckTheMove((Vector2I)Vector2.Up))
            {
                if (Box.animPlaying)
                {
                    lastDirection = up;
                    timer.Start();
                    return;
                }
                Position += up;
                historicPositions.Add(lastPosition);
            }
            else if (Input.IsActionJustPressed("down") && !CheckTheMove((Vector2I)Vector2.Down) )
            {
                if (Box.animPlaying)
                {
                    lastDirection = down;
                    timer.Start();
                    return;
                }
                historicPositions.Add(lastPosition);
                Position += down;
            }

        }


        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
