using Godot;
using System.Net.Sockets;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban.UI.Password
{
	public partial class TextEditConfirm : LineEdit
    {
        private void SecretOnPressed()
        {
            if (Secret) Secret = false;
            else Secret = true;
        }
    }
}
