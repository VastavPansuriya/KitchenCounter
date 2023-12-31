using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisuals : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    [SerializeField] private GameObject[] effectVisuals;

    private void OnEnable()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool isShow = e.currentState == StoveCounter.CokkingPattyState.Fried || e.currentState == StoveCounter.CokkingPattyState.Frying;
        HideShowToggle(isShow);
    }

    private void OnDisable()
    {
        stoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
    }

    private void HideShowToggle(bool isShow)
    {
        foreach (GameObject item in effectVisuals)
        {
            item.gameObject.SetActive(isShow);
        }
    }
}
