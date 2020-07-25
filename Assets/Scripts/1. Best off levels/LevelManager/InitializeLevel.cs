using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InitializeLevel : MonoBehaviour
{
    private GameObject playerSpawnsMaster;
    [SerializeField]
    private Transform[] playerSpawns;
    [SerializeField]
    private GameObject[] levelsDesign;
    public bool notFirstLevelSpawn;
    private GameObject[] playerObjects;
    void Awake()
    {
        //get all players information
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        //reset all players score
        PlayerConfigurationManager.Instance.ResetPlayerScores();
        //randomize level
        Instantiate(levelsDesign[Random.Range(0, levelsDesign.Length)], Vector3.zero, Quaternion.identity);


        playerSpawnsMaster = GameObject.Find("PlayerSpawns");
        playerSpawns = playerSpawnsMaster.GetComponentsInChildren<Transform>().Where(x => x != playerSpawnsMaster.transform).ToArray();


        //destroy players
        playerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (PlayerPrefs.GetInt("FirstRound") > 0)
        {
            for (int i = 0; i < playerObjects.Length; i++)
            {
                Destroy(playerObjects[i]);
            }
        }




        //instantiate player objects
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            if (playerConfigs[i].IsReady)
            {
                //instantiate each selected character in the real positions
                var player = Instantiate(playerConfigs[i].PlayerCharacter, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
                //initialize each player controller
                player.GetComponent<PlayerController>().InitializePlayer(playerConfigs[i]);
            }
        }
    }





}
