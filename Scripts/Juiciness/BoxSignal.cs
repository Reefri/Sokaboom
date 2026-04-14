using Godot;

// Author : Gramatikoff Sacha

namespace Com.IsartDigital.Sokoban {
	
	public partial class BoxSignal : Sprite2D
	{
        private static PackedScene factory = (PackedScene)GD.Load("res://Scenes/Juiciness/BoxSignal.tscn");

		private ShaderMaterial myMaterial;
		private string GROWPROGRESSION_SHADER_PARAMETER = "progressionGrow";
		private string MAXGROWPROGRESSION_SHADER_PARAMETER = "maxGrowth";
		[Export] private Color targetcolor;
		[Export] private float maxGrowth;
		private Color White = new Color(1,1, 1);


		private float progression = 0;
		private const string PROGRESSION_NAME = "progression";

		private Tween progressionTween;


		public override void _Ready()
		{
			this.Texture = Box.texture;

			myMaterial = (ShaderMaterial)Material;
			myMaterial.SetShaderParameter(MAXGROWPROGRESSION_SHADER_PARAMETER, maxGrowth);

			SoundManager.GetInstance().PlayBoxError();

            progressionTween = CreateTween()
				.SetTrans(Tween.TransitionType.Sine)
				.SetEase(Tween.EaseType.InOut)
				.SetLoops(3);

			progressionTween.TweenProperty(this, PROGRESSION_NAME, 1, 0.3f);
			progressionTween.TweenProperty(this, PROGRESSION_NAME, 0, 0.3f);

			progressionTween.Finished += QueueFree;
		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

			myMaterial.SetShaderParameter(GROWPROGRESSION_SHADER_PARAMETER,progression);

			Modulate = White * (1 - progression) + targetcolor * progression;
		}

		public void Destroy()
		{
			progressionTween.Kill();
			QueueFree();
		}
	

		public static void Create(Vector2I pPosition)
		{
			BoxSignal lNewSignal = (BoxSignal)factory.Instantiate();

			lNewSignal.GlobalPosition = pPosition * Map.DISTANCE_RANGE;

			GameManager.GetInstance().boxSignalContainer.AddChild(lNewSignal);
		}
	}
}
