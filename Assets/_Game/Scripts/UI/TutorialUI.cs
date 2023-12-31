using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TMP_Text keyMoveUpText;
    [SerializeField] private TMP_Text keyMoveDownText;
    [SerializeField] private TMP_Text keyMoveLeftText;
    [SerializeField] private TMP_Text keyMoveRightText;
    [SerializeField] private TMP_Text keyMoveInteractText;
    [SerializeField] private TMP_Text keyMoveInteractAltText;
    [SerializeField] private TMP_Text keyMovePauseText;
    [SerializeField] private TMP_Text gamePadMoveInteractText;
    [SerializeField] private TMP_Text gamePadMoveInteractAltText;
    [SerializeField] private TMP_Text gamePadMovePauseText;

    private void Start()
    {
        GameInputManager.Instance.OnBindingRebind += GameInputManager_OnBindingRebind;
        KitchenGameManager.Instance.OnGameStateChanged += KitchenGameManager_OnGameStateChanged;
        UpdateVisuals();
        Show();
    }

    private void KitchenGameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            Hide();
        }
    }

    private void GameInputManager_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisuals();
    }
    private void OnDestroy()
    {
        GameInputManager.Instance.OnBindingRebind -= GameInputManager_OnBindingRebind;
    }

    private void UpdateVisuals()
    {
        keyMoveUpText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Up);
        keyMoveDownText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Down);
        keyMoveLeftText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Left);
        keyMoveRightText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Right);
        keyMoveInteractText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Interact);
        keyMoveInteractAltText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.InteractAlt);
        keyMovePauseText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Pause);
        gamePadMoveInteractText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.GamePadInteract);
        gamePadMoveInteractAltText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.GamePadInteractAlt);
        gamePadMovePauseText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.GamePadPause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
