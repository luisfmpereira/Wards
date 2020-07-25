using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPowerUpManager : MonoBehaviour
{
    private PlayerController playerController;
    private AudioManager AudioManager;

    [Header("Magnet Variables")]
    [SerializeField]
    private float magnetDuration;
    [SerializeField]
    private float currentMagnetDuration;
    [SerializeField]
    private GameObject magnetRegion;
    private bool magnetActive;

    [Header("Higher Jump Variables")]
    public bool highJumpActive;
    [SerializeField]
    private float highJumpDuration;
    private float currentHighJumpDuration;
    [SerializeField]
    private GameObject highJumpFeedback;

    [Header("Smoke variables")]
    [SerializeField]
    private float smokeDuration;
    private float currentSmokeDuration;
    private bool smokeActive;
    [SerializeField]
    private GameObject smokeFeedback;

    [Header("Ice variables")]
    [SerializeField]
    private float iceDuration;
    private float currentIceDuration;
    private bool iceActive;
    [SerializeField]
    private GameObject iceFeedback;

    private void Start()
    {
        playerController = this.GetComponent<PlayerController>();
        AudioManager = AudioManager.Instance;

    }

    private void Update()
    {
        //manget power up
        if (magnetActive)
        {
            currentMagnetDuration -= Time.deltaTime;
            if (currentMagnetDuration <= 0)
                DisableMagnetPowerUp();
        }

        //spring power up
        if (highJumpActive)
        {
            currentHighJumpDuration -= Time.deltaTime;
            if (currentHighJumpDuration <= 0)
                DisableJumpPowerUp();
        }

        if (smokeActive)
        {
            currentSmokeDuration -= Time.deltaTime;
            if (currentSmokeDuration <= 0)
                DisableSmoke();
        }

        if (iceActive)
        {
            currentIceDuration -= Time.deltaTime;
            if (currentIceDuration <= 0)
                DisableIce();
        }

    }

    //magnet power up
    void ActivateMagnetPowerUp()
    {
        currentMagnetDuration = magnetDuration;
        magnetActive = true;
        magnetRegion.SetActive(true);
    }
    void DisableMagnetPowerUp()
    {
        magnetActive = false;
        magnetRegion.SetActive(false);
    }

    //spring power up
    void EnableJumpPowerUp()
    {
        currentHighJumpDuration = highJumpDuration;
        highJumpFeedback.SetActive(true);
        highJumpActive = true;
        playerController.SetJumpHeight();
    }
    void DisableJumpPowerUp()
    {
        highJumpFeedback.SetActive(false);
        highJumpActive = false;
        playerController.SetJumpHeight();
    }

    //smoke power down
    void EnableSmokeOnOtherPlayers()
    {
        var otherPlayers = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < otherPlayers.Length; i++)
        {
            otherPlayers[i].GetComponent<PlayerPowerUpManager>().EnableSmoke();
        }
    }
    void EnableSmoke()
    {
        currentSmokeDuration = smokeDuration;
        smokeActive = true;
        smokeFeedback.SetActive(true);
    }

    void DisableSmoke()
    {
        smokeActive = false;
        smokeFeedback.SetActive(false);
    }

    //ice power down
    void EnableIceOnOtherPlayers()
    {
        var otherPlayers = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < otherPlayers.Length; i++)
        {
            AudioManager.PlaySound("IcePowerUp");
            otherPlayers[i].GetComponent<PlayerPowerUpManager>().EnableIce();
        }
    }

    void EnableIce()
    {
        currentIceDuration = iceDuration;
        iceActive = true;
        playerController.iceIsActive = iceActive;
        iceFeedback.SetActive(true);
    }

    void DisableIce()
    {
        iceActive = false;
        playerController.iceIsActive = iceActive;
        iceFeedback.SetActive(false);
    }


    //collision checks
    private void OnCollisionEnter2D(Collision2D other)
    {
        //magnet power up
        if (other.gameObject.CompareTag("Magnet"))
        {
            ActivateMagnetPowerUp();
            AudioManager.PlaySound("PowerUpCollected");
            other.gameObject.GetComponent<PowerUpDespawner>().Destroy();
        }

        //spring power up
        if (other.gameObject.CompareTag("Spring"))
        {
            EnableJumpPowerUp();
            AudioManager.PlaySound("PowerUpCollected");
            other.gameObject.GetComponent<PowerUpDespawner>().Destroy();
        }

        //smoke power up
        if (other.gameObject.CompareTag("Smoke"))
        {
            EnableSmokeOnOtherPlayers();
            AudioManager.PlaySound("PowerUpCollected");
            other.gameObject.GetComponent<PowerUpDespawner>().Destroy();
        }
        //ice power up
        if (other.gameObject.CompareTag("Ice"))
        {
            EnableIceOnOtherPlayers();
            AudioManager.PlaySound("PowerUpCollected");
            other.gameObject.GetComponent<PowerUpDespawner>().Destroy();
        }




    }
}
