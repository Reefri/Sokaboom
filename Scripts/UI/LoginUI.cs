using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban.UI 
{
	public partial class LoginUI : Control
	{
        [Export] private ColorRect font;
        [Export] private Label title;
        [Export] private LineEdit pseudo;
        [Export] private LineEdit password;
        [Export] private LineEdit confirmPassword;
        [Export] private Button buttonSwitch;
        [Export] private Button buttonValidation;

        private const string TEXT_SWITCH_INSCRIPTION = "Already Register";
        private const string TEXT_VALIDATION_INSCRIPTION = "Register You";
        private const string TEXT_TITLE_INSCRIPTION = "Inscription :";

        private const string TEXT_SWITCH_LOGIN = "Inscription";
        private const string TEXT_VALIDATION_LOGIN = "Connect You";
        private const string TEXT_TITLE_LOGIN = "Login :";

        private bool isLogin = true;

        public override void _Ready()
		{
            confirmPassword.Visible = false;

            buttonValidation.ButtonDown += OnValidationPressed;
        }

        private void SwitchOnPressed()
        {
            if (isLogin)
            {
                isLogin = false;

                font.Color = new Color(0, 0.85f, 0.97f);
                buttonSwitch.SelfModulate = new Color(0, 0, 0.39f);

                buttonSwitch.Text = TEXT_SWITCH_INSCRIPTION;
                buttonValidation.Text = TEXT_VALIDATION_INSCRIPTION;

                title.Text = TEXT_TITLE_INSCRIPTION;

                confirmPassword.Visible = true;
                confirmPassword.Text = null;
            }
            else 
            {
                isLogin = true;

                font.Color = new Color(0, 0, 0.39f);
                buttonSwitch.SelfModulate = new Color(0, 0.85f, 0.97f);

                buttonSwitch.Text = TEXT_SWITCH_LOGIN;
                buttonValidation.Text = TEXT_VALIDATION_LOGIN;

                title.Text = TEXT_TITLE_LOGIN;

                confirmPassword.Visible = false;
                
            }

            pseudo.Text = null;
            password.Text = null;
            password.Secret = true;
            confirmPassword.Secret = true;
        }

        private void OnValidationPressed()
        {
            if (isLogin)
            {
                Login();
            }
            else
            {
                Register();
            }
        }

        private void Register()
        {
            if (password.Text != confirmPassword.Text)
            {
                GD.Print("Le mot de passe ne correspond pas à la confirmation.");
                return;
            }

            if (AccountManager.GetInstance().Register(pseudo.Text,password.Text))
            {
                GD.Print("Enregistrement réussi !");
            }
        }

        private void Login()
        {
            switch (AccountManager.GetInstance().TestConnexion(pseudo.Text,password.Text))
            {
                case AccountManager.TestConnexionResult.Incorrect:
                    GD.Print("Votre mot de passe est incorrecte.");
                    break;
                case AccountManager.TestConnexionResult.Valid:
                    GD.Print("Connexion réussi ! Bienvenue "+pseudo.Text);
                    break;
                case AccountManager.TestConnexionResult.NotFound:
                    GD.Print("Aucun compte n'a été trouvé avec le pseudo : "+pseudo.Text+". Si vous n'avez pas encore de compte, créez s'en un !");

                    break;
            }
        }
	}
}
