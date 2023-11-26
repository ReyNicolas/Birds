using System.Linq;
using UnityEngine;

public class FatBird: Bird
{
    [SerializeField] CapsuleCollider2D capsuleCollider2D;
    [SerializeField] LayerMask birdMask;
    float escapeRadius;
    protected override void Awake()
    {
        base.Awake();
        escapeRadius = capsuleCollider2D.size.x;
        InvokeRepeating("ScareBirds", 0.1f, 0.1f);
    }
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

    void ScareBirds()
    {
        Physics2D
            .OverlapCircleAll(transform.position, escapeRadius, birdMask)
            .ToList()
            .ForEach(
                collider =>
                {
                    if (collider.TryGetComponent(out Bird bird) && bird!=this)
                        bird.GetToEscapeObject(transform);
                });
    }
    
}
