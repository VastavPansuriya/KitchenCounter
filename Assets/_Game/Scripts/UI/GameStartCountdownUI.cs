using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string MUNER_POPUPTRIGGER = "NumberPopup";
    [SerializeField] private TMP_Text countDownText;
    private Animator animator;

    private int previouseCountdownNum = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameStateChanged += KitchenGameManager_OnGameStateChanged;
        Hide();
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        countDownText.text = countdownNumber.ToString();
        if (previouseCountdownNum != countdownNumber)
        {
            previouseCountdownNum = countdownNumber;
            animator.SetTrigger(MUNER_POPUPTRIGGER);
            SoundManager.Instance.PlayCountDownSound();
        }
    }

    private void KitchenGameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        KitchenGameManager.Instance.OnGameStateChanged -= KitchenGameManager_OnGameStateChanged;
    }
}
