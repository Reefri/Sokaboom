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


        private Dictionary<EnumOfExplosionPattern, Color> enumToColor = new Dictionary<EnumOfExplosionPattern, Color>
        {
            {EnumOfExplosionPattern.Collectible, new Color (1,0,0) },
            {EnumOfExplosionPattern.Bomb,        new Color (1,1,1) },
            {EnumOfExplosionPattern.HUD,         new Color (1,0,0) },
            {EnumOfExplosionPattern.Player,      new Color (1,0,0) },
            {EnumOfExplosionPattern.Mouse,       new Color (1,0,0) }
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

            int lYSize = pExplosionMatrix.Count;
            int lXSize = pExplosionMatrix[0].Count;


            for (int i = 0; i < lYSize; i++)
            {
                for (int j = 0; j < lXSize; j++)
                {
                    if (pExplosionMatrix[i][j] == 2)
                    {
                        originePos = new Vector2I(j, i);

                        pParent.CallDeferred(BombExplosion.ADD_CHILD_DEFERED, enumToCreateMethod[pExplosionPattern].Invoke(
                            (pUseParentPosition ? pParent.GlobalPosition : Vector2.Zero) + (pOffSet ?? Vector2.Zero),
                            enumToColor[pExplosionPattern],
                            pScale
                            ));

                    
                    }
                }
            }

            for (int i = 0; i < lYSize; i++)
            {
                for (int j = 0; j < lXSize; j++)
                {

                    if (pExplosionMatrix[i][j] == 1)
                    {
                        pParent.CallDeferred(BombExplosion.ADD_CHILD_DEFERED, enumToCreateMethod[pExplosionPattern].Invoke(
                            (pUseParentPosition ? pParent.GlobalPosition : Vector2.Zero) + (pOffSet ?? Vector2.Zero) + (new Vector2(j, i) - originePos) * Map.DISTANCE_RANGE, 
                            new Color(1, 1, 1),
                            pScale
                            ));

                      
                    }
                }
            }
        }

        



	}
}
