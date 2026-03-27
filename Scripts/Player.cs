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
        [Export] private Sprite2D sprite;
        
        [Export] private Node2D bombPrevisualisationContainer;

        private float pathFindingTime = 0.01f;
        private const float FIRST_TIME_PATH = 0.01f;
        private const float CASUAL_TIME_PATH = 0.2f;

        
        
        private const string PLAYER_ACTION_RIGHT = "right";
        private const string PLAYER_ACTION_LEFT = "left";
        private const string PLAYER_ACTION_UP = "up";
        private const string PLAYER_ACTION_DOWN = "down";


        private const string PLAYER_MOVING_UP = "movingUp";
        private const string PLAYER_MOVING_DOWN = "movingDown";
        private const string PLAYER_MOVING_LEFT = "movingLeft";
        private const string PLAYER_MOVING_RIGHT = "movingRight";
        private const string ANIM_PLAYER = "Anim";
        public const string ANIM_BLOCKED = "blocked";

        public List<Vector2I> path = new List<Vector2I>();

        public Vector2I lastDirection;
        public bool hasBoxToPush;

        public bool canInput = true;

        public Timer pathFindingTimer = new Timer();

        private Dictionary<string, Vector2I> nameOfVector = new Dictionary<string, Vector2I>
        {
            { PLAYER_ACTION_RIGHT , Vector2I.Right },
            { PLAYER_ACTION_LEFT , Vector2I.Left },
            { PLAYER_ACTION_UP , Vector2I.Up },
            { PLAYER_ACTION_DOWN , Vector2I.Down },
        };

        public Dictionary<Vector2I, string> nameOfAnimation = new Dictionary<Vector2I, string>
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
            CreatePrevisualisation();
        }


        public override void _Ready()
        {
            pathFindingTimer.WaitTime = pathFindingTime;

            pathFindingTimer.Timeout += MovingOnPath;
            AddChild(pathFindingTimer);


            animPlayer.AnimationFinished += ReplaceThePlayer;
        }

        private void ReplaceThePlayer(StringName pAnimName)
        {
            sprite.Visible = true;
            animatedSprite.Visible = false;

            GlobalPosition = animatedSprite.GlobalPosition;
            GameManager.GetInstance().UpdateAfterAction(); 
            
            CreatePrevisualisation();
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
            if (path.Count == 0) 
            { 

                pathFindingTimer.Stop();
                pathFindingTime = FIRST_TIME_PATH;
                pathFindingTimer.WaitTime= pathFindingTime;

                path.Clear();

                if (GlobalPosition != animatedSprite.GlobalPosition) { animatedSprite.GlobalPosition = GlobalPosition; }

                if ((GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, Map.boxOrContainerClickedOn) == null)) return;

                else if (hasBoxToPush)
                {
                    hasBoxToPush = false;
                    Box.hasABoxToCheck = false;

                    
                    lastDirection = Map.boxOrContainerClickedOn - GetPositionToVector2I();


                    if (Box.CanBoxBePushed(lastDirection, Map.boxOrContainerClickedOn))
                    {

                        AnimThePlayer(lastDirection);
                        Box.Create(Map.boxOrContainerClickedOn, lastDirection);
                    }

                    else
                    {
                        ExplodeBombInHand();
                    }


                }


                else if ((bool)GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, Map.boxOrContainerClickedOn).GetCustomData(Map.WALL)
                    && bombInHand != null)
                {
                    lastDirection = Map.boxOrContainerClickedOn - GetPositionToVector2I();
                    ExplodeBombInHand();
                }
                return;
            }
            else
            {
                animatedSprite.GlobalPosition = GlobalPosition;

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
                if ((bool)(GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, lUnitaryPos + pDirectionVector).GetCustomData(Map.CONTAINER)))
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
            if (Box.CanBoxBePushed(lastDirection, Map.boxOrContainerClickedOn))
            {
                AnimThePlayer(lastDirection);
                Box.Create(Map.boxOrContainerClickedOn, lastDirection);
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
            

            if (Box.animPlaying || animPlayer.IsPlaying() || path.Count != 0 || hasBoxToPush) { return; }

            foreach (string lActionName in nameOfVector.Keys)
            {
                if (Input.IsActionJustPressed(lActionName))
                {
                    lastDirection = nameOfVector[lActionName];
                    Box.hasABoxToCheck = false;

                    if (!CheckTheMove(nameOfVector[lActionName])) //if you are against a wall, or 2 consecutive boxes
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
            GlobalPosition = (pPosition + Vector2.One / 2) * States.DISTANCE_RANGE;
        }

        public void AnimThePlayer(Vector2I pLastDirection)
        {
            if (lastDirection == Vector2I.Zero )
            {
                return;
            }

            sprite.Visible = false;
            animatedSprite.Visible = true;

            animPlayer.Play(nameOfAnimation[pLastDirection]);
            animatedSprite.Play(nameOfAnimation[pLastDirection] + ANIM_PLAYER);

            if (path.Count != 0 )
            {
                GlobalPosition = animatedSprite.GlobalPosition;
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