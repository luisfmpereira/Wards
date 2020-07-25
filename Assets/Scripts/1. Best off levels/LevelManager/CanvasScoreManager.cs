using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasScoreManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI[] playersScores;
    [SerializeField]
    private Image[] playerSprites;
    [SerializeField]
    private GameObject[] playerLeaderImages;
    private PlayerController playerController;
    private PlayerConfiguration[] playerConfigs;
    private int currentHighscore = 0;

    private void Awake()
    {
        playerController = this.GetComponent<PlayerController>();
        playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
    }

    public void UpdateScore(int playerIndex)
    {
        SetLeaderImage();
        playersScores[playerIndex].text = playerConfigs[playerIndex].PlayerScore.ToString();
    }

    public void SetCharacterImage(int playerIndex)
    {
        playerSprites[playerIndex].sprite = playerConfigs[playerIndex].PlayerCharacterSprite;
    }


    public void SetLeaderImage()
    {
        currentHighscore = 0;
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            if (playerConfigs[i].PlayerScore > currentHighscore)
                currentHighscore = playerConfigs[i].PlayerScore;

        }

        for (int i = 0; i < playerConfigs.Length; i++)
        {

            if (playerConfigs[i].PlayerScore < currentHighscore)
            {
                playerLeaderImages[i].SetActive(false);
            }

            if (playerConfigs[i].PlayerScore == currentHighscore)
            {
                playerLeaderImages[i].SetActive(true);
            }
        }

    }
}
