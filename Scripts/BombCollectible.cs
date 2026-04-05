using Godot;
using System.Collections.Generic;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombCollectible : Area2D
	{
        [Export] Node2D hoverRenderer;
        [Export] Script hoverScript;

		private const string BOMB_COLLECTIBLE_PATH = "res://Scenes/Gameplay/Bomb/BombCollectible.tscn";

        private static PackedScene bombCollectible = GD.Load<PackedScene>(BOMB_COLLECTIBLE_PATH);

        private PrevisualisationBomb previsualisationBomb = (PrevisualisationBomb)GD.Load<PackedScene>("res://Scenes/UI/PrevisualisationBomb.tscn").Instantiate();

        private Node2D showPatern;

        public Bomb bomb;

        private Vector2 previsualisationOriginPos;
        private Vector2 rightCornerOfCollectible = new Vector2(25, -25);
        private float previsualisationScale = 0.3f;
        private float downFactor = 10;
        private float sideFactor = 0;
        private bool OriginOnTop;


        private Node2D chainReactionPatterne;


        [Export] Sprite2D head;
        [Export] Sprite2D body;

        private static float mainColorSaturation = 0.8f;
        private static float mainColorValue = 0.8f;

        private static float secondaryColorSaturation = 0.4f;
        private static float secondaryColorValue = 0.4f;

        private Color mainColor;
        private Color secondaryColor;

        private Texture2D bodyTexture;

        private static List<string> bodyTextureChoices = new List<string>()
        {
            "bubble",
            "diamond",
            "star",
            "triangle",
            "wave"
        };

        private const string BODYTEXTURE_FILE_PATH = "res://Assets/Bomb/Collectible/fireworkBody/";
        private const string BODYTEXTURE_FILE_EXTENSION = ".png";



        public override void _Ready()
		{


            hoverRenderer = (Node2D)GetNode("Renderer").GetNode("Hover");

            hoverRenderer.SetScript(hoverScript);

            hoverRenderer.AddChild(showPatern);

            showPatern.Scale = Vector2.One * 0.3f;
            showPatern.GlobalPosition = hoverRenderer.GlobalPosition + rightCornerOfCollectible;



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

        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;
        }

        private void InBomb()
        {
            hoverRenderer.Scale *= 1.5f;
            previsualisationBomb.explosionMatrix = bomb.explosionMatrix;


            UIManager.GetInstance().AddChild(previsualisationBomb);
        }
        private void OutBomb()
        {
            hoverRenderer.Scale /= 1.5f;

            UIManager.GetInstance().RemoveChild(previsualisationBomb);
        }

        public static BombCollectible Create(Bomb pBomb, Vector2I pPosition)
		{
			BombCollectible lBombCollectible = (BombCollectible)bombCollectible.Instantiate();
			lBombCollectible.Position = (pPosition) * States.DISTANCE_RANGE;
			lBombCollectible.ZIndex = 1;

			lBombCollectible.bomb = pBomb;


            lBombCollectible.mainColor = Color.FromHsv(GD.Randf(), mainColorSaturation, mainColorValue);
            lBombCollectible.secondaryColor = Color.FromHsv(GD.Randf(), secondaryColorSaturation, secondaryColorValue);

            lBombCollectible.bodyTexture = (Texture2D)GD.Load(
                BODYTEXTURE_FILE_PATH +
                bodyTextureChoices[GD.RandRange(0, bodyTextureChoices.Count - 1)] +
                BODYTEXTURE_FILE_EXTENSION
                );

            lBombCollectible.body.Texture = lBombCollectible.bodyTexture;
            lBombCollectible.head.Modulate = lBombCollectible.mainColor;


            ((ShaderMaterial)lBombCollectible.body.Material).SetShaderParameter("MainColor", lBombCollectible.mainColor);
            ((ShaderMaterial)lBombCollectible.body.Material).SetShaderParameter("SecondaryColor", lBombCollectible.secondaryColor);



            return lBombCollectible;
		}


        public BombCollectible Duplicate()
        {
            BombCollectible lBombCollectible = (BombCollectible)base.Duplicate();

            lBombCollectible.bomb = bomb.Duplicate() ;


            lBombCollectible.showPatern = new Node2D();

            lBombCollectible.previsualisationOriginPos = (new BombPattern(
                lBombCollectible.showPatern, 
                lBombCollectible.bomb.explosionMatrix, 
                BombPattern.EnumOfExplosionPattern.Collectible,
                default, 
                default
                
                )
                ).originePos;


            return lBombCollectible;
        }

        protected override void Dispose(bool pDisposing)
        {
            previsualisationBomb.QueueFree();
        }
    }
}
