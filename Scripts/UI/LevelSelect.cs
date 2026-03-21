using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.Sokoban;

// Author : Ethan FRENARD / Ethan MASSE

namespace Com.IsartDigital.UI {
	
	public partial class LevelSelect : CanvasLayer
	{
        [Export] private Button backButton;
		[Export] private Button unlockButton;

		[Export] private Control allButtons;

        private bool buttonlock = true;

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

            backButton.Pressed += UIManager.GetInstance().GoToTitle;
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
            if (pIndex == 0) GoToHelp();
            else UIManager.GetInstance().GoToLevel(pIndex);
        }

        private void Unlock()
        {
            if (buttonlock) buttonlock = false;
            else buttonlock = true;

            foreach (Button lButton in allButtons.GetChildren())
			{
				if (lButton.GetIndex() > 0) lButton.Disabled = buttonlock;
            }
        }

        private void GoToHelp()
        {
            UIManager.GetInstance().GoToHelp();
            UIManager.GetInstance().comeToMenu = false;
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
