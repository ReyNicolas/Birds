using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HomeMenuManager : MonoBehaviour
{
    [SerializeField] List<PlayerSO> playersDatas;
    [SerializeField] List<HomePlayerPanel> playersPanels;
    [SerializeField] MatchSO matchData;
    [SerializeField] TextMeshProUGUI playerPointsLabel;
    [SerializeField] TextMeshProUGUI matchPointsLabel;
    [SerializeField] TextMeshProUGUI gamepadCount;
    [SerializeField] TextMeshProUGUI keyboardPlayers;
    [SerializeField] TextMeshProUGUI errorMessage;
    int gamepadsCount, playerCount;
    readonly string KEYBOARD1 = "Keyboard1";
    readonly string KEYBOARD2 = "Keyboard2";
    readonly string GAMEPAD = "Gamepad";

    [SerializeField] List<string> keyboards = new List<string>();


    private void Start()
    {
        SetPoinstPlayer();
        SetPoinstMatch();
        InputSystem.onDeviceChange += OnDeviceChange;
        UpdatePlayers();
    }
    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
    void OnDeviceChange(InputDevice device, InputDeviceChange change) 
        => UpdatePlayers();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (keyboards.Contains(KEYBOARD1))
                keyboards.Remove(KEYBOARD1);
            else
                keyboards.Add(KEYBOARD1);

            UpdatePlayers();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (keyboards.Contains(KEYBOARD2))
                keyboards.Remove(KEYBOARD2);
            else
                keyboards.Add(KEYBOARD2);

            UpdatePlayers();
        }
    }


    public void UpdatePlayers()
    {
        gamepadsCount = Gamepad.all.Count;
        playerCount = keyboards.Count + gamepadsCount;
        gamepadCount.text = gamepadsCount.ToString();
        SetDatas();
    }

    void SetDatas()
    {
        playersPanels.ForEach(pp => pp.gameObject.SetActive(false));

        for (int i = 0; i < keyboards.Count; i++)
        {
            playersDatas[i].PlayerColor = matchData.posibleBirdsColors[i];
            playersDatas[i].InputDevice = keyboards[i];

            playersPanels[i].gameObject.SetActive(true);
            playersPanels[i].SetMyPlayer(playersDatas[i]);
        }
        for (int i = keyboards.Count; i < Math.Min(playerCount, playersDatas.Count); i++)
        {
            playersDatas[i].PlayerColor = matchData.posibleBirdsColors[i];
            playersDatas[i].InputDevice = GAMEPAD + ((i +1 ) - keyboards.Count);

            playersPanels[i].gameObject.SetActive(true);
            playersPanels[i].SetMyPlayer(playersDatas[i]);
        }
    }

    public void StartMatch()
    {
        if (playerCount == 0)
        {
            errorMessage.text = "Add at least one gamepad or keyboard";
            return;
        }

       matchData.playersDatas = playersDatas.Take(playerCount).ToList();

        InputSystem.onDeviceChange -= OnDeviceChange;
        matchData.Initialize();
        SceneManager.LoadScene(matchData.matchScene);
    }

    public void SetPoinstPlayer()
    {
        matchData.totalPlayerPointsLimit = int.Parse(playerPointsLabel.text);
    }

    public void SetPoinstMatch()
    {
        matchData.totalPointsLimit = int.Parse(matchPointsLabel.text);
    }
    
}







