using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cutKitchenObjectSOArray;

    public static event EventHandler OnAnyCut;

    public event EventHandler OnCut;

    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChange;


    private int cuttingProgess;
    public override void Interact(Player player)
    {
        //cutting Counter don't Have kitchen Object
        if (!HasKitchenObject())
        {
            //Player Have kitchen Object and it's have object which can cut by this counter
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgess = 0;
                CuttingRecipeSO cuttingRecipeSO = GetRecipeSoWithOutput(GetKitchenObject().GetKitchenObjectSo());
                OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                {
                    progressNormalized = (float)cuttingProgess / (float)cuttingRecipeSO.cuttingProgressMax
                });
            }
            // this object can not cut
            else
            {

            }
        }
        //Clear Counter Have kitchen Object
        else
        {
            //switch parent to cutting counter to player
            //Player don't Have kitchen Object
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                {
                    progressNormalized = 0f
                });
            }
            else
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngerdiant(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        Debug.Log("Event Working!");
                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                        {
                            progressNormalized = 0f
                        });
                        GetKitchenObject().DestroySelf();

                    }
                }
            }
        }
    }
    public override void InteractAlternate(Player player)
    {

        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo()))
        {
            cuttingProgess++;

            KitchenObjectSO inputKitchenObjectSO = GetKitchenObject().GetKitchenObjectSo();

            CuttingRecipeSO cuttingRecipeSO = GetRecipeSoWithOutput(inputKitchenObjectSO);

            OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
            {
                progressNormalized = (float)cuttingProgess / cuttingRecipeSO.cuttingProgressMax
            });

            OnCut?.Invoke(this, EventArgs.Empty);
            Debug.Log(OnCut.GetInvocationList().Length);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            if (cuttingProgess >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObject = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());

                GetKitchenObject().DestroySelf();

                kitchenObject = null;

                KitchenObject.SpawnKitchenObject(outputKitchenObject, this);
            }

        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetRecipeSoWithOutput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetRecipeSoWithOutput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetRecipeSoWithOutput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO item in cutKitchenObjectSOArray)
        {
            if (item.input == inputKitchenObjectSO)
            {
                return item;
            }
        }
        return null;
    }
}
