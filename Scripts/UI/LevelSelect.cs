using Godot;
using Com.IsartDigital.Sokoban;
using System.Collections.Generic;
using System.Linq;
using System;

// Author : Ethan FRENARD / Ethan MASSE

namespace Com.IsartDigital.UI {
	
	public partial class LevelSelect : Control
    {
		[Export] private Control allButtons;
        [Export] private Button backButton;

        private bool buttonlock = true;

        private const string LEVEL = "ID_LEVEL";

    

        public override void _Ready()
		{
            base._Ready();

            int i = 0;

            List<Node> lListButtons = allButtons.GetChildren().ToList();


            foreach (Button lButtons in lListButtons)
			{
                int lLevelID = i;

                Control lStars = (Control)lButtons.GetChild(0);
                for (int j = 0; j < GridManager.GetInstance().CreateStars(lLevelID); j++)
                {
                    AnimatedSprite2D lStar = (AnimatedSprite2D)lStars.GetChild(j);
                    lStar.Frame = 1;
                }

                if (GridManager.GetInstance().numberOfLevel > i) lButtons.Disabled = !AccountManager.GetInstance().currentAccount.LockedLevels[i];

                lButtons.Text = Tr(LEVEL) + lLevelID;
                lButtons.Pressed += () => GoToLevel(lLevelID);
                i++;
            }

            TreeEntered += UpdateLevelSelect;

            backButton.Pressed += BackPressed;
        }


        private void UpdateLevelSelect()
        {
            List<Node> lListButtons = allButtons.GetChildren().ToList();

            foreach (Button lButton in lListButtons)
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
            MenuTransition.Create(UIManager.GetInstance().GoToTitle);
        }

        private void UnlockPressed()
        {
            buttonlock = !buttonlock;

            UpdateLevelSelect();
        }

        private void GoToHelp()
        {
            UIManager.GetInstance().comeToMenu = false;
            UIManager.GetInstance().GoToHelp();
        }
	}
}
