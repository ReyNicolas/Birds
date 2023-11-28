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
    int gamepads, playerCount;


    private void Start()
    {
        SetPoinstPlayer();
        SetPoinstMatch();
        InputSystem.onDeviceChange += OnDeviceChange;
        SetKeyboardPlayersCount();
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        CountPlayers();
    }
    public void CountPlayers()
    {
        gamepads = Gamepad.all.Count;
        playerCount = matchData.KeyboardPlayersCount + gamepads;
        gamepadCount.text = gamepads.ToString();
        ActivePlayerPanels();
    }

    void ActivePlayerPanels()
    {
        playersPanels.ForEach(pp => pp.gameObject.SetActive(false));
        for (int i = 0; i < Math.Min(playerCount, playersDatas.Count); i++)
        {
            playersDatas[i].PlayerColor = matchData.posibleBirdsColors[i];
            playersPanels[i].gameObject.SetActive(true);

            if (i < matchData.KeyboardPlayersCount)
            {
                playersDatas[i].InputDevice = "Keyboard" + (i + 1);
            }
            else
            {
                playersDatas[i].InputDevice = "Gamepad" + ((i +1 ) - matchData.KeyboardPlayersCount);
            }

            playersPanels[i].SetMyPlayer(playersDatas[i]);
        }
    }

    public void StartMatch()
    {
        playerCount = matchData.KeyboardPlayersCount + gamepads;
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
    public void SetKeyboardPlayersCount()
    {
        matchData.KeyboardPlayersCount = int.Parse(keyboardPlayers.text);
        CountPlayers();
    }
}







