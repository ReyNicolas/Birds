using System.Collections.Generic;
using UnityEngine;

public class BirdGenerator: IObjectGenerator
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

    public GameObject Generate()
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

public interface IObjectGenerator
{
    GameObject Generate();
}