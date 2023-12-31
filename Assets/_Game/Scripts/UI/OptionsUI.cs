using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;

    [Space()]
    [SerializeField] private TMP_Text soundEffectText;
    [SerializeField] private TMP_Text musicText;

    [Space(30)]
    [Header("BindingButton")]
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAltButton;
    [SerializeField] private Button gamepadPauseButton;

    [Space(30)]
    [Header("BindingText")]
    [SerializeField] private TMP_Text moveUPText;
    [SerializeField] private TMP_Text moveDownText;
    [SerializeField] private TMP_Text moveLeftText;
    [SerializeField] private TMP_Text moveRightText;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private TMP_Text interactAltText;
    [SerializeField] private TMP_Text pauseText;
    [SerializeField] private TMP_Text gamepadInteractText;
    [SerializeField] private TMP_Text gamepadInteractAltText; 
    [SerializeField] private TMP_Text gamepadPauseText;

    [Space()]
    [SerializeField] private GameObject pressToRebindKeyTransform;


    private Action onCloseButtonAction;
    private void Awake()
    {
        Instance = this;
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction?.Invoke();
        });

        moveUpButton.onClick.AddListener(() => { RebingBinding(GameInputManager.Binding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebingBinding(GameInputManager.Binding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebingBinding(GameInputManager.Binding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebingBinding(GameInputManager.Binding.Move_Right); });
        interactButton.onClick.AddListener(() => { RebingBinding(GameInputManager.Binding.Interact); });
        interactAltButton.onClick.AddListener(() => { RebingBinding(GameInputManager.Binding.InteractAlt); });
        gamepadInteractButton.onClick.AddListener(() => { RebingBinding(GameInputManager.Binding.GamePadInteract); });
        gamepadInteractAltButton.onClick.AddListener(() => { RebingBinding(GameInputManager.Binding.GamePadInteract); });
        gamepadPauseButton.onClick.AddListener(() => { RebingBinding(GameInputManager.Binding.GamePadPause); });
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePauseToggle += KitchenGameManager_OnGamePauseToggle;
        PressToRebindKeyHide();
        UpdateVisual();
        Hide();
    }

    private void KitchenGameManager_OnGamePauseToggle(object sender, bool isPaused)
    {
        if (!isPaused)
        {
            Hide();
        }
    }

    private void OnDestroy()
    {
        KitchenGameManager.Instance.OnGamePauseToggle -= KitchenGameManager_OnGamePauseToggle;
    }
    private void UpdateVisual()
    {
        soundEffectText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f).ToString();
        musicText.text = "Music : " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f).ToString();

        moveUPText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Up);
        moveDownText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Down);
        moveLeftText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Left);
        moveRightText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_Right);
        interactText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Interact);
        interactAltText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.InteractAlt);
        pauseText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Pause);
        gamepadInteractText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.GamePadInteract);
        gamepadInteractAltText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.GamePadInteractAlt);
        gamepadPauseText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.GamePadPause);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
        soundEffectsButton.Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void PressToRebindKeyShow()
    {
        pressToRebindKeyTransform.SetActive(true);
    }

    public void PressToRebindKeyHide()
    {
        pressToRebindKeyTransform.SetActive(false);
    }

    private void RebingBinding(GameInputManager.Binding binding)
    {
        PressToRebindKeyShow();
        GameInputManager.Instance.RebindBinding(binding,() => {
            UpdateVisual();
            PressToRebindKeyHide();
            });
    }
}
