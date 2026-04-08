using Godot;
using System.Collections.Generic;

// Author : Ethan Masse / Ethan Frenard

namespace Com.IsartDigital.Sokoban 
{
	public partial class HelpMenu : Control
	{
		[Export] private Button nextPageButton;
		[Export] private Button previousPageButton;
		[Export] private Label currentPage;

        [Export] private ColorRect firstPage;
        [Export] private ColorRect secondPage;
        [Export] private ColorRect thirdPage;

        private List<ColorRect> pages;

        private int totalNumberOfPages;
        private int pageShown = 1;

        [Export] private Label explanationSfB;
        [Export] private Label explanationSfC;
        [Export] private Label explanationControlA;
        [Export] private Label explanationControlB;
        [Export] private Label explanationDetailA;
        [Export] private Label explanationDetailB;

        private const string CONTROL_AA = "ID_CONTROL_AA";
        private const string CONTROL_AB = "ID_CONTROL_AB";
        private const string CONTROL_BA = "ID_CONTROL_BA";
        private const string CONTROL_BB = "ID_CONTROL_BB";
        private const string CONTROL_BC = "ID_CONTROL_BC";
        private const string SF_BA = "ID_SF_BA";
        private const string SF_BB = "ID_SF_BB";
        private const string SF_CA = "ID_SF_CA";
        private const string SF_CB = "ID_SF_CB";
        private const string DETAILS_AA = "ID_DETAILS_AA";
        private const string DETAILS_AB = "ID_DETAILS_AB";
        private const string DETAILS_BA = "ID_DETAILS_BA";
        private const string DETAILS_BB = "ID_DETAILS_BB";

        [Export] private Button returnButton;

        private const string GO_MENU = "ID_MENU";
        private const string GO_LEVEL = "ID_START";

        public override void _Ready()
        {
            base._Ready();

            pages = new List<ColorRect>() { firstPage, secondPage, thirdPage };
            totalNumberOfPages = pages.Count;

            ActualizeCurrentPageNumber();
            ShowCurrentPage();

            nextPageButton.Pressed += NextPage;
            previousPageButton.Pressed += PreviousPage;

            explanationControlA.Text = Tr(CONTROL_AA) + "\n" + Tr(CONTROL_AB).Replace("@", ",");
            explanationControlB.Text = Tr(CONTROL_BA).Replace("@", ",") + "\n" + Tr(CONTROL_BB) +"\n \n \n" + Tr(CONTROL_BC);
            explanationSfB.Text = Tr(SF_BA) + "\n" + Tr(SF_BB);
            explanationSfC.Text = Tr(SF_CA) + "\n" + Tr(SF_CB);
            explanationDetailA.Text = Tr(DETAILS_AA) + "\n" + Tr(DETAILS_AB);
            explanationDetailB.Text = Tr(DETAILS_BA) + "\n \n" + Tr(DETAILS_BB).Replace("@", ",");

            if (UIManager.GetInstance().comeToMenu) 
            {
                returnButton.Pressed += UIManager.GetInstance().GoToTitle;
                returnButton.Text = Tr(GO_MENU);
            }
            else
            {
                returnButton.Pressed += () => UIManager.GetInstance().GoToLevel(0);
                returnButton.Text = Tr(GO_LEVEL);
            }
        }

        private void PreviousPage()
        {
            if (pageShown - 1 <= 0) pageShown = 3;
            else pageShown--;
            ShowCurrentPage();
        }

        private void NextPage()
        {
            if (pageShown + 1 > totalNumberOfPages) pageShown = 1;
            else pageShown++;
            ShowCurrentPage();
        }

        private void ShowCurrentPage()
        {
            int lNumberOfPages = pages.Count;
            for (int i = 0; i < lNumberOfPages; i++)
            {
                pages[i].Visible = false;

            }
            pages[pageShown - 1].Visible = true;

            ActualizeCurrentPageNumber();
        }

        private void ActualizeCurrentPageNumber()
        {
            currentPage.Text = pageShown.ToString() + "/" + totalNumberOfPages.ToString();
        }

	}
}
