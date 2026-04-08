using Com.IsartDigital.Utils.Effects;
using Godot;
using System.Collections.Generic;
using System.Linq;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class JuicinessManager : Node2D
	{
		static private JuicinessManager instance;
		static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/Manager/JuicinessManager.tscn");

		private Material bombShadow = (GD.Load<Material>("res://Ressources/Shaders/Materials/BombShadowShader.tres"));
		private const string bombShadowSpeedParameter = "speed";


        private float hoverAmplitude = 20;
        private float hoverSpeed = 0.1f;

        public float GlobalTime { private set; get; } = 0;


		[Export] public Shaker gameOverShaker;
		[Export] public Shaker simpleBombShaker;
		[Export] public Shaker fireworkShaker;

		private const float TIME_FOR_BANDEROLES = 0.2f;

		public Timer timeBeforeBanderoles = new Timer();
		private Timer waitBeforeNextEplosion = new Timer();
		private float timeBeforeNextExplosion = 1f;
		private float explodingAcceleration = 1.07f;


		List<Vector2I> alreadyExploded = new List<Vector2I>();
		List<Vector2I> lastExplosionPos = new List<Vector2I>();


		private JuicinessManager():base() 
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(JuicinessManager) + " Instance already exist, destroying the last added.");
				return;
			}
			instance = this;	
		}

		static public JuicinessManager GetInstance()
		{
			if (instance == null) instance = (JuicinessManager)factory.Instantiate();
			return instance;
		}

		public override void _Ready()
		{
			base._Ready();

			Node[] lArray = new Node[1];
			lArray[0] = CameraManager.GetInstance();

			fireworkShaker._targets = lArray;
			gameOverShaker._targets = lArray;
			simpleBombShaker._targets = lArray;


			HoverEffect.amplitude = hoverAmplitude;
			HoverEffect.speed = hoverSpeed;

			((ShaderMaterial)bombShadow).SetShaderParameter(bombShadowSpeedParameter, hoverSpeed);


			timeBeforeBanderoles.OneShot = true;
			timeBeforeBanderoles.WaitTime = TIME_FOR_BANDEROLES;
			timeBeforeBanderoles.Timeout += Banderole.GetInstance().StartTransitionToWin;
			AddChild(timeBeforeBanderoles);


			waitBeforeNextEplosion.WaitTime = timeBeforeNextExplosion;
			waitBeforeNextEplosion.OneShot = true;
			waitBeforeNextEplosion.Timeout += ExplodeAllTileInList;

			AddChild(waitBeforeNextEplosion);


		}

		public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;

			GlobalTime += lDelta;
		}

		public void ExplodeAllBorders(Vector2I pBorderOriginPos)
		{

			waitBeforeNextEplosion.WaitTime = timeBeforeNextExplosion;

			Player.GetInstance().Visible = false;
			Player.GetInstance().canInput = false;


			List<Node> lListBombCollectible = GameManager.GetInstance().bombCollectibleContainer.GetChildren().ToList();

            foreach (Node2D lChild in lListBombCollectible)
			{
				lChild.GetParent().RemoveChild(lChild);
			}


            lastExplosionPos.Clear();
			alreadyExploded.Clear();

            lastExplosionPos.Add(pBorderOriginPos);


            ExplodeAllTileInList();


		}

		private void ExplodeAllTileInList()
		{

			int lLastIndexOfLastExplosionPos = lastExplosionPos.Count-1;

            for (int i = lLastIndexOfLastExplosionPos; i >=0 ; i--) 
			{
				ExplodeNextBorder(lastExplosionPos[i]);
			} 
		}



		private void ExplodeNextBorder(Vector2I pBorderOriginPos)
		{


            BorderExplosion lBorderExplosion = BorderExplosion.Create(pBorderOriginPos);

			GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Ground, pBorderOriginPos);
			GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Target, pBorderOriginPos);
			GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Playground, pBorderOriginPos);


            lastExplosionPos.Remove(pBorderOriginPos);
			alreadyExploded.Add(pBorderOriginPos);

			List<Vector2I> lNeighborsCoor = GameManager.GetInstance().neighborsCoor;

            foreach (Vector2I lNeighborPos in lNeighborsCoor)
			{

				
				if (GameManager.GetInstance().tileMap.GetCellTileData((int)Map.LevelLayer.Ground, pBorderOriginPos + lNeighborPos) != null &&
					   !alreadyExploded.Contains(pBorderOriginPos + lNeighborPos) && !lastExplosionPos.Contains(pBorderOriginPos + lNeighborPos)
                       )
				{
					lastExplosionPos.Add(pBorderOriginPos+lNeighborPos);
				}

			}

			if (lastExplosionPos.Count != 0) { waitBeforeNextEplosion.WaitTime /= explodingAcceleration ; waitBeforeNextEplosion.Start(); }
			else lBorderExplosion.Finished += StopExplosion;

		}

		public void StopExplosion()
		{

			fireworkShaker.Stop();
			simpleBombShaker.Stop();
			gameOverShaker.Stop();

			waitBeforeNextEplosion.Stop();

			lastExplosionPos.Clear();
			alreadyExploded.Clear();

			List<Node> lGameOverExplosionList = GameManager.GetInstance().gameOverExplosionContainer.GetChildren().ToList();


            foreach (Node lBorderExplosion in lGameOverExplosionList)
			{
				lBorderExplosion.QueueFree();
			}

		}



		public void DoTheJuicyWin()
		{
			Banderole.GetInstance().StartTransitionToWin();
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
