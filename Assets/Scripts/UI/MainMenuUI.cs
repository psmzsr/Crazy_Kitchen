using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;


    private void Awake()
    {
        playBtn.onClick.AddListener(()=>
        {
            //SceneManager.LoadScene(1);
            Loader.load(Loader.Scene.GameScene);
        });
        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Time.timeScale = 1f;
    }


}
