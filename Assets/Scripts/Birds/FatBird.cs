using System.Linq;
using UnityEngine;

public class FatBird: Bird
{
    [SerializeField] CapsuleCollider2D capsuleCollider2D;
    [SerializeField] Transform mouthTransform;
    [SerializeField] LayerMask birdMask;
    float escapeRadius;
    protected override void Awake()
    {
        base.Awake();
        escapeRadius = capsuleCollider2D.size.x * 0.5f;
        InvokeRepeating("ScareBirds", 0.1f, 0.1f);
    }
    protected  void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Zone>(out Zone zone))
            zone.SetMyBird(this);
    }

    public override void GetToEscapeObject(Transform transformToEscape)
    {
        if (transformToEscape.GetComponent<Mouth>()) return;
        base.GetToEscapeObject(transformToEscape);
    }

    protected override void TryFindZoneFromABranch()
    {
        zoneToMove
            = branchFinder
            .TryFindBranch(transform)
            ?.TryGiveMeCloseZoneFreeOrWithNormalBird(transform.position);
        if (zoneToMove != null)
        {
            myState = BirdState.MovingToZone;
            stateTransform = zoneToMove.transform;
            stateTimer = 1;
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
                        bird.GetToEscapeObject(mouthTransform);
                });
    }



}
