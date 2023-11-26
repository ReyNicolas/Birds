using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Branch : MonoBehaviour
{
    [SerializeField] List<Zone> zones;

    public static Action<int, Color> OnPointsToColor; 

    private void OnEnable()
    {
        zones.ForEach(zone =>  zone.OnBirdEnter += (_ => CheckAllBirdsInBranchSameColor()));        
    }
    private void OnDisable()
    {
        zones.ForEach(zone => zone.OnBirdEnter -= (_ => CheckAllBirdsInBranchSameColor()));
    }  
    
    public List<Bird> GetBirdsInBranch()
    {
        return 
            zones
                .Select(zone=> zone.myBird)
                .Where(bird=> bird != null)
                .Distinct()
                .ToList();
    }

    public Zone TryGiveMeCloseZone(Vector2 point)
    {
        var freeZones = zones.Where(z => z.myBird == null);

        return (freeZones.Count() > 0)
            ? freeZones
                .OrderBy(z=> Vector2.Distance(z.transform.position,point))
                .First() 
            : null;
    }
    public Zone TryGiveMeCloseZoneFreeOrWithNormalBird(Vector2 point)
    {
        var freeZones = zones.Where(z => z.myBird == null || (z.myBird !=null && !z.myBird.GetComponent<FatBird>()));

        return (freeZones.Count() > 0)
            ? freeZones
                .OrderBy(z => Vector2.Distance(z.transform.position, point))
                .First()
            : null;
    }
    void CheckAllBirdsInBranchSameColor()
    {
        if (ThereAreEmptyZones())
            return;
        if (AllZoneBirdsAreTheSameColor())
        {
            OnPointsToColor?.Invoke(GetDistinctsBirdsInZoneCount(), GetFirstZoneBirdColor());
            zones.Select(zone=>zone.myBird).Distinct().ToList().ForEach(zone => Destroy(zone.gameObject));
        }
    }

    Color GetFirstZoneBirdColor() 
        => zones.First().myBird.GetColor();

    int GetDistinctsBirdsInZoneCount() 
        => zones.Select(zone => zone.myBird).Distinct().Count();

    bool AllZoneBirdsAreTheSameColor() 
        => zones.GroupBy(zone => zone.myBird.GetColor()).Count() == 1;

    bool ThereAreEmptyZones()
        => zones.Any(zone => zone.myBird == null);
}
