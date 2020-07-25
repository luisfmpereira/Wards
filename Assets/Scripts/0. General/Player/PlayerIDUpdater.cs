using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerIDUpdater : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField]
    private TextMeshProUGUI IdText;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        SetIdText();
    }

    private void SetIdText()
    {
        IdText.text = (playerController.playerIndex + 1).ToString();
    }

}
