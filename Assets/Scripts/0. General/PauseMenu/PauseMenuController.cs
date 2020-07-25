#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    private bool isPaused;
    private float originalFixedTime;

    [SerializeField]
    private GameObject pauseCanvas;
    [SerializeField]
    private Button firstButtonOnList;
    private LevelManager levelManager;
    private AudioManager AudioManager;


    public void Awake()
    {
        AudioManager = AudioManager.Instance;
    }

    public void Start()
    {
        isPaused = false;
        originalFixedTime = Time.fixedDeltaTime;
        pauseCanvas.SetActive(false);
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && levelManager.isStarted)
        {
            isPaused = !isPaused;

            if (isPaused)
                PauseGame();

            else
            {
                ResumeGame();
                isPaused = !isPaused;
            }
        }
    }

    public void PauseGame()
    {
        AudioManager.PlaySound("ButtonClick");
        AudioManager.SetVolume("LevelTrack", 0);
        pauseCanvas.SetActive(true);
        firstButtonOnList.Select();
        StopSimulation();

    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        isPaused = !isPaused;
        ResumeSimulation();
        AudioManager.PlaySound("ButtonClick");
        AudioManager.OriginalVolume("LevelTrack");
    }


    public void ReturnToMainMenu()
    {
        ResumeSimulation();
        AudioManager.PlaySound("ButtonClick");
        levelManager.ReturnToMainMenu();
    }

    void StopSimulation()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
    }

    void ResumeSimulation()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = originalFixedTime;
    }


}
