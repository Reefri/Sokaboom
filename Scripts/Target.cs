using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Diagnostics;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban
{
    public partial class Target : Node2D
    {
        private static PackedScene packedTarget = (PackedScene)ResourceLoader.Load("res://Scenes/Target.tscn");

        [Export] float timeOfFall = 1.5f;
        [Export] int turnToFall = 3;

        [Export] private Sprite2D renderer;

        public static Texture2D texture;



        public override void _Ready()
        {
            base._Ready();
            renderer.Texture = texture;
        }

        public static Target CreateAnimation(Vector2I pPosition, Tween pTween, float pInterval)
        {
            GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Target, pPosition);
            Target lTarget = (Target)packedTarget.Instantiate();
            lTarget.GlobalPosition = pPosition * (Map.DISTANCE_RANGE);

            lTarget.StartAnimation(pTween, pInterval);

            GameManager.GetInstance().tileMap.AddChild(lTarget);
            return lTarget;
        }

        private void StartAnimation(Tween pTween, float pInterval)
        {
            int lTargetStraight = 360 * turnToFall;

            pTween.TweenProperty(this, TweenProp.SCALE, Vector2.One, timeOfFall).SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.InOut).From(Vector2.Zero);
            pTween.TweenProperty(this, TweenProp.ROTATION, Mathf.DegToRad(lTargetStraight), timeOfFall + 0.5f).Finished += EndOfStartAnimation;
        }

        private void EndOfStartAnimation()
        {
            Vector2I lFinalPos = (Vector2I)(Position / Map.DISTANCE_RANGE);

            GameManager.GetInstance().tileMap.SetCell((int)Map.LevelLayer.Target, lFinalPos, 0, GameManager.GetInstance().objectPositionOnTileSet[ObjectChar.TARGET]);

            QueueFree();
        }
    }
}