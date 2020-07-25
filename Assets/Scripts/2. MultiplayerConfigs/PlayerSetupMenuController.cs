using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;
using static UnityEngine.InputSystem.InputAction;


public class PlayerSetupMenuController : MonoBehaviour
{
    private int playerIndex;
    private PlayerControls controls;
    private float ignoreInputTime = 0.5f;
    private bool inputEnabled;
    private AudioManager AudioManager;
    public PlayerStates currentState;


    [Header("Objects references")]
    [SerializeField]
    private TextMeshProUGUI playerIdText;
    [SerializeField]
    private Image characterSpriteUI;
    [SerializeField]
    private TextMeshProUGUI skillTextUI;
    [SerializeField]
    private Image externalBackgroundUI;
    [SerializeField]
    private GameObject inSelectionCharacterUIElements;
    [SerializeField]
    private GameObject selectedcharacterUIElements;
    [SerializeField]
    private GameObject arrowRight;
    [SerializeField]
    private GameObject arrowLeft;
    private Image arrowRightImage;
    private Image arrowLeftImage;


    [Header("Character selection")]
    [SerializeField]
    private int currentCharacterInt;
    [SerializeField]
    private float joystickCooldown;
    private float currentJoystickCooldown;
    private Vector2 joystickDirection;
    private PlayerInput input;

    [Header("Characters prefabs and sprites")]
    [SerializeField]
    private GameObject[] charatersPrefabsArray;
    [SerializeField]
    private Sprite[] charactersSpritesArray;
    [SerializeField]
    private string[] charactersSkillsTextArray;
    [SerializeField]
    private Color[] charactersMainColorArray;


    public enum PlayerStates
    {
        SelectingCharacter,
        WaitingForOthers,
        Playing
    }


    private void Start()
    {
        PlayerPrefs.SetString("PlayersState", "SelectingCharacter");

        AudioManager = AudioManager.Instance;
        controls = new PlayerControls();
        input.onActionTriggered += Input_onActionTriggered;

        arrowRightImage = arrowRight.GetComponent<Image>();
        arrowLeftImage = arrowLeft.GetComponent<Image>();

        currentCharacterInt = 0;
        currentState = PlayerStates.SelectingCharacter;
        ChangeUIToCurrentSelection();
    }

    //get player references
    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        playerIdText.SetText("P " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    public void GetInputReference(PlayerInput x) => input = x;

    void Update()
    {
        if (Time.time > ignoreInputTime)
            inputEnabled = true;

        if (currentJoystickCooldown >= 0)
            currentJoystickCooldown -= Time.deltaTime;

        if (currentJoystickCooldown < 0 && currentState == PlayerStates.SelectingCharacter)
        {
            if (joystickDirection.x > 0)
                MoveSelectionToRight();
            if (joystickDirection.x < 0)
                MoveSelectionToLeft();
        }

        switch (currentState)
        {
            case PlayerStates.SelectingCharacter:

                inSelectionCharacterUIElements.SetActive(true);
                selectedcharacterUIElements.SetActive(false);
                break;

            case PlayerStates.WaitingForOthers:

                inSelectionCharacterUIElements.SetActive(false);
                selectedcharacterUIElements.SetActive(true);
                break;

            default:
                break;

        }


    }

    // INPUT CONSEQUENCES
    private void Input_onActionTriggered(CallbackContext obj)
    {
        if (PlayerPrefs.GetString("PlayersState") == "SelectingCharacter")
        {
            if (obj.action.name == controls.Player.Movement.name)
                OnMove(obj);
            if (obj.action.name == controls.Player.Jump.name)
                if (currentState == PlayerStates.SelectingCharacter)
                    SetCharacter();
            if (obj.action.name == controls.Player.Cancel.name)
                if (currentState == PlayerStates.WaitingForOthers)
                    ReturnSelection();
        }

    }

    void OnMove(CallbackContext context) => joystickDirection = context.ReadValue<Vector2>();


    void ChangeUIToCurrentSelection()
    {
        characterSpriteUI.sprite = charactersSpritesArray[currentCharacterInt];
        skillTextUI.text = charactersSkillsTextArray[currentCharacterInt];
        skillTextUI.color = charactersMainColorArray[currentCharacterInt];
        externalBackgroundUI.color = charactersMainColorArray[currentCharacterInt];
    }

    void ResetJoystickCooldown() => currentJoystickCooldown = joystickCooldown;


    IEnumerator BlinkImage(Image image)
    {
        image.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        image.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }


    public void SetCharacter()
    {
        if (!inputEnabled) return;
        currentState = PlayerStates.WaitingForOthers;

        AudioManager.Instance.PlaySound("PlayerSelected");

        PlayerConfigurationManager.Instance.SetPlayerCharacter(playerIndex,
        charatersPrefabsArray[currentCharacterInt], charactersSpritesArray[currentCharacterInt]);

        ReadyPlayer();


    }
    public void ReturnSelection()
    {
        if (!inputEnabled) return;
        currentState = PlayerStates.SelectingCharacter;

        AudioManager.Instance.PlaySound("ClickBack");

        PlayerConfigurationManager.Instance.SetPlayerCharacter(playerIndex, null, null);

        UnreadyPlayer();
    }


    // READY AND UNREADY PLAYER
    public void ReadyPlayer()
    {
        if (!inputEnabled) return;
        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        AudioManager.PlaySound("ButtonClick");
    }

    public void UnreadyPlayer()
    {
        if (!inputEnabled) return;
        PlayerConfigurationManager.Instance.UnreadyPlayer(playerIndex);
        AudioManager.PlaySound("ButtonClick");

    }

    // MOVING CHARACTER SELECTION
    void MoveSelectionToRight()
    {
        StartCoroutine(BlinkImage(arrowRightImage));
        if (currentCharacterInt == charatersPrefabsArray.Length - 1)
            currentCharacterInt = 0;
        else
            currentCharacterInt += 1;

        ChangeUIToCurrentSelection();
        ResetJoystickCooldown();
    }


    void MoveSelectionToLeft()
    {
        StartCoroutine(BlinkImage(arrowLeftImage));
        if (currentCharacterInt == 0)
            currentCharacterInt = charatersPrefabsArray.Length - 1;
        else
            currentCharacterInt -= 1;

        ChangeUIToCurrentSelection();
        ResetJoystickCooldown();
    }


}
