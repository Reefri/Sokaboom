using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban.UI 
{
	public partial class Login : Control
	{
		[Export] private Control menuInscription;
        [Export] private Control menuLogin;
        [Export] private LineEdit pseudoLogin;
        [Export] private LineEdit passwordLogin;
        [Export] private LineEdit pseudoInscription;
        [Export] private LineEdit passwordInscription;

        public override void _Ready()
		{
            menuLogin.Visible = true;
            menuInscription.Visible = false;
        }

        /*
		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

		}
        */
        private void InscriptionOnPressed()
        {
			menuInscription.Visible = true;
            menuLogin.Visible = false;

            passwordLogin.Text = null;
            pseudoLogin.Text = null;
        }

        private void LoginOnPressed()
        {
            menuLogin.Visible = true;
            menuInscription.Visible = false;

            pseudoInscription.Text = null;
            passwordInscription.Text = null;
        }
        /*
        protected override void Dispose(bool pDisposing)
		{

		}
        */
	}
}
