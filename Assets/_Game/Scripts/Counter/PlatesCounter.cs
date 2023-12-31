using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSo;

    private float spawnPlateTimer;
    private const float spawnPlatTimerMax = 4f;
    private int plateSpawnAmount;
    private const int plateSpawnAmountMax = 4;

    private void Start()
    {
        spawnPlateTimer = 0;
        plateSpawnAmount = 0;
    }

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlatTimerMax)
        {
            spawnPlateTimer = 0;
            if (KitchenGameManager.Instance.IsGamePlaying() && plateSpawnAmount < plateSpawnAmountMax)
            {
                plateSpawnAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (plateSpawnAmount > 0)
            {
                plateSpawnAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSo, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
