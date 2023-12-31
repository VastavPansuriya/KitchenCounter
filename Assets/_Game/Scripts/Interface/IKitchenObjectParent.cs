using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent 
{
    public Transform CounterTopPointTransform();

    public KitchenObject GetKitchenObject();

    public void SetKitchenObject(KitchenObject kitchenObject);

    public void ClearKitchenObject();

    public bool HasKitchenObject();
}
