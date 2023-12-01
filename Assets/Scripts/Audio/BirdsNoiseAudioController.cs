using System.Collections;
using System.Linq;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

public class BirdsNoiseAudioController: MonoBehaviour
{
    [SerializeField] MatchSO matchData;
    [SerializeField] AudioSource noiseSource;
    CompositeDisposable disposables;


    private void Start()
    {
        disposables = new CompositeDisposable(
            matchData.winnerData
            .Where(wd => wd != null)
            .Subscribe(_ => noiseSource.Stop())
            );
        StartCoroutine(SetNoiseVolume());    
    }

    private void OnDestroy()
    {
        disposables.Dispose(); 
    }
    IEnumerator SetNoiseVolume()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            noiseSource.volume += matchData.numberBirdsInScene.Value > 0 ? 0.05f * matchData.numberBirdsInScene.Value : -0.05f;
            noiseSource.volume = math.max(noiseSource.volume, 0.1f);
        }

    }
}




