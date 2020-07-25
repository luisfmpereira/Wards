#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public class MainMenuManager : MonoBehaviour
{

    public GameObject logo;

    [Header("Main Menu buttons")]
    [SerializeField]
    private GameObject mainMenuButtons;
    [SerializeField]
    private Button firstMainMenuButton;
    private bool isShowingMainMenu;

    [Header("Gamemodes screen button")]
    [SerializeField]
    private Button firstGamemodesScreenButton;
    [SerializeField]
    private GameObject gamemodesScreenButtons;
    private bool isShowingGamemodesScreenButtons;

    [Header("Gamemode 1 buttons")]
    [SerializeField]
    private Button firstGamemode1Button;
    [SerializeField]
    private GameObject gamemode1Buttons;
    private bool isShowingGamemode1Buttons;

    [Header("Credits")]
    [SerializeField]
    private GameObject creditsObject;
    private bool isShowingCredits;

    [Header("How to Play")]
    [SerializeField]
    private GameObject howToPlayObject;
    private bool isShowingHowToPlay;

    [Header("Sound options")]
    [SerializeField]
    private TextMeshProUGUI soundButtonText;
    [SerializeField]
    private string soundEnabled = "Sounds On";
    [SerializeField]
    private string soundDisabled = "Sounds Off";
    private AudioManager AudioManager;

    void Start()
    {
        AudioManager = AudioManager.Instance;
        ChangeMuteSettings();
        firstMainMenuButton.Select();
        AudioManager.PlaySound("MenuTrack");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if ((isShowingCredits || isShowingHowToPlay || isShowingGamemodesScreenButtons))
                ShowMainMenu();
            if (isShowingGamemode1Buttons)
                ShowGamemodesScreen();
        }

    }


    public void ShowMainMenu()
    {
        if (isShowingCredits || isShowingHowToPlay || isShowingGamemodesScreenButtons)
        {
            //DEFINE PLAYERPREFS TO HELP WITH DUPLICATE SPAWNS
            PlayerPrefs.SetInt("FirstRound", 0);

            AudioManager.PlaySound("ClickBack");

            //enable and disable screens
            mainMenuButtons.SetActive(true);
            gamemodesScreenButtons.SetActive(false);
            gamemode1Buttons.SetActive(false);
            creditsObject.SetActive(false);
            howToPlayObject.SetActive(false);

            //set bools
            isShowingMainMenu = true;
            isShowingGamemodesScreenButtons = false;
            isShowingGamemode1Buttons = false;
            isShowingCredits = false;
            isShowingHowToPlay = false;

            firstMainMenuButton.Select();
            logo.SetActive(true);
        }
    }

    public void ShowGamemodesScreen()
    {
        AudioManager.PlaySound("ButtonClick");

        //enable and disable screens
        mainMenuButtons.SetActive(false);
        gamemodesScreenButtons.SetActive(true);
        gamemode1Buttons.SetActive(false);
        creditsObject.SetActive(false);
        howToPlayObject.SetActive(false);

        //set bools
        isShowingMainMenu = false;
        isShowingGamemodesScreenButtons = true;
        isShowingGamemode1Buttons = false;
        isShowingCredits = false;
        isShowingHowToPlay = false;

        firstGamemodesScreenButton.Select();
        logo.SetActive(true);
    }

    public void ShowGamemode1Screen()
    {
        AudioManager.PlaySound("ButtonClick");

        //enable and disable screens
        mainMenuButtons.SetActive(false);
        gamemodesScreenButtons.SetActive(false);
        gamemode1Buttons.SetActive(true);
        creditsObject.SetActive(false);
        howToPlayObject.SetActive(false);

        //set bools
        isShowingMainMenu = false;
        isShowingGamemodesScreenButtons = false;
        isShowingGamemode1Buttons = true;
        isShowingCredits = false;
        isShowingHowToPlay = false;

        firstGamemode1Button.Select();
        logo.SetActive(true);
    }

    public void ShowCreditsScreen()
    {
        AudioManager.PlaySound("ButtonClick");

        //enable and disable screens
        mainMenuButtons.SetActive(false);
        gamemodesScreenButtons.SetActive(false);
        gamemode1Buttons.SetActive(false);
        creditsObject.SetActive(true);
        howToPlayObject.SetActive(false);

        //set bools
        isShowingMainMenu = false;
        isShowingGamemodesScreenButtons = false;
        isShowingGamemode1Buttons = false;
        isShowingCredits = true;
        isShowingHowToPlay = false;

        logo.SetActive(false);
    }


    public void ShowHowToPlayScreen()
    {
        AudioManager.PlaySound("ButtonClick");

        //enable and disable screens
        mainMenuButtons.SetActive(false);
        gamemodesScreenButtons.SetActive(false);
        gamemode1Buttons.SetActive(false);
        creditsObject.SetActive(false);
        howToPlayObject.SetActive(true);

        //set bools
        isShowingMainMenu = false;
        isShowingGamemodesScreenButtons = false;
        isShowingGamemode1Buttons = false;
        isShowingCredits = false;
        isShowingHowToPlay = true;

        logo.SetActive(false);
    }

    public void LoadScene(int NumberOfPlayers)
    {
        AudioManager.PlaySound("ButtonClick");

        if (NumberOfPlayers == 2)
            SceneManager.LoadScene("PlayerSetup2P");
        if (NumberOfPlayers == 3)
            SceneManager.LoadScene("PlayerSetup3P");
        if (NumberOfPlayers == 4)
            SceneManager.LoadScene("PlayerSetup4P");
    }

    public void ChangeSoundConfig()
    {
        if (PlayerPrefs.GetInt("Sounds") == 0)
            PlayerPrefs.SetInt("Sounds", 1);
        else if (PlayerPrefs.GetInt("Sounds") == 1)
            PlayerPrefs.SetInt("Sounds", 0);

        ChangeMuteSettings();
    }

    public void ChangeMuteSettings()
    {
        var allSounds = AudioManager.GetComponentsInChildren<AudioSource>();
        if (PlayerPrefs.GetInt("Sounds") == 0)
        {
            for (int i = 0; i < allSounds.Length; i++)
            {
                allSounds[i].mute = true;
            }
            soundButtonText.text = soundDisabled;
        }

        if (PlayerPrefs.GetInt("Sounds") == 1)
        {

            for (int i = 0; i < allSounds.Length; i++)
            {
                allSounds[i].mute = false;
            }
            soundButtonText.text = soundEnabled;

        }

    }














    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
    }

}
