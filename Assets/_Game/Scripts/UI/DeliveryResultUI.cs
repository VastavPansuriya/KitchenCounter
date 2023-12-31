using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "Popup";
    [SerializeField] private Image bgImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failedSprite;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        bgImage.color = failedColor;
        iconImage.sprite = failedSprite;
        messageText.text = "DELIVERY\nFAILED";
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        bgImage.color = successColor;
        iconImage.sprite = successSprite;
        messageText.text = "DELIVERY\nSUCCESS";

    }
    private void OnDestroy()
    {
        DeliveryManager.Instance.OnRecipeSuccess -= DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed -= DeliveryManager_OnRecipeFailed;
    }
}
