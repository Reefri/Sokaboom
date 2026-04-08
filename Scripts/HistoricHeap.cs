// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban
{
	public partial class HistoricHeap
	{
		public Level value;
		public HistoricHeap nextValue = null;
		public HistoricHeap previousValue = null;

		public HistoricHeap(Level pValue) 
		{
			value = pValue;
		}
	}
}
