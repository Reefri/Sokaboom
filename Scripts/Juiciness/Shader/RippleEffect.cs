using Com.IsartDigital.Sokoban;
using Godot;

// Author : Gramatikoff Sacha
namespace Com.IsartDigital.Chromaberation
{

    public partial class RippleEffect : ColorRect
    {

        private static PackedScene factory = (PackedScene)GD.Load("res://Scenes/Juiciness/RippleEffect.tscn");

        private Tween radiusTween;
        private float radiusProgression = 0;


        private const float maxRadius = 0.7f;
        private float duration = 3;
        private float intensity = 4;
        private float edge = 0.03f;


        private ShaderMaterial shaderMaterial = new ShaderMaterial();
        private const string RIPPLEEFFECT_SHADER_PATH = "res://Ressources/RippleEffect.tres";

        private Shader rippleEffect = (Shader)GD.Load(RIPPLEEFFECT_SHADER_PATH);



        public Vector2 centerPosition = Vector2.One/2;


        private float camouflageShaderTimeIn = 1f;
        private float camouflageShaderValueIn = 0.4f;


        public static void Create(Vector2 pPostion)
        {
            CanvasLayer lSupport = (CanvasLayer)factory.Instantiate();

            RippleEffect lEffect = (RippleEffect)lSupport.GetChild(0);

            lEffect.centerPosition = (Player.GetInstance().GetViewport().CanvasTransform * Player.GetInstance().GlobalPosition + Player.GetInstance().lastDirection*Map.DISTANCE_RANGE) / Player.GetInstance().GetViewport().GetVisibleRect().Size;

            GD.Print(lEffect.centerPosition);

            Main.GetInstance().postProcessingNode.AddChild(lSupport);

        }


        public override void _Ready()
        {


            SetAnchorsPreset(LayoutPreset.FullRect);
            this.Material = shaderMaterial;
            shaderMaterial.Shader = rippleEffect;

            shaderMaterial.SetShaderParameter("intensity", intensity);
            shaderMaterial.SetShaderParameter("maxRadius", maxRadius);
            shaderMaterial.SetShaderParameter("CenterPosition", centerPosition);
            shaderMaterial.SetShaderParameter("edge", edge);

            radiusTween = CreateTween()
                .SetTrans(Tween.TransitionType.Linear)
                .SetEase(Tween.EaseType.Out);


            radiusTween.TweenProperty(this, "radiusProgression", 1, camouflageShaderTimeIn).From(0);

            radiusTween.Finished += QueueFree;

        }

        public override void _Process(double pDelta)
        {
            float lDelta = (float)pDelta;

            shaderMaterial.SetShaderParameter("radius", radiusProgression);



        }



    }
}
