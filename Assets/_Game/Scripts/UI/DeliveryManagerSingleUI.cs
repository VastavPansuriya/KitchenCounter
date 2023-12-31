using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text recipeName;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Image iconTemplete;


    private void Awake()
    {
        iconTemplete.gameObject.SetActive(false);
    }


    public void SetRecipeSOUI(RecipeSO recipeSO)
    {
        recipeName.text = recipeSO.recipeName;

        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplete.transform)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSo in recipeSO.kitchenObjectSoList)
        {
            Image iconUI = Instantiate(iconTemplete, iconContainer);
            iconUI.gameObject.SetActive(true);
            iconUI.sprite = kitchenObjectSo.sprite;
        }
    }
}
