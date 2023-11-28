using UnityEngine;
using UniRx;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player Data")]
public class PlayerSO : ScriptableObject
{
    public Sprite playerSprite;
    public string PlayerName;
    public string InputDevice;
    public Color PlayerColor;
    public int PlayerScore;
    public ReactiveProperty<int> PointsToAdd = new ReactiveProperty<int>(0);

    public void Initialize()
    {
        PlayerScore = 0;
        ResetValuesForRound();
    }

    public void ResetValuesForRound()
    {
        PointsToAdd.Dispose();
        PointsToAdd = new ReactiveProperty<int>(0);
    }
}

