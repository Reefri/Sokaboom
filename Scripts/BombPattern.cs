using Godot;
using System.Collections.Generic;

// Author : Sacha Gramatikoff

namespace Com.IsartDigital.Sokoban 
{
	public partial class BombPattern
	{


        public Vector2I originePos;

        public delegate Node SimpleDelegate(Vector2 pPosition, Color pColor, float pScale=1);

        private static SimpleDelegate ToPlaceOnExplosionCreate = ToPlaceOnExplosion.Create;
        private static SimpleDelegate ToPlaceBombCreate = ToPlaceOnCollectible.Create;
        private static SimpleDelegate ToPlaceOnHUDCreate = ToPlaceOnHUD.Create;
        private static SimpleDelegate ToPlaceOnPlayerCreate = ToPlaceOnPlayer.Create;
        private static SimpleDelegate ToPlaceOnMouseCreate = ToPlaceOnMousePrevisu.Create;

        public enum EnumOfExplosionPattern
        {
            Collectible,
            Bomb,
            HUD,
            Player,
            Mouse
        };

 
        private static Dictionary<EnumOfExplosionPattern, SimpleDelegate> enumToCreateMethod = new Dictionary<EnumOfExplosionPattern, SimpleDelegate>
        {
            {EnumOfExplosionPattern.Collectible, ToPlaceBombCreate },
            {EnumOfExplosionPattern.Bomb,        ToPlaceOnExplosionCreate },
            {EnumOfExplosionPattern.HUD,         ToPlaceOnHUDCreate },
            {EnumOfExplosionPattern.Player,      ToPlaceOnPlayerCreate },
            {EnumOfExplosionPattern.Mouse,       ToPlaceOnMouseCreate }
        };

        


        public BombPattern(Node2D pParent, List<List<int>> pExplosionMatrix, EnumOfExplosionPattern pExplosionPattern , bool pUseParentPosition = true, Vector2? pOffSet = null, float pScale = 1) 
		{




            originePos = Vector2I.Zero;

            for (int i = 0; i < pExplosionMatrix.Count; i++)
            {
                for (int j = 0; j < pExplosionMatrix[i].Count; j++)
                {
                    if (pExplosionMatrix[i][j] == 2)
                    {
                        originePos = new Vector2I(j, i);

                        pParent.CallDeferred("add_child", enumToCreateMethod[pExplosionPattern].Invoke((pUseParentPosition ? pParent.GlobalPosition : Vector2.Zero) + (pOffSet ?? Vector2.Zero), new Color(1,0,0),pScale));

                    
                    }
                }
            }

            for (int i = 0; i < pExplosionMatrix.Count; i++)
            {
                for (int j = 0; j < pExplosionMatrix[i].Count; j++)
                {

                    if (pExplosionMatrix[i][j] == 1)
                    {
                        pParent.CallDeferred("add_child", enumToCreateMethod[pExplosionPattern].Invoke(
                            (pUseParentPosition ? pParent.GlobalPosition : Vector2.Zero) + (pOffSet ?? Vector2.Zero) + (new Vector2(j, i) - originePos) * States.DISTANCE_RANGE, 
                            new Color(1, 1, 1),
                            pScale
                            ));

                      
                    }
                }
            }
        }

        



	}
}
