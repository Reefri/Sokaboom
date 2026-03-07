using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.Sokoban;

// Author : Ethan FRENARD / Ethan MASSE

namespace Com.IsartDigital.UI {
	
	public partial class LevelSelect : CanvasLayer
	{
		private const string TITLE_SCREEN_PATH = "res://Scenes/TitleCard.tscn";

		[Export] private Button backButton;
		[Export] private Button unlockButton;

		[Export] private Control allButtons;

        static private LevelSelect instance;
        static private PackedScene factory = GD.Load<PackedScene>("res://Scenes/LevelSelect.tscn");
        private LevelSelect() : base()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(LevelSelect) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;
        }

        static public LevelSelect GetInstance()
        {
            if (instance == null) instance = (LevelSelect)factory.Instantiate();
            return instance;
        }

        public override void _Ready()
		{
            base._Ready();
            int i = 0;

            backButton.Pressed += BackToTitle;
            unlockButton.Pressed += Unlock;

            foreach (Button lButtons in allButtons.GetChildren())
			{
                int lLevelID = i;
                lButtons.Pressed += () => GoToLevel(lLevelID);
                i++;
            }
        }

        private void GoToLevel(int pIndex)
        {
			GD.Print(pIndex);
            //GridManager.GetInstance().ChangeLevel(pIndex);
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
			GetTree().ChangeSceneToFile(TITLE_SCREEN_PATH);
        }

        public override void _Process(double pDelta)
		{
            base._Process(pDelta);
            float lDelta = (float)pDelta;
		}

		protected override void Dispose(bool pDisposing)
		{
			instance = null;
			base.Dispose(pDisposing);
		}
	}
}
