using Godot;
using System;

// Author : Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class SoundsSettingsMenu : TextureRect
	{
        [Export] private Button backToTitleButton;

		public override void _Ready()
		{
			base._Ready();

            backToTitleButton.Pressed += BackToTitle;
        }

        private void BackToTitle()
        {
            UIManager.GetInstance().GoToTitle();
            SoundManager.GetInstance().PlayClick();
        }
        public override void _Process(double pDelta)
		{
			base._Process(pDelta);
			float lDelta = (float)pDelta;
		}

		protected override void Dispose(bool pDisposing)
		{
			base.Dispose(pDisposing);
		}
	}
}
