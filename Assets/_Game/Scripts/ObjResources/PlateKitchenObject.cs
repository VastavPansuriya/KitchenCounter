using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{

    public static event EventHandler OnPlateUsed;
    public event EventHandler<KitchenObjectSO> OnIngredientAdded;
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSoList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngerdiant(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSoList.Contains(kitchenObjectSO))
        {
            return false;
        }

        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            OnPlateUsed?.Invoke(this, EventArgs.Empty);
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this,kitchenObjectSO);
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSoList()
    {
        return kitchenObjectSOList;
    }
}
