using Godot;
using Com.IsartDigital.Sokoban;

// Author : Ethan FRENARD / Ethan MASSE

namespace Com.IsartDigital.UI {
	
	public partial class LevelSelect : Control
    {
		[Export] private Control allButtons;

        [Export] private PackedScene spiral;

        private bool buttonlock = true;

        private const string LEVEL = "ID_LEVEL";

        private Node2D particles;
        private Button selectedButton;

        public override void _Ready()
		{
            base._Ready();

            particles = (Node2D)spiral.Instantiate();
            particles.Visible = false;
            AddChild(particles);

            int i = 0;

            foreach (Button lButtons in allButtons.GetChildren())
			{
                int lLevelID = i;

                if (GridManager.GetInstance().numberOfLevel > i) lButtons.Disabled = !AccountManager.GetInstance().currentAccount.LockedLevels[i];

                lButtons.Text = Tr(LEVEL) + lLevelID;
                lButtons.Pressed += () => GoToLevel(lLevelID);
                i++;

            }

            TreeEntered += UpdateLevelSelect;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            foreach (Button lButton in allButtons.GetChildren())
            {

                if (lButton.IsHovered() && !lButton.Disabled)
                {
                    selectedButton = lButton;

                    particles.Reparent(lButton);
                    particles.Visible = true;
                    particles.ShowBehindParent = true;
                    particles.Rotation = lButton.Rotation;
                    particles.Scale = lButton.Scale;
                }
                if(selectedButton != null)
                particles.GlobalPosition = selectedButton.GlobalPosition + selectedButton.Size/2;
            }
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
            MenuTransition.Create();
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
