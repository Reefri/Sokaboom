using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Collections.Generic;
using System.Linq;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class ButtonAnimation : Control
	{
        RandomNumberGenerator lRand = new RandomNumberGenerator();
        int lIndex;
        private const float DURATION_WHEN_CROSSED = 0.5f;

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
			lTween.TweenProperty(pButton, TweenProp.SCALE, new Vector2(1.1f, 1.1f), DURATION_WHEN_CROSSED);
            lTween.TweenProperty(pButton, TweenProp.ROTATION, Mathf.DegToRad(lIndex == 0 ? -5 : 5), DURATION_WHEN_CROSSED);
        }

        private void AnimationMouseExited(Button pButton)
        {
            if (TitleDoors.GetInstance().animationFinished)
            {
                Tween lTween = CreateTween().SetParallel();
                lTween.TweenProperty(pButton, TweenProp.SCALE, new Vector2(1f, 1f), DURATION_WHEN_CROSSED);
                lTween.TweenProperty(pButton, TweenProp.ROTATION, Mathf.DegToRad(0), DURATION_WHEN_CROSSED);
            }
        }
        private void AnimationMousePressed(Button pButton)
        {
            SoundManager.GetInstance().PlayClick();

            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.InOut).SetParallel();
            lTween.TweenProperty(pButton, TweenProp.ROTATION, 100, 1f);
            lTween.TweenProperty(pButton, TweenProp.POSITION_Y, GetWindow().Size.Y, 1f);
        }
    }
}
