using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text recipeDeliveredText;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.MainMenuScene); }); 
        KitchenGameManager.Instance.OnGameStateChanged += KitchenGameManager_OnGameStateChanged;
        Hide();
    }
    private void KitchenGameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();
            recipeDeliveredText.text = DeliveryManager.Instance.GetSuccessRecipeCount().ToString();
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
