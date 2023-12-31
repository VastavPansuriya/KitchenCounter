using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnProgressChange += StoveCounter_OnProgressChange;
        Hide();
    }

    private void StoveCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e)
    {
        float burnShowProgressAmount = .5f;
        Debug.Log(e.progressNormalized);
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount && e.progressNormalized <= 1;
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void OnDestroy()
    {
        stoveCounter.OnProgressChange -= StoveCounter_OnProgressChange;
    }

    private void Show() { gameObject.SetActive(true); }
    private void Hide() { gameObject.SetActive(false); }
}
