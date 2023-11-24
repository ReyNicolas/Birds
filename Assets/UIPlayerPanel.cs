using TMPro;
using UniRx;
using UnityEngine;

public class UIPlayerPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointsTxt;
    [SerializeField] PlayerSO playerData;
    CompositeDisposable disposables;

    public void Initiaze(PlayerSO playerData)
    {
        this.playerData = playerData;

    }

    private void Start()
    {
        disposables = new CompositeDisposable()
        {
            playerData.PointsToAdd.Subscribe(value => pointsTxt.text = value.ToString())
        };
    }
    private void OnDestroy()
    {
        disposables.Dispose();
    }
}
