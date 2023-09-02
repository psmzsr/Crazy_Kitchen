using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timeImage;
    private void Update()
    {
        timeImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
