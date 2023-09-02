using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    private const string PlAY_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInterAction;
    public event EventHandler OnInterAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnbindingRebind;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause,
    }

    private PlayerInputActions playInputActions;

    public void Awake()
    {
        Instance = this;

        playInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PlAY_PREFS_BINDINGS))
        {
            playInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PlAY_PREFS_BINDINGS));
        }

        playInputActions.Player.Enable();

        playInputActions.Player.Interact.performed += Interact_performed;
        playInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playInputActions.Player.Pause.performed += Pause_performed;
    }
    private void OnDestroy()
    {
        playInputActions.Player.Interact.performed -= Interact_performed;
        playInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playInputActions.Player.Pause.performed -= Pause_performed;

        playInputActions.Dispose();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInterAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //if (OnInterAction != null)
        //{
        //    OnInterAction(this, EventArgs.Empty);

        //}

        OnInterAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return playInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playInputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }
    public void RebindBinding(Binding binding,Action onActionRebound)
    {
        // 禁用玩家输入操作，确保在重新绑定期间不会接收输入
        playInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = playInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = playInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
        .OnComplete(callback =>
        {
            callback.Dispose();
            playInputActions.Player.Enable();
            onActionRebound();
            PlayerPrefs.SetString(PlAY_PREFS_BINDINGS, playInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            OnbindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();
    }
}
