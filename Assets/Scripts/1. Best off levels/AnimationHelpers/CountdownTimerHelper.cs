using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimerHelper : MonoBehaviour
{
    [SerializeField]
    float countdownTimerMax = 3;

    LevelManager levelManager;
    void Awake()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }


    public void DecreaseCountdownTimer()
    {
        countdownTimerMax--;
        this.GetComponent<TextMeshProUGUI>().text = countdownTimerMax.ToString();
    }
    public void EnableMainTimer() => levelManager.EnableMainTimer();

    public void DisableGameObject() => this.gameObject.SetActive(false);

}
