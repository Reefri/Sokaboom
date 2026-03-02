using Godot;
using System;
using System.Collections.Generic;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban
{
    public partial class player : Area2D
    {
        static private player instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/player.tscn");

        //public List<List<char>> exempleList = new List<List<char>>();
        private const float DISTANCE_RANGE = 64f;

        private Vector2 up = Vector2.Up * DISTANCE_RANGE;
        private Vector2 down = Vector2.Down * DISTANCE_RANGE;
        private Vector2 left = Vector2.Left * DISTANCE_RANGE;
        private Vector2 right = Vector2.Right * DISTANCE_RANGE;
        private static Vector2 lastPosition;

        private player() : base()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(player) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }

        static public player GetInstance()
        {
            if (instance == null) instance = (player)factory.Instantiate();
            return instance;
        }

        public override void _Ready()
        {
            BodyShapeEntered += PlayerCollision;
            lastPosition = GlobalPosition;
            base._Ready();
        }



        public override void _Input(InputEvent @event)
        {
            lastPosition = GlobalPosition;
            if (Input.IsActionJustPressed("right")) Position += right;

            else if (Input.IsActionJustPressed("left")) Position += left;
            else if (Input.IsActionJustPressed("up")) Position += up;
            else if (Input.IsActionJustPressed("down")) Position += down;

        }

        private void PlayerCollision(Rid pBodyRid, Node2D pBody, long pBodyShapeIndex, long pLocalShapeIndex)
        {
            if (pBody is TileMap)
            {
                GD.Print("Just debugging");
                TileMap lTileMap = (TileMap)pBody;
                Vector2 lActualPosition = GlobalPosition;

                Vector2I lCell = lTileMap.LocalToMap(lTileMap.ToLocal(lActualPosition));

                TileData lData = lTileMap.GetCellTileData(0, lCell);

                if (lData != null) Position = lastPosition;
            }
        }


        //public override void _Process(double pDelta)
        //{
        //    base._Process(pDelta);
        //    float lDelta = (float)pDelta;
        //    //MovePlayer();

        //}

        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
