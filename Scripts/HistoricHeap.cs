using Godot;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban
{
	public partial class HistoricHeap
	{

		public LevelScreenShot value;
		public HistoricHeap nextValue = null;
		public HistoricHeap previousValue = null;

		public HistoricHeap(LevelScreenShot pValue) 
		{
			value = pValue;
		}
	}
}
