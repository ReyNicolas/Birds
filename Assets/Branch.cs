using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Branch : MonoBehaviour
{
    [SerializeField] List<Zone> zones;
    
    public Zone TryGiveMeCloseZone(Vector2 point)
    {
        var freeZones = zones.Where(z => z.myBird == null);

        return (freeZones.Count() > 0)
            ? freeZones.OrderBy(z=> Vector2.Distance(z.transform.position,point)).First() 
            : null;
    }
}
