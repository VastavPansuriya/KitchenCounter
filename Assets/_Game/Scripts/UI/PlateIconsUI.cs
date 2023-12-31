using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private PlateIconSingleUI iconTemplete;

    private void Awake()
    {
        iconTemplete.gameObject.SetActive(false); 
    }
    private void OnEnable()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, KitchenObjectSO e)
    {
        UpdateVisual();
    }

    private void OnDisable()
    {
        plateKitchenObject.OnIngredientAdded -= PlateKitchenObject_OnIngredientAdded;
    }

    private void UpdateVisual()
    {
        foreach (Transform item in transform)
        {
            if (item == iconTemplete.transform)
            {
                continue;
            }
            Destroy(item.gameObject);   
        }
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSoList())
        {
            PlateIconSingleUI icon = Instantiate(iconTemplete,transform);
            icon.gameObject.SetActive(true);
            icon.SetKitchenObjectSo(kitchenObjectSO);
        }
    }
}
