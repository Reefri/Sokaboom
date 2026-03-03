using Godot;
using System;
using System.Collections.Generic;

// Author : Ethan FRENARD

namespace Com.IsartDigital.ProjectName {
	
	public partial class LevelSelect : CanvasLayer
	{
		private const string TITLE_SCREEN_PATH = "";

		[Export] private Button backButton;
		[Export] private Button unlockButton;

		[Export] private Node2D allButtons;

		static private LevelSelect instance;

		private LevelSelect() { }

		static public LevelSelect GetInstance()
		{
			if (instance == null) instance = new LevelSelect();
			return instance;

		}

		public override void _Ready()
		{
			if (instance != null)
			{
				QueueFree();
				GD.Print(nameof(LevelSelect) + " Instance already exist, destroying the last added.");
				return;
			}

			instance = this;


            backButton.Pressed += BackToTitle;
            unlockButton.Pressed += Unlock;

			
            foreach(Button lButtons in allButtons.GetChildren())
			{
				lButtons.Pressed += GoToLevel;
			}

		}

        private void GoToLevel()
        {
			GD.Print("Test");
        }

        private void Unlock()
        {
			foreach(Button lButton in allButtons.GetChildren())
			{
				lButton.Disabled = false;
			}
        }

        private void BackToTitle()
        {
			//GetTree().ChangeSceneToFile(TITLE_SCREEN_PATH);
        }



        public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
