using Godot;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class TimeStop : CanvasLayer
	{

		[Export] private ColorRect support;

		private ShaderMaterial shader;


		private const float EDGE = 0.3f;

		private float progression = 0;
        private float edge = 0;

        private float minValue = 0.1f;


		private Tween progressionTween;
		public override void _Ready()
		{
			shader = (ShaderMaterial)support.Material;

			shader.SetShaderParameter("progression",progression);
			shader.SetShaderParameter("minValue",minValue);

		}

		public override void _Process(double pDelta)
		{
			shader.SetShaderParameter("progression", progression);
			shader.SetShaderParameter("edge", edge);
		}

		public void TweenProgression(float pProgressionFinalValue,float pDuration, Vector2? pCenterPosition=null)
		{
            if(pCenterPosition!=null)shader.SetShaderParameter("CenterPosition", (Vector2)pCenterPosition);

            progressionTween?.Kill();
			progressionTween = CreateTween()
				.SetTrans(Tween.TransitionType.Expo)
				.SetEase(Tween.EaseType.Out).SetParallel();

			progressionTween.TweenProperty(this, "progression", pProgressionFinalValue, pDuration);
			progressionTween.TweenProperty(this, "edge", pProgressionFinalValue*EDGE, pDuration);
		}

	}
}
