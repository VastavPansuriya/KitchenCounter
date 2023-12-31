using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{

    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjSO;


    public override void Interact(Player player)
    {
        // Player Don't have any Object
        if (!player.HasKitchenObject())
        {
            Debug.Log("Interact!", gameObject);
            KitchenObject.SpawnKitchenObject(kitchenObjSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
