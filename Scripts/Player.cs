using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Collections.Generic;
using System.Linq;

// Author : Cayot Daniel

namespace Com.IsartDigital.Sokoban
{
    public partial class Player : Area2D
    {
        static private Player instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Gameplay/player.tscn");

     	[Export] float timeOfFall = 1.8f;
        [Export] int turnToFall = 2;

        [Export] public Node2D oldTextureContainer;
        [Export] public Node2D newTextureContainer;

        public Node2D currentTextureContainer;


        [Export] public AnimationPlayer animPlayer;

        
        [Export] public AnimatedSprite2D oldAnimatedSprite;
        [Export] private AnimatedSprite2D oldActualPlayerSprite;
        
        [Export] public AnimatedSprite2D newAnimatedSprite;
        [Export] private AnimatedSprite2D newActualPlayerSprite;

        public AnimatedSprite2D currentAnimatedSprite;
        private AnimatedSprite2D currentActualPlayerSprite;


        public void UpdateTexture(bool pIsOld)
        {
            oldTextureContainer.Visible = pIsOld;
            newTextureContainer.Visible = !pIsOld;


            currentTextureContainer = (pIsOld?oldTextureContainer:newTextureContainer);
            currentAnimatedSprite = (pIsOld? oldAnimatedSprite : newAnimatedSprite);
            currentActualPlayerSprite = (pIsOld? oldActualPlayerSprite : newActualPlayerSprite);
        }

        
        [Export] private Node2D bombPrevisualisationContainer;

        [Export] public CollisionShape2D collider;

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

        public bool blocked = false;

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

            GraphicManager.Update();

            pathFindingTimer.WaitTime = pathFindingTime;

            pathFindingTimer.Timeout += MovingOnPath;
            AddChild(pathFindingTimer);

            animPlayer.Play(ANIM_IDLE);
            animPlayer.AnimationFinished += ReplaceThePlayer;
        }

        private void ReplaceThePlayer(StringName pAnimName)
        {
            OrientThePlayer();
            currentActualPlayerSprite.Play(orientation);

            currentActualPlayerSprite.Visible = true;
            currentAnimatedSprite.Visible = false;

            GlobalPosition = currentAnimatedSprite.GlobalPosition;
            if (!Box.animPlaying) GameManager.GetInstance().UpdateAfterAction();

            blocked = false;

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
            if (GlobalPosition != currentAnimatedSprite.GlobalPosition) { currentAnimatedSprite.GlobalPosition = GlobalPosition; }

            if (path.Count == 0) 
            { 

                pathFindingTimer.Stop();
                pathFindingTime = FIRST_TIME_PATH;
                pathFindingTimer.WaitTime= pathFindingTime;


                if (hasBoxToPush)
                {
                    hasBoxToPush = false;
                    Box.hasABoxToCheck = false;
                    

                    AdjacentToInteractable(Map.boxOrWallClickedOn - GetPositionToVector2I());
                }

                else if (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, Map.boxOrWallClickedOn) != null && (bool)GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, Map.boxOrWallClickedOn).GetCustomData(Map.WALL)
                    && (Map.boxOrWallClickedOn - GetPositionToVector2I()).LengthSquared() <= 1)
                {
                    ExplodeBombInHand(Map.boxOrWallClickedOn - GetPositionToVector2I());
                }

                return;
            }
            else
            {
                AnimThePlayer(path[0] - GetPositionToVector2I());

                path.RemoveAt(0);
                pathFindingTimer.Start();
            }
        }


        public bool CheckTheMove(Vector2I pDirectionVector )
        {
            Vector2I lUnitaryPos = GetPositionToVector2I();
     

            bool lIsNextCellEmpty = (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, lUnitaryPos + pDirectionVector) == null);
            bool lIsNextCellBox = !lIsNextCellEmpty && ((bool)GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, lUnitaryPos + pDirectionVector).GetCustomData(Map.BOX));
            bool lCanBoxBePushed = lIsNextCellBox && Box.CanBoxBePushed(pDirectionVector, lUnitaryPos + pDirectionVector);

            return lIsNextCellEmpty || lCanBoxBePushed;
        }


        public Vector2I GetPositionToVector2I()
        {
            return new Vector2I((int)(Position.X / Map.DISTANCE_RANGE), (int)(Position.Y / Map.DISTANCE_RANGE));
        }

        public void AdjacentToInteractable(Vector2I pDirection)
        {

            if (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, GetPositionToVector2I() + pDirection) == null)
            {
                AnimThePlayer(pDirection);
            }

            else if (((bool)GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Playground, GetPositionToVector2I()+ pDirection).GetCustomData(Map.BOX))
                && Box.CanBoxBePushed(pDirection, GetPositionToVector2I() + pDirection))
            {
                AnimThePlayer(pDirection);
                Box.Create( GetPositionToVector2I() + pDirection, pDirection);
                Box.hasABoxToCheck = false;
            }

            else 
            {
                ExplodeBombInHand(pDirection);
            }
        }


        public override void _Input(InputEvent pEvent)
        {
            if (!canInput) return;

            if ( animPlayer.CurrentAnimation != ANIM_IDLE || Box.animPlaying || path.Count != 0 || GameManager.GetInstance().startAnimation) { return; }

            foreach (string lActionName in PlayersVector.Keys)
            {
                if (Input.IsActionJustPressed(lActionName))
                {
                    GameManager.GetInstance().EmptyBoxSignalContainer();

                    Vector2I lTryDirection = PlayersVector[lActionName];

                    orientation = lActionName;
                    Box.hasABoxToCheck = false;


                    AdjacentToInteractable(lTryDirection);
                }
            }
        }

        public void StartAnimation(Tween pTween)
        {
            collider.Disabled = true;
            int lPlayerStraight = 360 * turnToFall;
            
            pTween.TweenProperty(this, TweenProp.POSITION_Y, Position.Y, timeOfFall).From(-Position.Y);
            pTween.TweenProperty(this, TweenProp.ROTATION, Mathf.DegToRad(lPlayerStraight + 90), timeOfFall);

            pTween.TweenProperty(this, TweenProp.ROTATION, Mathf.DegToRad(lPlayerStraight), 0.35f).SetDelay(timeOfFall + 0.6f);
            pTween.TweenProperty(this, TweenProp.ROTATION, 0, 0).SetDelay(timeOfFall + 1f);
        }

        public void AnimThePlayer(Vector2I pLastDirection)
        {
            if (pLastDirection == Vector2I.Zero )
            {
                return;
            }

            SoundManager.GetInstance().PlayFootStep();

            currentActualPlayerSprite.Visible = false;
            currentAnimatedSprite.Visible = true;

            animPlayer.Play(nameOfAnimation[pLastDirection]);
            currentAnimatedSprite.Play(nameOfAnimation[pLastDirection] + ANIM_PLAYER);

            lastDirection = pLastDirection;
        }


        private void OrientThePlayer()
        {
            orientation = PlayersVector.FirstOrDefault(lKeyValuePair => lKeyValuePair.Value == lastDirection).Key;


            currentActualPlayerSprite.Play(orientation);

           
        }


        private void ExplodeBombInHand(Vector2I pTryDirection)
        {
            if (pTryDirection != lastDirection) 
            {

                lastDirection = pTryDirection;
                OrientThePlayer();
                CreatePrevisualisation();

                return; 
            }

            OrientThePlayer();
            currentActualPlayerSprite.Play(orientation);
            if (bombInHand == null)
            {
                animPlayer.Play(ANIM_BLOCKED);
                blocked = true;
                currentAnimatedSprite.GlobalPosition = GlobalPosition;
                return;
            }

            bombInHand.Explode((Vector2I)Position / Map.DISTANCE_RANGE + lastDirection, lastDirection);

            GameManager.GetInstance().UpdateAfterAction();

            GiveBombToPlayer(null);
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }


        public void CreatePrevisualisation()
        {

            List<Node> lListBombPrevisu = bombPrevisualisationContainer.GetChildren().ToList();

            foreach (Node2D lChild in lListBombPrevisu)
            {
                lChild.QueueFree();
            }

            bombPrevisualisationContainer.Position = Vector2.Zero;

            if (bombInHand == null) return;


            foreach (Vector2I lDirection in nameOfAnimation.Keys)
            {
                if (!CheckTheMove(lDirection))
                {
                    new BombPattern(
                        bombPrevisualisationContainer,
                        Main.RotateMatrix(bombInHand.explosionMatrix, lDirection), 
                        BombPattern.EnumOfExplosionPattern.Player ,
                        false,
                        lDirection * Map.DISTANCE_RANGE,
                        (lDirection == lastDirection?1:0.2f)
                        
                        );
                }
            }
        }
    }
}