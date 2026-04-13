// Author : Gramatikoff Sacha
using Com.IsartDigital.Sokoban;
using Godot;
using Godot.Collections;

namespace Com.IsartDigital.Chromaberation {
	
	public partial class ColorFilterPanel : Control
	{

        public static Color NORMAL_COLOR = new Color(1, 1, 1);
        public static Color COLORBLIND_COLOR = new Color(0.5f, 0.7f, 1);
        public static Color CORRECTEDCOLORBLIND_COLOR = new Color(0.7f, 0.5f, 0.35f) / 0.7f;

        [Export] private Button mainButton;
        [Export] private Panel panel;

        [Export] protected Array<Button> buttons;

        


        public override void _Ready()
		{
            panel.ZIndex = 100;

            mainButton.ButtonDown += ToggleShowPanel;
            panel.Hide();

            buttons[0].ButtonDown += MakeColorblind;
            buttons[1].ButtonDown += CureColorblind;
            buttons[2].ButtonDown += BackToNormal;
        }


        protected void ToggleShowPanel()
        {
            panel.Visible = !panel.Visible;
        }

        private void MakeColorblind()
        {
            ColorBlind.GetInstance().SetShaderColor(COLORBLIND_COLOR);
        }

        private void CureColorblind()
        {
            ColorBlind.GetInstance().SetShaderColor(CORRECTEDCOLORBLIND_COLOR);

        }

        private void BackToNormal()
        {
            ColorBlind.GetInstance().SetShaderColor(NORMAL_COLOR);

        }


    }
}
