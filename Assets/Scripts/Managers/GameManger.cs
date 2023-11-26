using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField]MatchSO matchData;
    [SerializeField]List<UIPlayerPanel> playerPanels;
    BirdGenerator birdGenerator;
    PowerGenerator powerGenerator;
    PositionGenerator positionGenerator;
    PositionGenerator positionGeneratorWithPercentMargin;
    private void Awake()
    {
        CreateBirdGenerator();
        CreatePowerGenerator();
        SetPlayers();

        matchData.Initialize();
        Branch.OnPointsToColor += GivePointsToPlayer;
    }
    private void OnDestroy()
    {
        Branch.OnPointsToColor -= GivePointsToPlayer;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))// to test
        {
            birdGenerator.GenerateBird();
        }

        if (Input.GetKeyDown(KeyCode.P))// to test
        {
            powerGenerator.GeneratePower();
        }
    }
    void SetPlayers()
    {
        for (int i = 0; i < matchData.playersDatas.Count; i++)
        {
            var playerData = matchData.playersDatas[i];
            playerData.PlayerColor = matchData.posibleBirdsColors[i];
            playerPanels[i].Initiaze(playerData);
        }
    }

    void CreatePowerGenerator()
    {
        positionGeneratorWithPercentMargin = new PositionGenerator();
        positionGeneratorWithPercentMargin.SetDimension(matchData.percentMarginRespawn);
        powerGenerator = new PowerGenerator(matchData, positionGeneratorWithPercentMargin);
    }

    void CreateBirdGenerator()
    {
        positionGenerator = new PositionGenerator();
        positionGenerator.SetDimension();
        birdGenerator = new BirdGenerator(matchData.birdsPrefabs, matchData.posibleBirdsColors, positionGenerator);
    }

    void GivePointsToPlayer(int points, Color color)
    {
        var playerDataToAddPoints = matchData.playersDatas.Find(pd => pd.PlayerColor == color);
        if (playerDataToAddPoints != null)
            playerDataToAddPoints.PointsToAdd.Value += points;
    }

}


public class BirdGenerator
{
    List<GameObject> birdsPrefabs;
    List<Color> colors;
    PositionGenerator positionGenerator;

    public BirdGenerator(List<GameObject> birdsPrefabs, List<Color> colors, PositionGenerator positionGenerator)
    {
        this.birdsPrefabs = birdsPrefabs;
        this.colors = colors;
        this.positionGenerator = positionGenerator;
    }

    public GameObject GenerateBird()
    {
        var birdGO = GameObject.Instantiate(GetRandomBirdPrefab());
        positionGenerator.AssignPosition(birdGO);

        birdGO.GetComponent<Bird>().SetColor(GetRandomColor());
        return birdGO;        
    }
    GameObject GetRandomBirdPrefab() 
        => birdsPrefabs[Random.Range(0, birdsPrefabs.Count)];

    Color GetRandomColor()
        => colors[Random.Range(0, colors.Count)];
}

public class PowerGenerator
{
    MatchSO matchData;
    PositionGenerator positionGenerator;

    public PowerGenerator(MatchSO matchData, PositionGenerator positionGenerator)
    {
        this.matchData = matchData;
        this.positionGenerator = positionGenerator;
    }

    public GameObject GeneratePower()
    {
        var powerGO = GameObject.Instantiate(GetRandomPowerPrefab());
        positionGenerator.AssignPosition(powerGO);
        return powerGO;
    }
    GameObject GetRandomPowerPrefab()
        => matchData.powersPrefabs[Random.Range(0, matchData.powersPrefabs.Count)];

}

