using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;
    private void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        Hide();
    }

    private void Awake()
    {
        resumeBtn.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePasueGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            Hide();
            OptionsUI.Instance.Show(show);
        });
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        show();
    }

    private void show()
    {
        gameObject.SetActive(true);
        resumeBtn.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
