using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomePlayerPanel : MonoBehaviour
{
    [SerializeField] PlayerSO playerData;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Image playerImage;

    public void SetMyPlayer(PlayerSO playerData)
    {
        this.playerData = playerData;
        playerName.text = playerData.PlayerName;
        playerName.color = playerData.PlayerColor;
    }
}
