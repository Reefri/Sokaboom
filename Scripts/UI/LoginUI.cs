using Godot;
using System;
using System.Runtime.CompilerServices;

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
        [Export] private Label statut;

        private const string TEXT_TITLE_INSCRIPTION = "Inscription :";

        private const string TEXT_SWITCH_LOGIN = "Inscription";

        private Color green = new Color(0, 1, 0);
        private Color yellow = new Color(1, 1, 0);
        private Color red = new Color(1, 0, 0);

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
                font.Color = new Color(0, 0.85f, 0.97f);
                buttonSwitch.SelfModulate = new Color(0, 0, 0.39f);

                buttonSwitch.Text = Tr("ID_SWITCH_INSCRIPTION");
                buttonValidation.Text = Tr("ID_VALIDATION_INSCRIPTION");

                title.Text = TEXT_TITLE_INSCRIPTION;

                confirmPassword.Visible = true;
                confirmPassword.Text = null;
            }
            else
            {
                font.Color = new Color(0, 0, 0.39f);
                buttonSwitch.SelfModulate = new Color(0, 0.85f, 0.97f);

                buttonSwitch.Text = TEXT_SWITCH_LOGIN;
                buttonValidation.Text = Tr("ID_VALIDATION_LOGIN");

                title.Text = Tr("ID_TITLE_LOGIN");

                confirmPassword.Visible = false;

            }

            isLogin = !isLogin;

            pseudo.Text = null;
            password.Text = null;
            password.Secret = true;
            confirmPassword.Secret = true;
        }

        private void OnValidationPressed()
        {
            if (isLogin) Login();
            else Register();
        }

        private void Register()
        {
            if (password.Text != confirmPassword.Text)
            {
                statut.Text = Tr("ID_PASSWORD_NOT_CONFIRM");
                statut.SelfModulate = yellow;
                return;
            }
            if (AccountManager.GetInstance().Register(pseudo.Text, password.Text))
            {
                statut.Text = Tr("ID_PASSWORD_CONFIRM");
                statut.SelfModulate = green;
                Login();
            }
            else
            {
                statut.Text = Tr("ID_PSEUDO_EXIST_A") + pseudo.Text + Tr("ID_PSEUDO_EXIST_B");
                statut.SelfModulate = yellow;
            }
        }

        private void Login()
        {
            switch (AccountManager.GetInstance().TestConnexion(pseudo.Text, password.Text))
            {
                case AccountManager.TestConnexionResult.Incorrect:
                    statut.Text = Tr("ID_INCORECT_PASSWORD");
                    statut.SelfModulate = red;
                    break;
                case AccountManager.TestConnexionResult.Valid:
                    statut.Text = Tr("ID_CONNECTION_SUCESS") + pseudo.Text;
                    statut.SelfModulate = green;
                    UIManager.GetInstance().GoToTitle();
                    break;
                case AccountManager.TestConnexionResult.NotFound:
                    statut.Text = Tr("ID_NO_ACCOUNT_A") + pseudo.Text + Tr("ID_NO_ACCOUNT_B");
                    statut.SelfModulate = red;
                    break;
            }
        }
	}
}
