using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

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

        [Export] private Label explinationSfB;
        [Export] private Label explinationSfC;
        [Export] private Label explinationControlA;
        [Export] private Label explinationControlB;
        [Export] private Label explinationDetailA;
        [Export] private Label explinationDetailB;

        public override void _Ready()
        {
            base._Ready();

            pages = new List<ColorRect>() { firstPage, secondPage, thirdPage };
            totalNumberOfPages = pages.Count;

            ActualizeCurrentPageNumber();
            ShowCurrentPage();

            nextPageButton.Pressed += NextPage;
            previousPageButton.Pressed += PreviousPage;

            //explinationControlA.Text = Tr("ID_CONTROL_AA") + "\n" + Tr("ID_CONTROL_AB");
            //explinationControlB.Text = Tr("ID_CONTROL_BA") + "\n" + Tr("ID_CONTROL_BB") +"\n \n \n" + Tr("ID_CONTROL_BC");
            //explinationSfB.Text = Tr("ID_SF_BA") + "\n" + Tr("ID_SF_BB");
            //explinationSfC.Text = Tr("ID_SF_CA") + "\n" + Tr("ID_SF_CB");
            //explinationDetailA.Text = Tr("ID_DETAILS_AA") + "\n" + Tr("ID_DETAILS_AB");
            //explinationDetailB.Text = Tr("ID_DETAILS_BA") + "\n \n" + Tr("ID_DETAILS_BB");
        }
        private void ReturnPressed()
		{
			if (UIManager.GetInstance().comeToMenu) UIManager.GetInstance().GoToTitle();
			else UIManager.GetInstance().GoToLevel(0);
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
            for (int i = 0; i < pages.Count; i++)
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
