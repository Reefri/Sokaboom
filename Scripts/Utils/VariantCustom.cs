using Godot;
using GodotDict = Godot.Collections.Dictionary;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class VariantCustom
	{
		public GodotDict content;

		public VariantCustom(Variant pVariant)
		{
			content = (GodotDict)pVariant;
			//GD.Print("########");
			//GD.Print(pVariant.GetType());
			//GD.Print(pVariant);
			//GD.Print(content);
			//GD.Print("########");
		}

		public VariantCustom At(string pKey)
		{

            return new VariantCustom(content[pKey]);
		}

		public VariantCustom At(int pKey)
		{
            return new VariantCustom(content[pKey]);
		}

		public void Print()
		{
			GD.Print(content); 
		}

		public override string ToString()
		{
			return content.ToString();
		}

	}
}
