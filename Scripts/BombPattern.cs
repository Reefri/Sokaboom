using Godot;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombPattern
	{


        public Vector2I originePos;


        public BombPattern(Node2D pParent, bool pDoesExplose, List<List<int>> pExplosionMatrix, bool pUseParentPosition = true, Vector2? pOffSet = null, bool pIsCollectiblePattern = false) 
		{

            originePos = Vector2I.Zero;

            for (int i = 0; i < pExplosionMatrix.Count; i++)
            {
                for (int j = 0; j < pExplosionMatrix[i].Count; j++)
                {
                    if (pExplosionMatrix[i][j] == 2)
                    {
                        if (!pIsCollectiblePattern)
                        {
                            originePos = new Vector2I(j, i);
                            pParent.CallDeferred("add_child", ToPlaceOnExplosion.Create((pUseParentPosition ? pParent.GlobalPosition : Vector2.Zero) + (pOffSet ?? Vector2.Zero), new Color(1, 0, 0), pDoesExplose));
                        }
                        else
                        {
                            originePos = new Vector2I(j, i);
                            pParent.CallDeferred("add_child", BombCollectiblePattern.Create((pUseParentPosition ? pParent.GlobalPosition : Vector2.Zero) + (pOffSet ?? Vector2.Zero), new Color(1, 0, 0), pDoesExplose));

                        }
                    }
                }
            }

            for (int i = 0; i < pExplosionMatrix.Count; i++)
            {
                for (int j = 0; j < pExplosionMatrix[i].Count; j++)
                {

                    if (pExplosionMatrix[i][j] == 1)
                    {
                        if (!pIsCollectiblePattern)
                        {

                            Vector2 lPosition = (pUseParentPosition ? pParent.GlobalPosition : Vector2.Zero) + (pOffSet ?? Vector2.Zero) + (new Vector2(j, i) - originePos) * States.DISTANCE_RANGE;
                            pParent.CallDeferred("add_child", ToPlaceOnExplosion.Create(lPosition, new Color(1, 1, 1), pDoesExplose));
                        }
                        else
                        {
                            Vector2 lPosition = (pUseParentPosition ? pParent.GlobalPosition : Vector2.Zero) + (pOffSet ?? Vector2.Zero) + (new Vector2(j, i) - originePos) * States.DISTANCE_RANGE;
                            pParent.CallDeferred("add_child", BombCollectiblePattern.Create(lPosition, new Color(1, 1, 1), pDoesExplose));
                        
                        }
                    }
                }
            }
        }

        



	}
}
