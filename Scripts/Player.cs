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

        [Export] private AnimationPlayer animPlayer;
        [Export] public AnimatedSprite2D animatedSprite;
        [Export] private Sprite2D sprite;

        private const float ANIM_TIME = 0.15f;
        private const float PATH_FINDING_TIME = 0.1f;
        private const string PLAYER_ACTION_RIGHT = "right";
        private const string PLAYER_ACTION_LEFT = "left";
        private const string PLAYER_ACTION_UP = "up";
        private const string PLAYER_ACTION_DOWN = "down";


        private const string PLAYER_MOVING_UP = "movingUp";
        private const string PLAYER_MOVING_DOWN = "movingDown";
        private const string PLAYER_MOVING_LEFT = "movingLeft";
        private const string PLAYER_MOVING_RIGHT = "movingRight";
        private const string ANIM_PLAYER = "Anim";



        public List<Vector2I> path = new List<Vector2I>();

        public Vector2I lastDirection;
        public Vector2I lastPosition;
        private static Vector2I pathPosition;

        public Timer pathFindingTimer = new Timer();

        private Dictionary<string, Vector2I> nameOfVector = new Dictionary<string, Vector2I>
        {
            { PLAYER_ACTION_RIGHT , Vector2I.Right },
            { PLAYER_ACTION_LEFT , Vector2I.Left },
            { PLAYER_ACTION_UP , Vector2I.Up },
            { PLAYER_ACTION_DOWN , Vector2I.Down },
        };

        private Dictionary<Vector2I, string> nameOfAnimation = new Dictionary<Vector2I, string>
        {
            { Vector2I.Up , PLAYER_MOVING_UP },
            { Vector2I.Down , PLAYER_MOVING_DOWN },
            { Vector2I.Right , PLAYER_MOVING_RIGHT },
            { Vector2I.Left , PLAYER_MOVING_LEFT },
        };


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

        public void GiveBombToPlayer(Bomb pBomb)
        {
            bombInHand = pBomb;
            GameManager.GetInstance().currentPosition.value.currentBomb = pBomb;
        }


        public override void _Ready()
        {
            pathFindingTimer.WaitTime = PATH_FINDING_TIME;

            pathFindingTimer.Timeout += MovingOnPath;
            AddChild(pathFindingTimer);
        }

        public override void _Process(double pDelta)
        {
            if (path.Count != 0 && pathFindingTimer.TimeLeft == 0)
            {
                pathFindingTimer.Start();
            }
            else if (!animPlayer.IsPlaying() && GlobalPosition != animatedSprite.Position && animatedSprite.Visible && !Box.animPlaying)
            {
                sprite.Visible = true;
                animatedSprite.Visible = false;
                GlobalPosition = animatedSprite.GlobalPosition;
                GameManager.GetInstance().UpdateAfterAction();


                //Position += nameOfVector[lActionName] * States.DISTANCE_RANGE;
            }
            else return;
        }

        private void MovingOnPath()
        {
            GoTo(path[0]);
            path.Remove(path[0]);

            GameManager.GetInstance().UpdateAfterAction();
            pathFindingTimer.Stop();
        }


        private bool CheckTheMove(Vector2I pDirectionVector)
        {
            Vector2I lUnitaryPos = GetPositionToVector2I();

            if (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, lUnitaryPos + pDirectionVector) == null) return false;

            else if ((bool)(GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, lUnitaryPos + pDirectionVector).GetCustomData(Map.INTERACTABLE)))
            {
                if ((bool)(GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, lUnitaryPos + pDirectionVector).GetCustomData(Map.CONTAINER)))
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
            if (Box.animPlaying || animatedSprite.Visible || animPlayer.IsPlaying()) { return; }

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
                        }
                        return;
                    }


                    if (Box.animPlaying)
                    {
                        AnimThePlayer();
                        return;
                    }
                    else
                    {
                        AnimThePlayer();

                    }
                }
            }

        }

        public void GoTo(Vector2I pPosition)
        {
            Position = (pPosition + Vector2.One / 2) * States.DISTANCE_RANGE;
        }

        private void AnimThePlayer()
        {

            sprite.Visible = false;
            animatedSprite.Visible = true;


            foreach (Vector2I lVector in nameOfAnimation.Keys)
            {
                if (lVector == lastDirection)
                {
                    animPlayer.Play(nameOfAnimation[lVector]);
                    animatedSprite.Play(nameOfAnimation[lVector] + ANIM_PLAYER);
                }
            }
        }

        private void ExplodeBombInHand()
        {
            if (bombInHand == null) return;

            bombInHand.Explode((Vector2I)Position / States.DISTANCE_RANGE + lastDirection, lastDirection);

            GameManager.GetInstance().UpdateAfterAction();

            GiveBombToPlayer(null);


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