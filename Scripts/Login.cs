using Godot;
using System;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban.UI 
{
	public partial class Login : Control
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

        private bool login = true;

        public override void _Ready()
		{
            confirmPassword.Visible = false;
        }

        private void SwitchOnPressed()
        {
            if (login)
            {
                login = false;

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
                login = true;

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
	}
}
