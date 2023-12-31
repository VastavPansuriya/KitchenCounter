using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSOGameObject
    {
        public GameObject kitchenObject;
        public KitchenObjectSO kitchenObjectSo;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;

    [SerializeField] private List<KitchenObjectSOGameObject> kitchenObjectSOGameObjectList;

    private void OnEnable()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }
    private void Start()
    {
        foreach (KitchenObjectSOGameObject item in kitchenObjectSOGameObjectList)
        {
            item.kitchenObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, KitchenObjectSO kitchenObject)
    {
        foreach (KitchenObjectSOGameObject item in kitchenObjectSOGameObjectList)
        {
            if (item.kitchenObjectSo == kitchenObject)
            {
                item.kitchenObject.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        plateKitchenObject.OnIngredientAdded -= PlateKitchenObject_OnIngredientAdded;
    }

}
