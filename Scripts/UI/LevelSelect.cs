using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.Sokoban;

// Author : Ethan FRENARD / Ethan MASSE

namespace Com.IsartDigital.UI {
	
	public partial class LevelSelect : Control
    {
		[Export] private Control allButtons;

        private bool buttonlock = true;

        private int numberOfLevel = 3; //petite sécurité à enlever plus tard

        public override void _Ready()
		{
            base._Ready();
            int i = 0;

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

        private void BackPressed()
        {
            UIManager.GetInstance().GoToTitle();
        }

        private void UnlockPressed()
        {
            if (buttonlock) buttonlock = false;
            else buttonlock = true;

            foreach (Button lButton in allButtons.GetChildren())
			{
                if (lButton.GetIndex() > numberOfLevel) lButton.Disabled = true; //faire en sorte que personne n'essaye d'aller dans les niveau pas créé)
                else if (lButton.GetIndex() > 0) lButton.Disabled = buttonlock;
            }
        }

        private void GoToHelp()
        {
            UIManager.GetInstance().GoToHelp();
            UIManager.GetInstance().comeToMenu = false;
        }
	}
}
