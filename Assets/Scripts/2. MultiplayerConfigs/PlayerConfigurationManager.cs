using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerConfigurationManager : MonoBehaviour
{

    private List<PlayerConfiguration> playerConfigs;
    [SerializeField]
    private int maxPlayers;
    public static PlayerConfigurationManager Instance { get; private set; }
    [SerializeField]
    private string sceneToLoad;

    public GameObject[] waitingPanels;


    void Awake()
    {
        if (Instance != null)
            Debug.Log("Trying to create another instance of singleton!");
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public void SetPlayerCharacter(int index, GameObject character, Sprite chararacterSprite)
    {
        playerConfigs[index].PlayerCharacter = character;
        playerConfigs[index].PlayerCharacterSprite = chararacterSprite;
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void ReadyPlayer(int index)
    {
        if (playerConfigs[index].IsReady)
            return;

        playerConfigs[index].IsReady = true;
        if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.IsReady == true))
        {
            PlayerPrefs.SetString("PlayersState", "Playing");
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void UnreadyPlayer(int index)
    {
        playerConfigs[index].IsReady = false;
    }


    public void HandlePlayerJoin(PlayerInput pi)
    {
        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }

    public void ResetPlayers()
    {
        Destroy(this.gameObject);
    }


    public int DefineWinner()
    {

        //check if a draw happened
        var winnerIndex = -1;
        var winnerPoints = 0;
        var isDraw = false;


        for (int i = 0; i < playerConfigs.ToArray().Length; i++)
        {
            if (playerConfigs[i].PlayerScore == winnerPoints)
                isDraw = true;
            else if (playerConfigs[i].PlayerScore > winnerPoints)
            {
                isDraw = false;
                winnerIndex = playerConfigs[i].PlayerIndex;
                winnerPoints = playerConfigs[i].PlayerScore;
            }
        }
        if (isDraw)
            return -1;
        else
            return winnerIndex;
    }



    public void AddRoundVictory(int winnerIndex)
    {
        playerConfigs[winnerIndex].RoundVictories++;
    }

    public int CountActivePlayers()
    {
        return playerConfigs.Count(p => p.IsReady == true);
    }


    public void ResetPlayerScores()
    {
        for (int i = 0; i < playerConfigs.ToArray().Length; i++)
        {
            playerConfigs[i].PlayerScore = 0;
        }
    }


}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }

    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public GameObject PlayerCharacter { get; set; }
    public Sprite PlayerCharacterSprite { get; set; }
    public int PlayerScore { get; set; }
    public int RoundVictories { get; set; }

}