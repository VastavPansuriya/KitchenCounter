using UnityEngine;

public class SelectedCounterVisuals : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] selectedCoutnerVisualAll;
    private void OnEnable()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        foreach (GameObject selectedCoutnerVisual in selectedCoutnerVisualAll)
        {
            selectedCoutnerVisual.SetActive(e.baseCounter == baseCounter);
        }
    }

    private void OnDisable()
    {
        Player.Instance.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
    }
}
