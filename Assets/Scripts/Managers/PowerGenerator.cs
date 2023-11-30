using UnityEngine;

public class PowerGenerator: IObjectGenerator
{
    MatchSO matchData;
    PositionGenerator positionGenerator;

    public PowerGenerator(MatchSO matchData, PositionGenerator positionGenerator)
    {
        this.matchData = matchData;
        this.positionGenerator = positionGenerator;
    }

    public GameObject Generate()
    {
        var powerGO = GameObject.Instantiate(GetRandomPowerPrefab());
        positionGenerator.AssignPosition(powerGO);
        return powerGO;
    }
    GameObject GetRandomPowerPrefab()
        => matchData.powersPrefabs[Random.Range(0, matchData.powersPrefabs.Count)];

}

