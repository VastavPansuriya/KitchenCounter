using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private DeliveryManagerSingleUI recipeTemplete;


    private void Awake()
    {
        recipeTemplete.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        DeliveryManager.Instance.OnRecipeComplete += DeliveryManager_OnRecipeComplete;
        DeliveryManager.Instance.OnRecipeSpawn += DeliveryManager_OnRecipeSpawn;
    }
    private void Start()
    {
        UpdateVisuals();
    }
    private void DeliveryManager_OnRecipeSpawn(object sender, System.EventArgs e)
    {
        UpdateVisuals();
    }

    private void DeliveryManager_OnRecipeComplete(object sender, System.EventArgs e)
    {
        UpdateVisuals();
    }

    private void OnDisable()
    {
        DeliveryManager.Instance.OnRecipeComplete -= DeliveryManager_OnRecipeComplete;
        DeliveryManager.Instance.OnRecipeSpawn -= DeliveryManager_OnRecipeSpawn;
    }
    private void UpdateVisuals()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplete.transform)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            DeliveryManagerSingleUI recipeUI = Instantiate(recipeTemplete, container);
            recipeUI.gameObject.SetActive(true);
            recipeUI.SetRecipeSOUI(recipeSO);
        }
    }
}
