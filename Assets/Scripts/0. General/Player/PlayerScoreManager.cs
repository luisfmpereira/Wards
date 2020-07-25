using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    private AudioManager AudioManager;
    [SerializeField]
    private int currentScore = 0;
    private List<PlayerConfiguration> playerConfigs;
    private PlayerController playerController;
    private CanvasScoreManager canvasScoreManager;

    [Header("Hazards interaction")]
    [SerializeField]
    private int spikeScorePenalty;
    [SerializeField]
    private float invincibilityTime;
    private float currentInvincibilityTime;
    private bool isHit;



    void Start()
    {
        AudioManager = AudioManager.Instance;
        canvasScoreManager = GameObject.Find("LevelCanvas").GetComponent<CanvasScoreManager>();
        playerController = this.GetComponent<PlayerController>();
        playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs();
        currentScore = 0;
        ResetScores();
        SetImages();

    }

    private void Update()
    {
        if (isHit)
        {
            currentInvincibilityTime -= Time.deltaTime;
            if (currentInvincibilityTime <= 0)
                isHit = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {

            Scored(other.GetComponent<CollectibleDespawn>().scoreToAdd);
            other.GetComponent<CollectibleDespawn>().PlayDespawnAnim();
            AudioManager.PlaySound("CollectibleCollected");

            if (other.GetComponent<CollectibleDespawn>().isRotten)
                AudioManager.PlaySound("CollectibleRotten");

        }

        if (other.CompareTag("Spike"))
        {
            playerController.Ricochet(other.transform);

            if (!isHit)
            {
                AudioManager.PlaySound("SpikeHit");
                isHit = true;
                currentInvincibilityTime = invincibilityTime;
                if (playerConfigs[playerController.playerIndex].PlayerScore > 0)
                    Scored(spikeScorePenalty);
            }

        }

    }

    void Scored(int points)
    {
        currentScore += points;

        if (currentScore <= 0)
            currentScore = 0;

        playerConfigs[playerController.playerIndex].PlayerScore = currentScore;
        canvasScoreManager.UpdateScore(playerController.playerIndex);
    }


    private void SetImages()
    {
        var length = playerConfigs.Count(p => p.IsReady == true);
        for (int i = 0; i < length; i++)
        {
            canvasScoreManager.SetCharacterImage(i);
        }
    }

    private void ResetScores()
    {
        var length = playerConfigs.Count(p => p.IsReady == true);
        for (int i = 0; i < length; i++)
        {
            playerConfigs[i].PlayerScore = 0;
            canvasScoreManager.UpdateScore(i);
        }
    }

}
