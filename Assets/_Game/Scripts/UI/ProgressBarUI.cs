using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour  
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image cuttingProgressSlider;
    private IHasProgress hasProgress;

    private void Awake()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError($"Your assigned gameobject {hasProgress} does not contain IhasProgress");
        }
    }
    private void OnEnable()
    {
        hasProgress.OnProgressChange += IHasProgress_OnProgressChange;
    }

    private void OnDestroy()
    {
        hasProgress.OnProgressChange -= IHasProgress_OnProgressChange;
    }
    private void Start()
    {
        cuttingProgressSlider.fillAmount = 0;
        HideSlider();
    }

    private void IHasProgress_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e)
    {
        cuttingProgressSlider.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0 || e.progressNormalized >= 1)
        {
            HideSlider();
        }
        else
        {
            ShowSlider();
        }
    }
    private void ShowSlider()
    {
        gameObject.SetActive(true);
    }

    private void HideSlider()
    {
        gameObject.SetActive(false);
    }
}
