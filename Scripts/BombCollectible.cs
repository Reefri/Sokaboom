using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Collections.Generic;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombCollectible : Area2D
	{
        [Export] private Node2D hoverRenderer;
        //[Export] private Script hoverScript;

        private const string BOMB_COLLECTIBLE_PATH = "res://Scenes/Gameplay/Bomb/BombCollectible.tscn";

        private static PackedScene factory = GD.Load<PackedScene>(BOMB_COLLECTIBLE_PATH);

        private PackedScene previsualisationBomb = GD.Load<PackedScene>("res://Scenes/UI/PrevisualisationBomb.tscn");
        private PrevisualisationBomb previsualisationCreate;

        private Node2D showPatern;

        public Bomb bomb;

        private Vector2I previsualisationOriginPos;
        private Vector2 rightCornerOfCollectible = new Vector2(25, -25);
        private float previsualisationScale = 0.3f;
        private float downFactor = 10;
        private float sideFactor = 0;
        private bool OriginOnTop;


        private Node2D chainReactionPatterne;


        [Export] private Sprite2D head;
        [Export] private Sprite2D body;


        private Color mainColor;
        private Color secondaryColor;



      

        private const string BODYTEXTURE_FILE_PATH = "res://Assets/Bomb/Collectible/fireworkBody/";
        private const string BODYTEXTURE_FILE_EXTENSION = ".png";
        private string myTextureChoice;


        private Timer chainTimer = new Timer();
        private float chainWait = 0.3f;


        private Vector2I positionInGrid;

        public override void _Ready()
        {

            if (GameManager.GetInstance().bombStartAnimation < GameManager.GetInstance().levelBombCollectibles.Count)
            {
                StartAnimation();
                GameManager.GetInstance().bombStartAnimation++;
            }


            previsualisationOriginPos = (new BombPattern(
                showPatern,
                bomb.explosionMatrix,
                BombPattern.EnumOfExplosionPattern.Collectible,
                default,
                default

                )
                ).originePos;


            hoverRenderer.AddChild(showPatern);


            chainTimer.WaitTime = chainWait;
            chainTimer.Autostart = false;
            chainTimer.OneShot = true;
            chainTimer.Timeout += Explode;
            AddChild(chainTimer);

           





            showPatern.Scale = Vector2.One * 0.3f;
            showPatern.GlobalPosition = hoverRenderer.GlobalPosition + rightCornerOfCollectible;





            body.Texture = (Texture2D)GD.Load(
              BODYTEXTURE_FILE_PATH +
              myTextureChoice +
              BODYTEXTURE_FILE_EXTENSION
              ); 

            head.Modulate = mainColor;


            ((ShaderMaterial)body.Material).SetShaderParameter("MainColor", mainColor);
            ((ShaderMaterial)body.Material).SetShaderParameter("SecondaryColor", secondaryColor);


            AreaEntered += BombCollectibleAreaEntered;


            MouseEntered += InBomb;
            MouseExited += OutBomb;


          
        }


        private void BombCollectibleAreaEntered(Area2D pArea)
        {

			if(pArea == Player.GetInstance() && Player.GetInstance().bombInHand == null)
			{
				GameManager.GetInstance().RemoveBombAtIndex(bomb.indexInLevel);

				Player.GetInstance().GiveBombToPlayer(bomb);
                SoundManager.GetInstance().PlayBombPickUp();

                QueueFree();
                return;
			}
        }

        public void ShowChainReaction(float pScale)
        {
            if (chainReactionPatterne!=null && chainReactionPatterne.IsInsideTree()) return;

            chainReactionPatterne = new Node2D();

            new BombPattern(chainReactionPatterne, bomb.explosionMatrix, BombPattern.EnumOfExplosionPattern.Player, false, null, pScale);
            
            AddChild(chainReactionPatterne);
        }

        public void HideChainReaction()
        {
            chainReactionPatterne?.QueueFree();
            chainReactionPatterne=null;
        }

     
        private void InBomb()
        {
            hoverRenderer.Scale *= 1.5f;
            previsualisationCreate = (PrevisualisationBomb)previsualisationBomb.Instantiate();
            previsualisationCreate.explosionMatrix = bomb.explosionMatrix;


            UIManager.GetInstance().AddChild(previsualisationCreate);
        }
        private void OutBomb()
        {
            hoverRenderer.Scale /= 1.5f;

            if (previsualisationCreate != null) previsualisationCreate.QueueFree();
            previsualisationCreate = null;
        }

      

        public void StartAnimation()
        {
            Modulate = new Color(1,1,1,0);
            Tween lTween = CreateTween().SetParallel();
            lTween.TweenProperty(this, TweenProp.MODULATE_ALPHA, 1, GD.Randf()).SetDelay(GD.Randf());
        }

        public static BombCollectible Create(BombCollectiblePatron pPatron )
        {
            BombCollectible lBombCollectible = (BombCollectible)factory.Instantiate();

            lBombCollectible.bomb = pPatron.bomb ;
            lBombCollectible.positionInGrid = pPatron.positionInGrid;
            lBombCollectible.Position = pPatron.positionInGrid * Map.DISTANCE_RANGE;

            lBombCollectible.mainColor = pPatron.mainColor;
            lBombCollectible.secondaryColor = pPatron.secondaryColor;

            lBombCollectible.showPatern = new Node2D();

            lBombCollectible.myTextureChoice = pPatron.texturePath;

            lBombCollectible.ZIndex = 1;




            return lBombCollectible;
        }

        protected override void Dispose(bool pDisposing)
        {
            if (previsualisationCreate != null) previsualisationCreate.QueueFree();
        }


        public void RotatePattern(Vector2 pDirection)
        {
            showPatern.GlobalRotation =pDirection.Rotated(Mathf.Pi/2).Angle();
        }

        private void Explode()
        {
            bomb.Explode(positionInGrid,Vector2I.Up);
            QueueFree();


            GameManager.GetInstance().RemoveBombAtIndex(bomb.indexInLevel);
            GameManager.GetInstance().UpdateCurrentPosition();
        }

        public void QueueForChain()
        {

            Tween lTween = CreateTween().
                SetTrans(Tween.TransitionType.Expo).
                SetEase(Tween.EaseType.Out).SetParallel();

            lTween.TweenProperty(hoverRenderer,TweenProp.SCALE, Vector2.One * 2,0.3f);
            lTween.TweenProperty(head,TweenProp.MODULATE, new Color(1,1,1),0.3f);
            lTween.TweenProperty(this,"mainColor", new Color(1,1,1),0.3f);
            lTween.TweenProperty(this,"secondaryColor", new Color(1,1,1),0.3f);
            showPatern.Hide();

            chainTimer.Start();
        }

        public override void _Process(double pDelta)
        {
            ((ShaderMaterial)body.Material).SetShaderParameter("MainColor", mainColor);
            ((ShaderMaterial)body.Material).SetShaderParameter("SecondaryColor", secondaryColor);

        }
    }
}
