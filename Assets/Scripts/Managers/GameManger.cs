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
    [SerializeField]List<UIPlayerPanel> playerPanels;

    BirdGenerator birdGenerator;
    PowerGenerator powerGenerator;
    PositionGenerator positionGenerator;
    PositionGenerator positionGeneratorWithPercentMargin;
    float birdTimer;
    float powerTimer;

    CompositeDisposable disposables;

    private void Awake()
    {
        CreateBirdGenerator();
        CreatePowerGenerator();
        SetPlayers();

        matchData.Initialize();
        
        Branch.OnPointsToColor += GivePointsToPlayer;

        Bird.OnNewBird += (_=> matchData.numberBirdsInScene++);
        Bird.OnDestroyBird += (_ => matchData.numberBirdsInScene--);

        Power.OnNewPower += (_ => matchData.numberPowersInScene++);
        Power.OnDestroyPower += (_ => matchData.numberPowersInScene--);
    }
    private void Start()
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

        birdTimer = matchData.timeToGenerateBird;
        powerTimer = matchData.timeToGeneratePower;
    }

    private void OnDestroy()
    {
        disposables.Dispose();

        Branch.OnPointsToColor -= GivePointsToPlayer;

        Bird.OnNewBird -= (_ => matchData.numberBirdsInScene++);
        Bird.OnDestroyBird -= (_ => matchData.numberBirdsInScene--);

        Power.OnNewPower -= (_ => matchData.numberPowersInScene++);
        Power.OnDestroyPower -= (_ => matchData.numberPowersInScene--);
    }

    private void Update()
    {
        birdTimer -= Time.deltaTime;
        if(birdTimer <=0 && matchData.NumberBirdsIsLessMax())
        {
            birdTimer = matchData.timeToGenerateBird;
            birdGenerator.GenerateBird();
        }

        powerTimer -= Time.deltaTime;
        if (powerTimer <= 0 && matchData.NumberPowersIsLessMax())
        {
            powerTimer = matchData.timeToGeneratePower;
            powerGenerator.GeneratePower();
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
        }
    }

    void SetPlayers()
    {
        for (int i = 0; i < matchData.playersDatas.Count; i++)
        {
            var playerData = matchData.playersDatas[i];
            playerData.PlayerColor = matchData.posibleBirdsColors[i];


            GameObject playerGO = Instantiate(matchData.playerPrefab);
            playerGO.GetComponent<Player>().Initialize(playerData);

            if(i < matchData.KeyboardPlayersCount) //First players take keyboards inputs
            {
                SetKeyboardInput(i, playerGO.GetComponent<PlayerInput>());
            }
            else
            {
                 SetGamepadInput(i - matchData.KeyboardPlayersCount, playerGO.GetComponent<PlayerInput>());
            }

            playerPanels[i].Initiaze(playerData);
        }
    }
    void SetKeyboardInput(int index, PlayerInput playerInput)
    {
        playerInput.user.UnpairDevices();
        InputUser.PerformPairingWithDevice(Keyboard.current, user: playerInput.user);
        playerInput.SwitchCurrentActionMap("Keyboard" + index);
    }

    void SetGamepadInput(int index, PlayerInput playerInput)
    {
        playerInput.user.UnpairDevices();
        InputUser.PerformPairingWithDevice(Gamepad.all[index], user: playerInput.user);
        playerInput.SwitchCurrentActionMap("Gamepad"); 
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

