using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour
{
    private const string PLAYERPREF_BINDINGS = "InputBindings";

    public static GameInputManager Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPause;
    public event EventHandler OnBindingRebind;


    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlt,
        Pause,
        GamePadInteract,
        GamePadInteractAlt,
        GamePadPause,
        
    }
    private PlayerInputAction playerInputActions;

    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey(PLAYERPREF_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYERPREF_BINDINGS));
        }
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();


    }

    private void OnEnable()
    {
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }
    public Vector2 GetMovementInputNormalize()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlt:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.GamePadInteract:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.GamePadInteractAlt:
                return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.GamePadPause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
        }

    }

    public void RebindBinding(Binding binding, Action OnActionRebound)
    {
        playerInputActions.Disable();

        InputAction inputAction;
        int bindingIndex = 0;
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlt:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.GamePadInteract:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamePadInteractAlt:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.GamePadPause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).
            OnComplete(callback =>
            {
                callback.Dispose();
                playerInputActions.Enable();
                OnActionRebound?.Invoke();
                playerInputActions.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString(PLAYERPREF_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            }).Start();
    }
}
