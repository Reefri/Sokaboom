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

        private float maxRadius;
        private float duration;
        private float intensity;
        private float edge;


        private const float EXPLOSION_MAX_RADIUS = 0.7f;
        private const float EXPLOSION_DURATION = 3;
        private const float EXPLOSION_INTENSITY = 40;
        private const float EXPLOSION_EDGE = 0.03f;



        private const float WIN_MAX_RADIUS = 0.7f;
        private const float WIN_DURATION = 3;
        private const float WIN_INTENSITY = 4;
        private const float WIN_EDGE = 0.03f;


        private ShaderMaterial shaderMaterial = new ShaderMaterial();
        private const string RIPPLEEFFECT_SHADER_PATH = "res://Ressources/RippleEffect.tres";

        private Shader rippleEffect = (Shader)GD.Load(RIPPLEEFFECT_SHADER_PATH);



        public Vector2 centerPosition = Vector2.One/2;




        private void SetValues(float pMaxRadius,float pDuration,float pIntensity,float pEdge)
        {
            maxRadius = pMaxRadius;
            duration  = pDuration ;
            intensity = pIntensity;
            edge      = pEdge     ;
        }


        public static void CreateExplosion(Vector2 pPosition)
        {

            CanvasLayer lSupport = (CanvasLayer)factory.Instantiate();
            RippleEffect lEffect = (RippleEffect)lSupport.GetChild(0);

            lEffect.SetValues(EXPLOSION_MAX_RADIUS,EXPLOSION_DURATION,EXPLOSION_INTENSITY,EXPLOSION_EDGE);


            lEffect.centerPosition = (Main.GetInstance().GetViewport().CanvasTransform * (pPosition * Map.DISTANCE_RANGE)) / Main.GetInstance().GetViewport().GetVisibleRect().Size;


            Main.GetInstance().postProcessingNode.AddChild(lSupport);

        }

        public static void CreateWin(Vector2 pPosition)
        {
            CanvasLayer lSupport = (CanvasLayer)factory.Instantiate();

            RippleEffect lEffect = (RippleEffect)lSupport.GetChild(0);

            lEffect.SetValues(WIN_MAX_RADIUS,WIN_DURATION,WIN_INTENSITY,WIN_EDGE);


            lEffect.centerPosition = (Main.GetInstance().GetViewport().CanvasTransform * (pPosition * Map.DISTANCE_RANGE)) / Player.GetInstance().GetViewport().GetVisibleRect().Size;

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


            radiusTween.TweenProperty(this, "radiusProgression", 1, 1).From(0);

            radiusTween.Finished += GetParent().QueueFree;

        }

        public override void _Process(double pDelta)
        {
            float lDelta = (float)pDelta;

            shaderMaterial.SetShaderParameter("radius", radiusProgression);



        }



    }
}
