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

        public override void _Ready()
        {
            base._Ready();

            pages = new List<ColorRect>() { firstPage, secondPage, thirdPage };
            totalNumberOfPages = pages.Count;

            ActualizeCurrentPageNumber();
            ShowCurrentPage();

            nextPageButton.Pressed += NextPage;
            previousPageButton.Pressed += PreviousPage;
        }
        private void ReturnPressed()
		{
			if (UIManager.GetInstance().comeToMenu) UIManager.GetInstance().GoToTitle();
			else UIManager.GetInstance().GoToLevel(0);
		}

        private void PreviousPage()
        {
            if (pageShown - 1 <= 0) return;
            pageShown--;
            ShowCurrentPage();
        }

        private void NextPage()
        {
            if (pageShown + 1 > totalNumberOfPages) return;
            pageShown++;
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
