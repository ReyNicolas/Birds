using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomePlayerPanel : MonoBehaviour
{
    [SerializeField] PlayerSO playerData;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Image playerImage;

    private void Start()
    {
        playerName.text = playerData.PlayerName;
        playerName.color = playerData.PlayerColor;
        // playerImage.sprite = playerData.playerSprite;
    }

    public void SetName(bool isBot)
    {
        if (isBot)
        {
            playerName.text = playerData.PlayerName + " BOT";
            return;
        } 
        playerName.text = playerData.PlayerName;
    }
}