using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{

    PlayerConfigurationManager playerConfigurationManager;
    InitializeLevel initializeLevel;
    CollectibleSpawnManager spawnManager;
    private AudioManager AudioManager;

    [Header("Funcitonality variables")]
    [SerializeField]
    private float maxPlayTime;
    private float currentPlayTime;
    public bool isStarted;
    public int levelRepetitions;

    [Header("Main timer references")]
    private GameObject mainTimerObj;
    private TextMeshProUGUI mainTimerText;
    public bool timeEndedFlag;

    [Header("Score Bar references")]
    private GameObject scoresObject;

    [Header("Victory Canvas references")]
    [SerializeField]
    private GameObject victoryCanvas;

    [Header("Winner variables")]
    public int winnerIndex;

    void Start()
    {
        //reset level
        //reset scores

        playerConfigurationManager = PlayerConfigurationManager.Instance;
        currentPlayTime = maxPlayTime;
        PlayerPrefs.SetInt("FirstRound", 1);

        //spawn manager
        spawnManager = GameObject.Find("CollectibleSpawnManager").GetComponent<CollectibleSpawnManager>();

        //Canvas objects
        mainTimerObj = GameObject.Find("MainTimer");
        mainTimerText = mainTimerObj.GetComponent<TextMeshProUGUI>();
        mainTimerObj.SetActive(false);

        scoresObject = GameObject.Find("ScoresObject");
        scoresObject.SetActive(false);

        //Victory screen 
        victoryCanvas.SetActive(false);

        AudioManager = AudioManager.Instance;
        AudioManager.PlaySound("LevelTrack");
        AudioManager.StopSound("MenuTrack");
    }

    void Update()
    {
        if (isStarted)
            CountdownTimer();

        if (currentPlayTime <= 0 && !timeEndedFlag)
            TimeEnded();
    }


    void CountdownTimer()
    {
        currentPlayTime -= Time.deltaTime;
        var ts = TimeSpan.FromSeconds(currentPlayTime);
        mainTimerText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

    }

    public void EnableMainTimer()
    {
        isStarted = true;
        mainTimerObj.SetActive(true);
        scoresObject.SetActive(true);
    }


    void TimeEnded()
    {
        isStarted = false;
        AudioManager.PlaySound("EndOfLevel");
        mainTimerObj.SetActive(false);
        scoresObject.SetActive(false);

        //get round winner
        DefineWinner();

        victoryCanvas.SetActive(true);
        timeEndedFlag = true;
    }

    public void ReturnToMainMenu()
    {
        playerConfigurationManager.ResetPlayers();
        AudioManager.StopSound("LevelTrack");
        SceneManager.LoadScene("MainMenu");
    }



    public void DefineWinner()
    {
        winnerIndex = playerConfigurationManager.DefineWinner();
        if (winnerIndex >= 0)
            playerConfigurationManager.AddRoundVictory(winnerIndex);
    }






}
