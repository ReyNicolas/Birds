using System;
using UniRx;
using UnityEngine;

public class BirdsAudioControl : MonoBehaviour
{
    MatchSO matchData;
    [SerializeField] AudioManager audioManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] int minBirds = 2;
    IDisposable disposable;

    private void Start()
    {
        matchData = audioManager.matchData;
        disposable = matchData.numberBirdsInScene.Subscribe(_ => UpdateVolume());
    }

    private void OnDestroy()
    {
        disposable.Dispose();
    }

    void UpdateVolume()
    {
        Debug.Log("Update");
        audioSource.volume = (float)(matchData.numberBirdsInScene.Value * 3 + 2)/ matchData.maxNumberOfBirds;
    }
}
