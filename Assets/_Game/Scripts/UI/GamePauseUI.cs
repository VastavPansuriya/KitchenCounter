using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;
    private void Awake()
    {
        resumeButton.onClick.AddListener (() => { KitchenGameManager.Instance.TogglePauseGame(); });
        mainMenuButton.onClick.AddListener (() => { Loader.Load(Loader.Scene.MainMenuScene); });
        optionsButton.onClick.AddListener (() => { Hide(); OptionsUI.Instance.Show(Show); });
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePauseToggle += KitchenGameManager_OnGamePauseToggle;
        Hide();
    }

    private void KitchenGameManager_OnGamePauseToggle(object sender, bool isPause)
    {
        gameObject.SetActive(isPause);
    }

    private void OnDestroy()
    {
        KitchenGameManager.Instance.OnGamePauseToggle -= KitchenGameManager_OnGamePauseToggle;
    }
    public void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
