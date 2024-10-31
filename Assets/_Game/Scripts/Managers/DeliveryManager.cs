using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{

    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRecipeSpawn;
    public event EventHandler OnRecipeComplete;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer = 4f;

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
        if (!IsServer)
        {
            return;
        }

        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer < 0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
            {
                int waitingRecipeSOIndex = UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count);

                SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex);
            }
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[waitingRecipeSOIndex];
        waitingRecipeSOList.Add(waitingRecipeSO);
        OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
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
                    DeliverCorrectRecipeServerRpc(i);
                    return;
                }
            }
        }
        DeliverIncorrectRecipeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverIncorrectRecipeServerRpc()
    {
        DeliverIncorrectRecipeClientRpc();
    }

    [ClientRpc]
    private void DeliverIncorrectRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int correctRecipe)
    {
        DeliverCorrectRecipeClientRpc(correctRecipe);
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int correctRecipe)
    {
        successfulRecipeCount++;
        waitingRecipeSOList.RemoveAt(correctRecipe);
        OnRecipeComplete?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
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
