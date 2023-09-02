using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    [SerializeField] private Button soundEffectBtn;
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button moveUpBtn;
    [SerializeField] private Button moveDownBtn;
    [SerializeField] private Button moveLeftBtn;
    [SerializeField] private Button moveRightBtn;
    [SerializeField] private Button interactBtn;
    [SerializeField] private Button interactAlternateBtn;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button gamepadInteractBtn;
    [SerializeField] private Button gamepadInteractAlternateBtn;
    [SerializeField] private Button gamepadPauseBtn;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;
    [SerializeField] private Transform pressToRevindKeyTransForm;

    private Action onCloseButtonAction;

    private void Awake()
    {
        Instance = this;
        soundEffectBtn.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicBtn.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeBtn.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });

        moveUpBtn.onClick.AddListener(() => {RebindBinding(GameInput.Binding.Move_Up); });
        moveDownBtn.onClick.AddListener(() => {RebindBinding(GameInput.Binding.Move_Down); });
        moveLeftBtn.onClick.AddListener(() => {RebindBinding(GameInput.Binding.Move_Left); });
        moveRightBtn.onClick.AddListener(() => {RebindBinding(GameInput.Binding.Move_Right); });
        interactBtn.onClick.AddListener(() => {RebindBinding(GameInput.Binding.Interact); });
        interactAlternateBtn.onClick.AddListener(() => {RebindBinding(GameInput.Binding.InteractAlternate); });
        pauseBtn.onClick.AddListener(() => {RebindBinding(GameInput.Binding.Pause); });
        gamepadInteractBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact); });
        gamepadInteractAlternateBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_InteractAlternate); });
        gamepadPauseBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Pause); });
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        UpdateVisual();
        Hide();
        HidePressTorebindKey();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects" + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music" + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        gamepadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

    }
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);
        soundEffectBtn.Select();
    }

    private void ShowPressTorebindKey()
    {
        pressToRevindKeyTransForm.gameObject.SetActive(true);
    }
    private void HidePressTorebindKey()
    {
        pressToRevindKeyTransForm.gameObject.SetActive(false);
    }
    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressTorebindKey();
        GameInput.Instance.RebindBinding(binding,()=> { HidePressTorebindKey();UpdateVisual(); });
    }
}
