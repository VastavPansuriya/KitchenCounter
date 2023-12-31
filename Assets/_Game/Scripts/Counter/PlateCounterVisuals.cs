using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisuals : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;

    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private GameObject plateVisualPrefab;


    private List<GameObject> plateVisualGameObjectList = new List<GameObject>();
    private void OnEnable()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject lastPlateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(lastPlateGameObject);
        Destroy(lastPlateGameObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
       GameObject plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.transform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);   
        plateVisualGameObjectList.Add(plateVisualTransform);
    }

    private void OnDisable()
    {
        platesCounter.OnPlateSpawned -= PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved -= PlatesCounter_OnPlateRemoved;
    }
}


