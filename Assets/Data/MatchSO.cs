using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using System;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "MatchData", menuName = "Match Data")]
public class MatchSO : ScriptableObject
{
    [Header("Data")]
    public string homeMenuScene;
    public string matchScene;
    public List<Color> posibleBirdsColors = new List<Color>();
    public List<PlayerSO> playersDatas;
    public GameObject playerPrefab;
    public List<GameObject> birdsPrefabs = new List<GameObject>() ;
    [Header("End conditions Settings")]
    public int totalPointsLimit;
    public int totalPlayerPointsLimit;
    public float timeLeft;
    [Header("Birds Settings")]
    public int numberBirdsInScene;
    public int timeToGenerateBird;
    public int maxNumberOfBirds;
    [Header("Sound Settings")]
    public AudioMixer mixer;
    public ReactiveProperty<float> masterVolume = new ReactiveProperty<float>(1);
    public ReactiveProperty<float> musicVolume = new ReactiveProperty<float>(1);
    public ReactiveProperty<float> soundEffectsVolume = new ReactiveProperty<float>(1);
    public ReactiveProperty<float> interfaceVolume = new ReactiveProperty<float>(1);
    public float volumeMultiplier;
    public List<AudioClip> actualMusicToPlay;
    CompositeDisposable audioDisposables;


    public ReactiveProperty<PlayerSO> winnerData = new ReactiveProperty<PlayerSO>(null);

    public void DisposeAudio()
    {
        audioDisposables.Dispose();
    }

    public void SetAudioMixer()
    {
        audioDisposables = new CompositeDisposable(
            masterVolume.Subscribe(value => SetAudio("master", value / 100)),
            musicVolume.Subscribe(value => SetAudio("bgm", value / 100)),
            soundEffectsVolume.Subscribe(value => SetAudio("sfx", value / 100)),
            interfaceVolume.Subscribe(value => SetAudio("sui", value / 100))
        );
    }
    void SetAudio(string param, float value)
        => mixer.SetFloat(param, Mathf.Log10(value) * volumeMultiplier);

    public void Initialize()
    {
        numberBirdsInScene = 0;
        playersDatas.ForEach(playerData => playerData.Initialize());
        winnerData.Dispose();
        winnerData = new ReactiveProperty<PlayerSO>(null);
    }

    public void CheckWinner(int value)
    {
        if (EndedByTotalPointsLimit() || EndedByTotalPlayerPointsLimit())
        {
            SetWinner();
        }
    }

    void SetWinner()
    {
        winnerData.Value = playersDatas.OrderByDescending(pd => pd.PointsToAdd.Value).First();
    }

    bool EndedByTotalPlayerPointsLimit()
    {
        return playersDatas.Any(pd => pd.PointsToAdd.Value >= totalPlayerPointsLimit);
    }

    bool EndedByTotalPointsLimit()
    {
        return (playersDatas.Sum(pd => pd.PointsToAdd.Value)) >= totalPointsLimit;
    }


}

