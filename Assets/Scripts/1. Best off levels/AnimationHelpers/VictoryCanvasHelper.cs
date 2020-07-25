using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class VictoryCanvasHelper : MonoBehaviour
{
    PlayerConfigurationManager playerConfigurationManager;
    private List<PlayerConfiguration> playerConfigs;
    private AudioManager AudioManager;

    private bool animationFinished;
    private Animator animator;

    [Header("Level information")]
    private int levelRepetitions;
    int numberOfPlayers;
    LevelManager levelManager;

    [Header("Canvases")]
    [SerializeField]
    private GameObject roundEndedCanvas;
    [SerializeField]
    private GameObject matchEndedCanvas;
    [SerializeField]
    private GameObject pressToContinueText;

    [Header("Round info")]
    [SerializeField]
    private int roundWinnerIndex = -1;
    private bool isDraw;
    [SerializeField]
    private TextMeshProUGUI roundWinnerText;


    [Header("Match info")]
    [SerializeField]
    private TextMeshProUGUI matchWinnerText;
    public bool matchHasWinner = false;
    private int matchWinnerIndex;


    [Header("Round victory information")]
    [SerializeField]
    private GameObject scoreMaster;
    [SerializeField]
    private GameObject playerReferenceObject;
    [SerializeField]
    private GameObject victoryStarObject;
    [SerializeField]
    private List<GameObject> playerReferenceList;
    [SerializeField]
    private List<GameObject> player1Stars;
    [SerializeField]
    private List<GameObject> player2Stars;
    [SerializeField]
    private List<GameObject> player3Stars;
    [SerializeField]
    private List<GameObject> player4Stars;




    private void Awake()
    {
        AudioManager = AudioManager.Instance;
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        animator = GetComponent<Animator>();
        playerConfigurationManager = PlayerConfigurationManager.Instance;
        playerConfigs = playerConfigurationManager.GetPlayerConfigs();

        levelRepetitions = levelManager.levelRepetitions;
        numberOfPlayers = playerConfigurationManager.CountActivePlayers();

        animationFinished = false;


        InstantiatePlayersScoreReference();
        InstantiateVictoryStars();
        roundEndedCanvas.SetActive(true);
    }

    void SetCanvasRoundWinner()
    {
        roundWinnerIndex = levelManager.winnerIndex;
        if (roundWinnerIndex < 0)
            roundWinnerText.text = "Draw! No one gets points!";
        else
            roundWinnerText.text = "Player " + (roundWinnerIndex + 1).ToString() + " won this round!";
    }


    void EnableRoundVictoryStars()
    {
        for (int i = 0; i < playerConfigs.Count(x => x.IsReady == true); i++)
        {
            for (int j = 0; j < levelRepetitions; j++)
            {
                if (playerConfigs[i].RoundVictories >= j + 1)
                    switch (i)
                    {
                        case 0:
                            player1Stars[j].transform.GetChild(0).gameObject.SetActive(true);
                            break;

                        case 1:
                            player2Stars[j].transform.GetChild(0).gameObject.SetActive(true);
                            break;

                        case 2:
                            player3Stars[j].transform.GetChild(0).gameObject.SetActive(true);
                            break;

                        case 3:
                            player4Stars[j].transform.GetChild(0).gameObject.SetActive(true);
                            break;

                        default:
                            break;
                    }
            }
        }

    }

    public void CheckForMatchWinners()
    {
        for (int i = 0; i < playerConfigs.Count(x => x.IsReady == true); i++)
        {
            if (playerConfigs[i].RoundVictories >= levelRepetitions)
            {
                animator.StopPlayback();
                animator.Play("MatchVictory");
                matchWinnerIndex = i;
                matchHasWinner = true;

            }
        }
    }

    public void SetMatchWinnerText()
    {
        matchWinnerText.text = "Player " + (matchWinnerIndex + 1).ToString() + " won the match!";
    }

    public void ResetVictoryCanvas()
    {
        roundEndedCanvas.SetActive(false);
        matchEndedCanvas.SetActive(false);
    }



    //instantiate player victory game objects
    public void InstantiatePlayersScoreReference()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            var x = Instantiate(playerReferenceObject, scoreMaster.transform.position, Quaternion.identity, scoreMaster.transform);
            x.GetComponent<TextMeshProUGUI>().text = "P" + (i + 1).ToString();
            x.transform.GetChild(1).GetComponent<Image>().sprite = playerConfigs[i].PlayerCharacterSprite;
            playerReferenceList.Add(x);
        }
    }


    void InstantiateVictoryStars()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            for (int j = 0; j < levelRepetitions; j++)
            {
                var x = Instantiate(victoryStarObject, playerReferenceList[i].transform.position, Quaternion.identity, playerReferenceList[i].transform.GetChild(0).transform);
                switch (i)
                {
                    case 0:
                        player1Stars.Add(x);
                        break;

                    case 1:
                        player2Stars.Add(x);
                        break;

                    case 2:
                        player3Stars.Add(x);
                        break;

                    case 3:
                        player4Stars.Add(x);
                        break;

                    default:
                        break;

                }
            }
        }
    }






    private void Update()
    {
        if (animationFinished && Input.GetButtonDown("Submit"))
            if (!matchHasWinner)
                ReloadLevel();
            else
                ReturnToMainMenu();
    }

    public void EnableReturnClick()
    {
        animationFinished = true;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        levelManager.ReturnToMainMenu();
    }




}
