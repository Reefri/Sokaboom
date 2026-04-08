using Com.IsartDigital.Utils.Tweens;
using Godot;
using System;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban
{
    public partial class IsartLogo : Control
    {
        [Export] private PackedScene fadeIn;

        [Export] private CanvasLayer canvas;
        [Export] private ColorRect fade;

        private float time = 0;
        [Export] private float tweenDuration = 2;

        private bool active = true;
        
        public override void _Ready()
        {
            Tween lTween = fade.CreateTween();

            lTween.TweenProperty(fade, TweenProp.ROTATION, MathF.PI, tweenDuration);
            lTween.TweenProperty(fade, TweenProp.ROTATION, 0, tweenDuration);

        }

        public override void _Process(double pDelta)
        {
            float lDelta = (float)pDelta;
            time += lDelta;

            if (time >= tweenDuration * 2 && active)
            {
                Fade lTransition = (Fade)fadeIn.Instantiate();
                lTransition.queufreeWhenOver = true;
                lTransition.animationTimer.WaitTime = tweenDuration;
                UIManager.GetInstance().GetParent().AddChild(lTransition);
                active = false;
            }
            if (time >= tweenDuration * 2 + 0.25f)
            {
                if (!Main.GetInstance().noLogin) UIManager.GetInstance().GoToLogin();
                else
                {
                    AccountManager.GetInstance().TestConnexion("Guest", "");
                    UIManager.GetInstance().GoToTitle();

                    TitleDoors.GetInstance().ActivateDoors();
                }

              
            }
        }
    }
}
