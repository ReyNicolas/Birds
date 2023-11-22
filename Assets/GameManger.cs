using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField]MatchSO matchData;
    BirdGenerator birdGenerator;
    PositionGenerator positionGenerator;
    private void Awake()
    {
        positionGenerator = new PositionGenerator();
        positionGenerator.SetDimension();
        birdGenerator = new BirdGenerator(matchData.birdsPrefabs, matchData.posibleBirdsColors, positionGenerator);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            birdGenerator.GenerateBird();
        }
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

