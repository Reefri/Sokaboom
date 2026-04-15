using Com.IsartDigital.Utils.Tweens;
using Godot;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban.UI 
{
    public partial class LoginUI : Control
    {
        [Export] private ColorRect realfont;
        [Export] private ColorRect font;
        [Export] private Label title;
        [Export] private LineEdit pseudo;
        [Export] private LineEdit password;
        [Export] private LineEdit confirmPassword;
        [Export] private Button buttonSwitch;
        [Export] private Button buttonValidation;
        [Export] private Label statut;

        [Export] private TextureRect cadenas;

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
                font.Color = new Color(0.62f, 0.62f, 0);

                buttonSwitch.Text = Tr(SWITCH_INSCRIPTION);
                buttonValidation.Text = Tr(VALIDATION_INSCRIPTION);

                title.Text = TEXT_TITLE_INSCRIPTION;

                confirmPassword.Visible = true;
                confirmPassword.Text = null;
            }
            else
            {
                font.Color = new Color(0.52f, 0, 0);

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

        public override void _Input(InputEvent pEvent)
        {
            if (pEvent is InputEventKey lKeyEvent && lKeyEvent.Pressed && lKeyEvent.Keycode == Key.Enter)
            {
                if (pseudo.HasFocus()) password.GrabFocus();
                else if ((password.HasFocus() && isLogin) || confirmPassword.HasFocus()) buttonValidation.GrabFocus();
                else if (password.HasFocus() && !isLogin) confirmPassword.GrabFocus();
            }
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
                    Animation();
                    break;
                case AccountManager.TestConnexionResult.NotFound:
                    statut.Text = Tr(NO_ACCOUNT_A) + pseudo.Text + Tr(NO_ACCOUNT_B);
                    pseudo.GrabFocus();
                    break;
            }
        }

        private void Animation()
        {
            Tween lTween = CreateTween().SetParallel();

            lTween.TweenProperty(cadenas, TweenProp.POSITION, new Vector2(788, 122), 0.2f);
            lTween.TweenProperty(cadenas, TweenProp.ROTATION, Mathf.DegToRad(45), 0.2f);

            lTween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.InOut).SetParallel();

            lTween.TweenProperty(realfont, TweenProp.GLOBAL_POSITION, new Vector2(0, GetWindow().Size.Y), 1f);
            lTween.TweenProperty(realfont, TweenProp.ROTATION, Mathf.DegToRad(-360 * 2), 1f);

            lTween.TweenProperty(cadenas, TweenProp.POSITION, new Vector2(588 + 350, GetWindow().Size.Y), 0.8f).SetDelay(0.2f);
            lTween.TweenProperty(cadenas, TweenProp.ROTATION, Mathf.DegToRad(360), 0.8f).SetDelay(0.2f);

            lTween.Finished += GoOnLogin;
        }

        private void GoOnLogin()
        {
            UIManager.GetInstance().ChangeLayer();
            TitleDoors.GetInstance().ActivateDoors();
            UIManager.GetInstance().GoToTitle();
        }
	}
}
