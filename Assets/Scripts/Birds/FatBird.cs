using UnityEngine;

public class FatBird: Bird
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.TryGetComponent<Zone>(out Zone zone))
            zone.SetMyBird(this);
    }

    protected override void TryFindZoneFromABranch()
    {
        zoneToMove
            = branchFinder
            .TryFindBranch(transform)
            ?.TryGiveMeCloseZoneFreeOrWithNormalBird(transform.position);
        if (zoneToMove != null)
        {
            myState = BirdState.Following;
            stateTransform = zoneToMove.transform;
        }
    }
    
}
