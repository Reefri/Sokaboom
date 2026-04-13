using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Collections.Generic;
using System.Linq;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class ButtonAnimMouse : Control
	{
        RandomNumberGenerator lRand = new RandomNumberGenerator();
        private int lIndex;
        private const float DURATION_WHEN_CROSSED = 0.5f;

        protected bool pressed;

        [Export] private int defaultRotation = 0;

        public override void _Ready()
        {
            List<Node> lChildren = GetChildren().ToList();

            foreach (Button lButtons in lChildren)
            {
                lButtons.MouseEntered += () => AnimationMouseEntered(lButtons);
                lButtons.MouseExited += () => AnimationMouseExited(lButtons);
                lButtons.Pressed += () => AnimationMousePressed(lButtons);

                lButtons.PivotOffset = lButtons.Size / 2;
            }
        }

        private void AnimationMouseEntered(Button pButton)
        {
            if (pButton.Disabled) return;
            lIndex = lRand.RandiRange(0, 1);

            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.OutIn).SetParallel();
            lTween.TweenProperty(pButton, TweenProp.SCALE, Vector2.One * 1.3f, DURATION_WHEN_CROSSED);
            lTween.TweenProperty(pButton, TweenProp.ROTATION, Mathf.DegToRad(lIndex == 0 ? -5 + defaultRotation: 5 + defaultRotation), DURATION_WHEN_CROSSED);
        }

        protected void AnimationMouseExited(Button pButton)
        {
            if (!pressed)
            {
                Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.OutIn).SetParallel();
                lTween.TweenProperty(pButton, TweenProp.SCALE, Vector2.One , DURATION_WHEN_CROSSED);
                lTween.TweenProperty(pButton, TweenProp.ROTATION, Mathf.DegToRad(0 + defaultRotation), DURATION_WHEN_CROSSED);
            }
        }
        virtual protected void AnimationMousePressed(Button pButton)
        {
            SoundManager.GetInstance().PlayClick();
        }
    }
}
