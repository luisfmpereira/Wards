using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMainMenu : MonoBehaviour
{
    PlayerConfigurationManager playerConfigurationManager;

    [SerializeField]
    private Object mainMenuScene;
    [SerializeField]
    private Image backProgression;
    [SerializeField]
    private float holdTime = 2f;
    private float currentHold;


    private void Start()
    {
        playerConfigurationManager = PlayerConfigurationManager.Instance;
    }

    private void Update()
    {
        UpdateProgressFill();
        if (Input.GetButton("Cancel"))
            currentHold += Time.deltaTime;
        else
            currentHold = 0;

        if (currentHold >= holdTime)
            LoadMainMenu();
    }

    void UpdateProgressFill()
    {
        backProgression.fillAmount = currentHold / holdTime;
    }

    void LoadMainMenu()
    {
        playerConfigurationManager.ResetPlayers();
        SceneManager.LoadScene("MainMenu");
    }

}
