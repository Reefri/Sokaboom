using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class Wall : Node2D
	{
        private static PackedScene packedWall = (PackedScene)ResourceLoader.Load("res://Scenes/wall.tscn");

        [Export] float timeOfFall = 0.5f;

        [Export] private Sprite2D renderer;

        public static Texture2D texture;

        public override void _Ready()
        {
            base._Ready();
            renderer.Texture = texture;
        }

        public static Wall CreateAnimation(Vector2I pPosition, Tween pTween, float pInterval)
        {
            GameManager.GetInstance().tileMap.EraseCell((int)Map.LevelLayer.Playground, pPosition);
            Wall lWall = (Wall)packedWall.Instantiate();
            lWall.GlobalPosition = pPosition * (Map.DISTANCE_RANGE);

            lWall.StartAnimation(pTween, pInterval);

            GameManager.GetInstance().tileMap.AddChild(lWall);
            return lWall;
        }

        private void StartAnimation(Tween pTween, float pInterval)
        {
            pTween.TweenProperty(this, TweenProp.POSITION_Y, -Position.Y, 0);
            pTween.TweenProperty(this, TweenProp.POSITION_Y, Position.Y, timeOfFall).SetDelay(pInterval);
            pTween.TweenProperty(this, TweenProp.SCALE, new Vector2(0.85f, 1.25f), timeOfFall).SetDelay(pInterval);


            pTween.TweenProperty(this, TweenProp.SCALE, new Vector2(1.25f, 0.85f), 0.15f).SetDelay(pInterval + timeOfFall);
            pTween.TweenProperty(this, TweenProp.POSITION_Y, Position.Y + 16, 0.15f).SetDelay(pInterval + timeOfFall);

            pTween.TweenProperty(this, TweenProp.SCALE, new Vector2(1, 1), 0.15f).SetDelay(pInterval + timeOfFall + 0.15f);
            pTween.TweenProperty(this, TweenProp.POSITION_Y, Position.Y, 0.15f).SetDelay(pInterval + timeOfFall + 0.15f).Finished += EndOfStartAnimation;
        }

        private void EndOfStartAnimation()
        {
            Vector2I lFinalPos = (Vector2I)(Position / Map.DISTANCE_RANGE);

            GameManager.GetInstance().tileMap.SetCell((int)Map.LevelLayer.Playground, lFinalPos, 0, GameManager.GetInstance().objectPositionOnTileSet[ObjectChar.WALL]);

            QueueFree();
        }
    }
}
