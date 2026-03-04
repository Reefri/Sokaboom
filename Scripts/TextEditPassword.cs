using Godot;
using System;
using System.Globalization;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban.UI.Password
{
	public partial class TextEditPassword : LineEdit
	{
		private void SecretOnPressed()
		{
			if (Secret) Secret = false;
			else Secret = true;
		}
	}
}
