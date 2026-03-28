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

        
        public override void _Ready()
		{
            base._Ready();

            GD.Print("a");

            int i = 0;

            foreach (Button lButtons in allButtons.GetChildren())
			{
                int lLevelID = i;

                if (GridManager.GetInstance().numberOfLevel == i) break;


                lButtons.Disabled = !AccountManager.GetInstance().currentAccount.LockedLevels[i];
                lButtons.Pressed += () => GoToLevel(lLevelID);
                i++;
            }

            TreeEntered += UpdateLevelSelect;
        }

        
        private void UpdateLevelSelect()
        {
            foreach (Button lButton in allButtons.GetChildren())
            {
                if (lButton.GetIndex() >= GridManager.GetInstance().numberOfLevel) lButton.Disabled = true; //faire en sorte que personne n'essaye d'aller dans les niveau pas créé)
                else if (lButton.GetIndex() != 0) lButton.Disabled = buttonlock && !AccountManager.GetInstance().currentAccount.LockedLevels[lButton.GetIndex()];
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
            buttonlock = !buttonlock;

            UpdateLevelSelect();
        }

        private void GoToHelp()
        {
            UIManager.GetInstance().GoToHelp();
            UIManager.GetInstance().comeToMenu = false;
        }
	}
}
