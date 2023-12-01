using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointsTxt;
    [SerializeField] GameObject launchImageGO;
    [SerializeField] Player player;
    [SerializeField] PlayerSO playerData;
    CompositeDisposable disposables;

    public void Initiaze(Player player)
    {
        this.player = player;
        this.playerData = player.playerData;
        launchImageGO.GetComponent<Image>().color = playerData.PlayerColor;
        pointsTxt.color = playerData.PlayerColor;

        SetDisposables();
    }

    void SetDisposables()
    {
        disposables = new CompositeDisposable()
        {
            playerData.PointsToAdd.Subscribe(value => pointsTxt.text = value.ToString()),
            player.shootTimer.Where(value=>value<0).Subscribe(value => launchImageGO.SetActive(true)),
            player.shootTimer.Where(value=>value>=0).Subscribe(value => launchImageGO.SetActive(false))
        };
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }
}
