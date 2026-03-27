using Godot;
using System.Net.Sockets;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban 
{
	public partial class SecretButton : Button
	{
		public override void _Ready()
		{
			Pressed += ChangeSecret;
		}

		private void ChangeSecret()
		{
			LineEdit lPassword = (LineEdit)GetParent();
            lPassword.Secret = !lPassword.Secret;
        }
	}
}
