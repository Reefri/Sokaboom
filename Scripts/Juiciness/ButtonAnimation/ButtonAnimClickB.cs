using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class ButtonAnimClickB : ButtonAnimMouse
    {
        protected override void AnimationMousePressed(Button pButton)
        {
            base.AnimationMousePressed(pButton);
            pressed = true;
            Vector2 originPos = pButton.Position;

            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.InOut).SetParallel();
            lTween.TweenProperty(pButton, TweenProp.ROTATION, Mathf.DegToRad(90), 0.3f);
            lTween.TweenProperty(pButton, TweenProp.POSITION_Y, GetWindow().Size.Y, 0.5f);
            lTween.TweenProperty(pButton, TweenProp.POSITION, originPos, 0.5f).SetDelay(0.5f);
            lTween.TweenProperty(pButton, TweenProp.ROTATION, 0, 0.5f).SetDelay(0.5f);
            lTween.Finished += () => ChangePressed(pButton) ;
        }

        private void ChangePressed(Button pButton)
        {
            pressed = false;
            AnimationMouseExited(pButton);
        }
    }
}
