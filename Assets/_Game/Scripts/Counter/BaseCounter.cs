using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;
    [SerializeField] private Transform counterTopPoint;

    protected KitchenObject kitchenObject;

    public virtual void Interact(Player player)
    {
        Debug.Log("Base Counter.Interact()");
    }

    public virtual void InteractAlternate(Player player)
    {
        Debug.Log("Base Counter.InteractAlternate()");
    }

    public Transform CounterTopPointTransform()
    {
        return counterTopPoint;
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null    ) OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }


}
