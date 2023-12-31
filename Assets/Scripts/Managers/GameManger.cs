using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    [SerializeField]MatchSO matchData;
    [SerializeField] GameObject optionsGO;
    [SerializeField]List<Transform> spawnTransforms;
    [SerializeField]List<UIPlayerPanel> playerPanels;

    BirdGenerator birdGenerator;
    PowerGenerator powerGenerator;
    PositionGenerator positionGenerator;
    PositionGenerator positionGeneratorWithPercentMargin;
    InputSetterManager inputSetterManager = new InputSetterManager();
    float birdTimer;
    float powerTimer;

    CompositeDisposable disposables;

    private void Awake()
    {
        CreateBirdGenerator();
        CreatePowerGenerator();
        matchData.Initialize();
        SetPlayers();
        SubscribeActions();
    }
    void CreateBirdGenerator()
    {
        positionGenerator = new PositionGenerator();
        positionGenerator.SetDimension();
        birdGenerator = new BirdGenerator(matchData.birdsPrefabs, matchData.posibleBirdsColors, positionGenerator);
    }
    void CreatePowerGenerator()
    {
        positionGeneratorWithPercentMargin = new PositionGenerator();
        positionGeneratorWithPercentMargin.SetDimension(matchData.percentMarginRespawn);
        powerGenerator = new PowerGenerator(matchData, positionGeneratorWithPercentMargin);
    }
    void SetPlayers()
    {
        for (int i = 0; i < matchData.playersDatas.Count; i++)
        {
            var playerData = matchData.playersDatas[i];
            playerData.PlayerColor = matchData.posibleBirdsColors[i];

            GameObject playerGO = Instantiate(matchData.playerPrefab, spawnTransforms[i].position, Quaternion.identity);
            Player player = playerGO.GetComponent<Player>();
            player.Initialize(playerData);

            playerPanels[i].gameObject.SetActive(true);
            playerPanels[i].Initiaze(player);
            
            inputSetterManager.SetPlayerInput(playerData.InputDevice, playerGO.GetComponent<PlayerInput>());
        }
    }
    void SubscribeActions()
    {
        Branch.OnPointsToColor += GivePointsToPlayer;

        Bird.OnNewBird += (_ => matchData.numberBirdsInScene.Value++);
        Bird.OnDestroyBird += (_ => matchData.numberBirdsInScene.Value--);

        Power.OnNewPower += (_ => matchData.numberPowersInScene++);
        Power.OnDestroyPower += (_ => matchData.numberPowersInScene--);
    }


    private void Start()
    {
        SetDisposables();
        SetTimers();
    }
    void SetTimers()
    {
        birdTimer = matchData.timeToGenerateBird;
        powerTimer = matchData.timeToGeneratePower;
    }
    void SetDisposables()
    {
        disposables = new CompositeDisposable(
                    matchData.winnerData
                    .Where(winner => winner != null)
                    .Subscribe(_ => StopGame()));

        matchData.playersDatas
        .ForEach(playerData
            => disposables.Add(  // add disposable playerData
                       playerData.PointsToAdd
                       .Subscribe(value => matchData.CheckWinner(value)))
        );
    }


    private void OnDestroy()
    {
        disposables.Dispose();
        UnSubscribeActions();
    }
    void UnSubscribeActions()
    {
        Branch.OnPointsToColor -= GivePointsToPlayer;

        Bird.OnNewBird -= (_ => matchData.numberBirdsInScene.Value++);
        Bird.OnDestroyBird -= (_ => matchData.numberBirdsInScene.Value--);

        Power.OnNewPower -= (_ => matchData.numberPowersInScene++);
        Power.OnDestroyPower -= (_ => matchData.numberPowersInScene--);
    }

    private void Update()
    {
        TryGenerateBird();
        TryGeneratePower();
    }
    void TryGeneratePower()
    {
        powerTimer -= Time.deltaTime;
        if (powerTimer <= 0 && matchData.NumberPowersIsLessMax())
        {
            powerTimer = matchData.timeToGeneratePower;
            powerGenerator.Generate();
        }
    }
    void TryGenerateBird()
    {
        birdTimer -= Time.deltaTime;
        if (birdTimer <= 0 && matchData.NumberBirdsIsLessMax())
        {
            birdTimer = matchData.timeToGenerateBird;
            birdGenerator.Generate();
        }
    }


    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsGO.SetActive(!optionsGO.activeSelf);
            Time.timeScale = optionsGO.activeSelf ? 0 : 1;
        }
    }

    

    void GivePointsToPlayer(int points, Color color)
    {
        var playerDataToAddPoints = matchData.playersDatas.Find(pd => pd.PlayerColor == color);
        if (playerDataToAddPoints != null)
            playerDataToAddPoints.PointsToAdd.Value += points;
    }

    void StopGame()
        => Time.timeScale = 0;

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(matchData.matchScene);
    }
    public void ReturnHomeMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(matchData.homeMenuScene);
    }

}

