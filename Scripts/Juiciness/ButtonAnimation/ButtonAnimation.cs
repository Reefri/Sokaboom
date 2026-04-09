using Com.IsartDigital.Utils.Tweens;
using Godot;
using System.Collections.Generic;
using System.Linq;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class ButtonAnimation : ButtonAnimMouse
    {
        protected override void AnimationMousePressed(Button pButton)
        {
            base.AnimationMousePressed(pButton);
            pressed = true;

            Tween lTween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.InOut).SetParallel();
            lTween.TweenProperty(pButton, TweenProp.ROTATION, 100, 1f);
            lTween.TweenProperty(pButton, TweenProp.POSITION_Y, GetWindow().Size.Y, 1f);
        }
    }
}
