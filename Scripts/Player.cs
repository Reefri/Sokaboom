using Godot;
using System;
using System.Collections.Generic;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban
{
    public partial class Player : Area2D
    {
        static private Player instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Gameplay/player.tscn");

        [Export] public AnimationPlayer animPlayer;
        [Export] public AnimatedSprite2D animatedSprite;
        [Export] private AnimatedSprite2D actualPlayerSprite;
        
        [Export] private Node2D bombPrevisualisationContainer;

        private float pathFindingTime = 0.01f;
        private const float FIRST_TIME_PATH = 0.01f;
        private const float CASUAL_TIME_PATH = 0.2f;

        private string orientation;
        
        private const string ACTION_RIGHT = "right";
        private const string ACTION_LEFT = "left";
        private const string ACTION_UP = "up";
        private const string ACTION_DOWN = "down";


        private const string MOVING_UP = "movingUp";
        private const string MOVING_DOWN = "movingDown";
        private const string MOVING_LEFT = "movingLeft";
        private const string MOVING_RIGHT = "movingRight";
        private const string ANIM_PLAYER = "Anim";
        private const string ANIM_IDLE = "idle";
        public const string ANIM_BLOCKED = "blocked";


        public List<Vector2I> path = new List<Vector2I>();

        public Vector2I lastDirection;
        public bool hasBoxToPush;

        public bool canInput = true;

        public Timer pathFindingTimer = new Timer();

        private Dictionary<string, Vector2I> PlayersVector = new Dictionary<string, Vector2I>
        {
            { ACTION_RIGHT , Vector2I.Right },
            { ACTION_LEFT , Vector2I.Left },
            { ACTION_UP , Vector2I.Up },
            { ACTION_DOWN , Vector2I.Down },
        };

        public Dictionary<Vector2I, string> nameOfAnimation = new Dictionary<Vector2I, string>
        {
            { Vector2I.Up , MOVING_UP },
            { Vector2I.Down , MOVING_DOWN },
            { Vector2I.Right , MOVING_RIGHT },
            { Vector2I.Left , MOVING_LEFT },
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
            CreatePrevisualisation();
        }


        public override void _Ready()
        {
            pathFindingTimer.WaitTime = pathFindingTime;

            pathFindingTimer.Timeout += MovingOnPath;
            AddChild(pathFindingTimer);

            animPlayer.Play(ANIM_IDLE);
            animPlayer.AnimationFinished += (AnimationMixer) => ReplaceThePlayer(ANIM_IDLE) ;
        }

        private void ReplaceThePlayer(StringName pAnimName)
        {
            OrientThePlayer();
            actualPlayerSprite.Play(orientation);

            actualPlayerSprite.Visible = true;
            animatedSprite.Visible = false;

            GlobalPosition = animatedSprite.GlobalPosition;
            if (!Box.animPlaying) GameManager.GetInstance().UpdateAfterAction(); 
            
            CreatePrevisualisation();
            animPlayer.Play(ANIM_IDLE);
        }

        public override void _Process(double pDelta)
        {
            if (path.Count != 0 && pathFindingTime == FIRST_TIME_PATH)
            {
                pathFindingTime = CASUAL_TIME_PATH;
                pathFindingTimer.Start();
            }


        }

        private void MovingOnPath()
        {
            pathFindingTimer.WaitTime = pathFindingTime;
            if (GlobalPosition != animatedSprite.GlobalPosition) { animatedSprite.GlobalPosition = GlobalPosition; }
            if (path.Count == 0) 
            { 

                pathFindingTimer.Stop();
                pathFindingTime = FIRST_TIME_PATH;
                pathFindingTimer.WaitTime= pathFindingTime;



                if ((GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, Map.boxOrWallClickedOn) == null)) return;


                else if (hasBoxToPush)
                {
                    hasBoxToPush = false;
                    Box.hasABoxToCheck = false;


                    lastDirection = Map.boxOrWallClickedOn - GetPositionToVector2I();


                    if (Box.CanBoxBePushed(lastDirection, Map.boxOrWallClickedOn))
                    {
                        AnimThePlayer(lastDirection);
                        Box.Create(Map.boxOrWallClickedOn, lastDirection);
                    }

                    else
                    {
                        ExplodeBombInHand();
                    }

                }

                else if ((bool)GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, Map.boxOrWallClickedOn).GetCustomData(Map.WALL)
                    && bombInHand != null)
                {
                    lastDirection = Map.boxOrWallClickedOn - GetPositionToVector2I();
                    ExplodeBombInHand();
                }
                return;
            }

            else
            {
                //animatedSprite.GlobalPosition = GlobalPosition;

                lastDirection = (path[0] - GetPositionToVector2I());

                AnimThePlayer(lastDirection);

                path.RemoveAt(0);
                pathFindingTimer.Start();
            }


        }


        public bool CheckTheMove(Vector2I pDirectionVector )
        {
            Vector2I lUnitaryPos = GetPositionToVector2I();

            if (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, lUnitaryPos + pDirectionVector) == null) return true;

            else if ((bool)(GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, lUnitaryPos + pDirectionVector).GetCustomData(Map.INTERACTABLE)))
            {
                if ((bool)(GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, lUnitaryPos + pDirectionVector).GetCustomData(Map.BOX)))
                {
                    return Box.CanBoxBePushed(pDirectionVector, lUnitaryPos + pDirectionVector);
                }
                else return false;
            }
            else return true;
        }



        public Vector2I GetPositionToVector2I()
        {
            return new Vector2I((int)(Position.X / States.DISTANCE_RANGE), (int)(Position.Y / States.DISTANCE_RANGE));
        }

        public void AdjacentToBox()
        {
            if (Box.CanBoxBePushed(lastDirection, Map.boxOrWallClickedOn))
            {
                AnimThePlayer(lastDirection);
                Box.Create(Map.boxOrWallClickedOn, lastDirection);
                Box.hasABoxToCheck = false;
            }

            else 
            {
                ExplodeBombInHand();
            }
        }



        public override void _Input(InputEvent pEvent)
        {
            if (!canInput) return;

            if ( animPlayer.CurrentAnimation != ANIM_IDLE || Box.animPlaying || path.Count != 0 || hasBoxToPush) { return; }

            foreach (string lActionName in PlayersVector.Keys)
            {
                if (Input.IsActionJustPressed(lActionName))
                {
                    lastDirection = PlayersVector[lActionName];
                    orientation = lActionName;
                    Box.hasABoxToCheck = false;

                    //OrientThePlayer();

                    if (!CheckTheMove(lastDirection)) //if you are against a wall, or 2 consecutive boxes
                    {
                        ExplodeBombInHand();
                    }


                    else
                    {
                        if (Box.hasABoxToCheck)
                        {
                            Box.Create(GetPositionToVector2I() + lastDirection, lastDirection);
                            AnimThePlayer(lastDirection);
                            Box.hasABoxToCheck = false;
                            return;
                        }

                        else
                        {
                            AnimThePlayer(lastDirection);
                        }


                    }
                }
            }
        }



        public void GoTo(Vector2I pPosition)
        {
            GlobalPosition = (pPosition ) * States.DISTANCE_RANGE;
        }

        public void AnimThePlayer(Vector2I pLastDirection)
        {
            if (pLastDirection == Vector2I.Zero )
            {
                return;
            }

            //OrientThePlayer();

            actualPlayerSprite.Visible = false;
            animatedSprite.Visible = true;

            animPlayer.Play(nameOfAnimation[pLastDirection]);
            animatedSprite.Play(nameOfAnimation[pLastDirection] + ANIM_PLAYER);


        }


        private void OrientThePlayer()
        {


            foreach (string lActionName in PlayersVector.Keys)
            {
                if (lastDirection == PlayersVector[lActionName])
                {
                    orientation = lActionName;
                    GD.Print(orientation);
                }
            }
        }


        private void ExplodeBombInHand()
        {
            OrientThePlayer();
            actualPlayerSprite.Play(orientation);
            if (bombInHand == null)
            {
                animPlayer.Play(ANIM_BLOCKED);

                animatedSprite.GlobalPosition = GlobalPosition;
                return;
            }

            //GameManager.GetInstance().canMoveBackInTime = false;
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


        public void CreatePrevisualisation()
        {
            foreach (Node2D lChild in bombPrevisualisationContainer.GetChildren())
            {
                lChild.QueueFree();
            }

            bombPrevisualisationContainer.Position = Vector2.Zero;

            if (bombInHand == null) return;



            foreach (Vector2I lDirection in nameOfAnimation.Keys)
            {
                if (!CheckTheMove(lDirection))
                {

                    new BombPattern(bombPrevisualisationContainer,false, Main.GetInstance().RotateMatrix(bombInHand.explosionMatrix, lDirection), false,lDirection*States.DISTANCE_RANGE);

                }
            }
        }



    }
}