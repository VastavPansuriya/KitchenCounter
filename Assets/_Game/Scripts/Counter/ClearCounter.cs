using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjSO;

    public override void Interact(Player player)
    {
        //Clear Counter don't Have kitchen Object
        if (!HasKitchenObject())
        {
            //Player Have kitchen Object
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {

            }
        }
        //Clear Counter Have kitchen Object
        else
        {
            //Player don't Have kitchen Object
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);

                // if player carying plate

            }
            else
            {
                // Player have plate 
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {

                    if (plateKitchenObject.TryAddIngerdiant(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                // don't have plate
                else
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngerdiant(player.GetKitchenObject().GetKitchenObjectSo()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
        }
    }



}
