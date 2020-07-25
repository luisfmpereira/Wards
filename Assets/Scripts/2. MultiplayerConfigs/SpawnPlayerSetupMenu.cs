using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


public class SpawnPlayerSetupMenu : MonoBehaviour
{

    public GameObject playerSetupMenuPrefab;
    public PlayerInput input;
    private AudioManager audioManager;
    [SerializeField]
    PlayerConfigurationManager playerConfigurationManager;
    private int currentSpawn;
    public int playerId;


    void Awake()
    {
        audioManager = AudioManager.Instance;
        playerConfigurationManager = PlayerConfigurationManager.Instance;
        playerId = input.user.index;
        audioManager.PlaySound("ButtonClick");

        //instantiate and disable text
        var menu = Instantiate(playerSetupMenuPrefab, playerConfigurationManager.waitingPanels[playerId].transform);
        playerConfigurationManager.waitingPanels[playerId].transform.GetChild(0).gameObject.SetActive(false);

        input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
        menu.GetComponent<PlayerSetupMenuController>().SetPlayerIndex(input.playerIndex);
        menu.GetComponent<PlayerSetupMenuController>().GetInputReference(input);


    }
}
