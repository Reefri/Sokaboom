using Godot;
using System;
using System.Globalization;

// Author : Ethan Masse

namespace Com.IsartDigital.Sokoban.UI
{
	public partial class TextEditPassword : LineEdit
	{
		/*
		public override void _Ready()
		{

		}

		public override void _Process(double pDelta)
		{
			float lDelta = (float)pDelta;

		}
		*/
		private void OnPressed()
		{
			if (Secret) Secret = false;
			else Secret = true;
		}

		/*
        protected override void Dispose(bool pDisposing)
		{

		}
		*/
	}
}
