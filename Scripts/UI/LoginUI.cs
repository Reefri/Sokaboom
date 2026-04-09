using Godot;

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

        private bool isLogin = true;

        private const string SWITCH_INSCRIPTION = "ID_SWITCH_INSCRIPTION";
        private const string VALIDATION_INSCRIPTION = "ID_VALIDATION_INSCRIPTION";
        private const string VALIDATION_LOGIN = "ID_VALIDATION_LOGIN";
        private const string TITLE_LOGIN = "ID_TITLE_LOGIN";
        private const string PASSWORD_NOT_CONFIRM = "ID_PASSWORD_NOT_CONFIRM";
        private const string PSEUDO_EXIST_A = "ID_PSEUDO_EXIST_A";
        private const string PSEUDO_EXIST_B = "ID_PSEUDO_EXIST_B";
        private const string INCORECT_PASSWORD = "ID_INCORECT_PASSWORD";
        private const string NO_ACCOUNT_A = "ID_NO_ACCOUNT_A";
        private const string NO_ACCOUNT_B = "ID_NO_ACCOUNT_B";

        public override void _Ready()
        {
            confirmPassword.Visible = false;

            buttonValidation.ButtonDown += OnValidationPressed;

            pseudo.GrabFocus();
        }

        private void SwitchOnPressed()
        {
            if (isLogin)
            {
                font.Color = new Color(0, 0.85f, 0.97f);
                buttonSwitch.SelfModulate = new Color(0, 0, 0.39f);

                buttonSwitch.Text = Tr(SWITCH_INSCRIPTION);
                buttonValidation.Text = Tr(VALIDATION_INSCRIPTION);

                title.Text = TEXT_TITLE_INSCRIPTION;

                confirmPassword.Visible = true;
                confirmPassword.Text = null;
            }
            else
            {
                font.Color = new Color(0, 0, 0.39f);
                buttonSwitch.SelfModulate = new Color(0, 0.85f, 0.97f);

                buttonSwitch.Text = TEXT_SWITCH_LOGIN;
                buttonValidation.Text = Tr(VALIDATION_LOGIN);

                title.Text = Tr(TITLE_LOGIN);

                confirmPassword.Visible = false;

            }

            isLogin = !isLogin;
            pseudo.GrabFocus();

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
                confirmPassword.GrabFocus();
                statut.Text = Tr(PASSWORD_NOT_CONFIRM);
                return;
            }
            if (AccountManager.GetInstance().Register(pseudo.Text, password.Text))
            {
                Login();
            }
            else
            {
                pseudo.GrabFocus();
                statut.Text = Tr(PSEUDO_EXIST_A) + pseudo.Text + Tr(PSEUDO_EXIST_B);
            }
        }

        private void Login()
        {
            switch (AccountManager.GetInstance().TestConnexion(pseudo.Text, password.Text))
            {
                case AccountManager.TestConnexionResult.Incorrect:
                    statut.Text = Tr(INCORECT_PASSWORD);
                    password.GrabFocus();
                    break;
                case AccountManager.TestConnexionResult.Valid:
                    UIManager.GetInstance().ChangeLayer();
                    TitleDoors.GetInstance().ActivateDoors();
                    UIManager.GetInstance().GoToTitle();
                    break;
                case AccountManager.TestConnexionResult.NotFound:
                    statut.Text = Tr(NO_ACCOUNT_A) + pseudo.Text + Tr(NO_ACCOUNT_B);
                    pseudo.GrabFocus();
                    break;
            }
        }
	}
}
