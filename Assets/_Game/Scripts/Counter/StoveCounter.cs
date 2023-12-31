using System;
using UnityEngine;

public class StoveCounter : BaseCounter,IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSo;

    private float burningTimer;
    private BurningRecipeSO burningRecipeSo;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChange;

    public class OnStateChangedEventArgs : EventArgs
    {
        public CokkingPattyState currentState;
    }

    public enum CokkingPattyState
    {
        Idel,
        Frying,
        Fried,
        Burned
    }

    private CokkingPattyState currentState;

    private void Start()
    {
        currentState = CokkingPattyState.Idel;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (currentState)
            {
                case CokkingPattyState.Idel:
                    break;
                case CokkingPattyState.Frying:
                    FryingState();
                    break;
                case CokkingPattyState.Fried:
                    FriedSate();
                    break;
                case CokkingPattyState.Burned:
                    break;
                default:
                    break;
            }
        }

    }

    private void FryingState()
    {
        fryingTimer += Time.deltaTime;
        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
        {
            progressNormalized = fryingTimer / fryingRecipeSo.fryingTimerMax
        });

        if (fryingTimer > fryingRecipeSo.fryingTimerMax)
        {
            GetKitchenObject().DestroySelf();
            kitchenObject = null;
            KitchenObject.SpawnKitchenObject(fryingRecipeSo.output, this);


            burningTimer = 0;
            currentState = CokkingPattyState.Fried;
            burningRecipeSo = GetBurningRecipeSoWithOutput(GetKitchenObject().GetKitchenObjectSo());
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                currentState = currentState
            });
        }
    }

    private void FriedSate()
    {
        burningTimer += Time.deltaTime;
        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
        {
            progressNormalized = burningTimer / burningRecipeSo.burningTimerMax
        });
        if (burningTimer > burningRecipeSo.burningTimerMax)
        {
            GetKitchenObject().DestroySelf();
            kitchenObject = null;
            KitchenObject.SpawnKitchenObject(burningRecipeSo.output, this);

            currentState = CokkingPattyState.Burned;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                currentState = currentState
            });
        }
    }

    public override void Interact(Player player)
    {
        //cutting Counter don't Have kitchen Object
        if (!HasKitchenObject())
        {
            //Player Have kitchen Object and it's have object which can fry by this counter
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                fryingRecipeSo = GetFringRecipeSoWithOutput(GetKitchenObject().GetKitchenObjectSo());
                fryingTimer = 0;
                currentState = CokkingPattyState.Frying;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    currentState = currentState
                });

                OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                {
                    progressNormalized = fryingTimer / fryingRecipeSo.fryingTimerMax
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
                currentState = CokkingPattyState.Idel; 
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    currentState = currentState
                });
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
                        GetKitchenObject().DestroySelf();

                        currentState = CokkingPattyState.Idel;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            currentState = currentState
                        });

                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
                        {
                            progressNormalized = 0f
                        });
                       
                    }
                }
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSo = GetFringRecipeSoWithOutput(inputKitchenObjectSO);
        return fryingRecipeSo != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSo = GetFringRecipeSoWithOutput(inputKitchenObjectSO);
        if (fryingRecipeSo != null)
        {
            return fryingRecipeSo.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFringRecipeSoWithOutput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO item in fryingRecipeSOArray)
        {
            if (item.input == inputKitchenObjectSO)
            {
                return item;
            }
        }
        return null;
    }


    private BurningRecipeSO GetBurningRecipeSoWithOutput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO item in burningRecipeSOArray)
        {
            if (item.input == inputKitchenObjectSO)
            {
                return item;
            }
        }
        return null;
    }
    public bool IsFried()
    {
        return currentState == CokkingPattyState.Fried;
    }
}
