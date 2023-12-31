using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRecipeSpawn;
    public event EventHandler OnRecipeComplete;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private const float spawnRecipeTimerMax = 4f;

    private const int waitingRecipesMax = 4;

    private int successfulRecipeCount = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer < 0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO);
                waitingRecipeSOList.Add(waitingRecipeSO);
                OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSo = waitingRecipeSOList[i];
            if (waitingRecipeSo.kitchenObjectSoList.Count == plateKitchenObject.GetKitchenObjectSoList().Count)
            {
                //has the same ingredients
                bool plateContantsMachesRecipe = true;
                foreach (KitchenObjectSO kitchenObjectSO in waitingRecipeSo.kitchenObjectSoList)
                {
                    bool isIngredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSoList())
                    {
                        if (plateKitchenObjectSO == kitchenObjectSO)
                        {
                            isIngredientFound = true;
                            break;
                        }
                    }
                    if (!isIngredientFound)
                    {
                        // recipe not complete
                        plateContantsMachesRecipe = false;
                    }
                }

                if (plateContantsMachesRecipe)
                {
                    //CorrectRecipe
                    successfulRecipeCount++;
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeComplete?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }


    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessRecipeCount()
    {
        return successfulRecipeCount;
    }
}
