using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class ButtonAnimation : Control
	{
        RandomNumberGenerator lRand = new RandomNumberGenerator();
        int lIndex;


        public override void _Ready()
		{
            foreach (Button lButtons in GetChildren())
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
			lTween.TweenProperty(pButton, TweenProp.SCALE, new Vector2(1.1f, 1.1f), 0.5f);
            lTween.TweenProperty(pButton, TweenProp.ROTATION, Mathf.DegToRad(lIndex == 0 ? -5 : 5), 0.5f);
        }

        private void AnimationMouseExited(Button pButton)
        {
            Tween lTween = CreateTween().SetParallel();
            lTween.TweenProperty(pButton, TweenProp.SCALE, new Vector2(1f, 1f), 0.5f);
            lTween.TweenProperty(pButton, TweenProp.ROTATION, Mathf.DegToRad(0), 0.5f);
        }
        private void AnimationMousePressed(Button pButton)
        {
            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.InOut).SetParallel();
            lTween.TweenProperty(pButton, TweenProp.ROTATION, 100, 1f);
            lTween.TweenProperty(pButton, TweenProp.POSITION_Y, GetWindow().Size.Y, 1f);
        }
    }
}
