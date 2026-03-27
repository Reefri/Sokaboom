using Com.IsartDigital.Sokoban.UI;
using Com.IsartDigital.Utils.Tweens;
using Godot;
using System;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban
{
    public partial class IsartLogo : Control
    {
        static private IsartLogo instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/UI/IsartLogo.tscn");

        [Export] private PackedScene fadeIn;

        [Export] private CanvasLayer canvas;
        [Export] private ColorRect fade;

        private float time = 0;
        [Export] private float tweenDuration = 2;

        private bool active = true;

        private IsartLogo() : base()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(IsartLogo) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }

        static public IsartLogo GetInstance()
        {
            if (instance == null) instance = (IsartLogo)factory.Instantiate();
            return instance;
        }

        
        public override void _Ready()
        {
            if(!Main.GetInstance().testOnlyGameFeature)
                Visible = true;
            else QueueFree();

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
                SceneTransition lTransition = (SceneTransition)fadeIn.Instantiate();
                lTransition.queufreeWhenOver = true;
                lTransition.animationTimer.WaitTime = tweenDuration;
                UIManager.GetInstance().GetParent().AddChild(lTransition);
                active = false;
            }
            if (time >= tweenDuration * 2 + 0.25f || Input.IsActionJustReleased("leftClick"))
            {
                QueueFree();
                
            }
        }
        protected override void Dispose(bool pDisposing)
        {
            instance = null;
            base.Dispose(pDisposing);
        }
    }
}
