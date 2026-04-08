using Godot;
using System.Reflection;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class Fade : Control
	{
        [Export] private ColorRect blackScreen;
        [Export] public Timer animationTimer;
        [Export] public bool queufreeWhenOver = false;
        [Export] public bool fadeIn = true;
        public bool animationFinished = false;
        public override void _Ready()
        {
            Visible = true;
            animationTimer.Start();
            animationTimer.Timeout += Finished;
        }

        private void Finished()
        {
            animationFinished = true;
            if (queufreeWhenOver) QueueFree();
        }

        public override void _Process(double pDelta)
        {
            float lDelta = (float)pDelta;
            if (fadeIn)
                blackScreen.Modulate = new Color(0, 0, 0, (float)animationTimer.TimeLeft / (float)animationTimer.WaitTime);
            else blackScreen.Modulate = new Color(0, 0, 0, 1 - (float)animationTimer.TimeLeft / (float)animationTimer.WaitTime);
        }

        protected override void Dispose(bool pDisposing)
        {

        }
    }
}
