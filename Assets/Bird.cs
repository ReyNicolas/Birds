using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] LayerMask branchMask;
    [SerializeField] int distanceToFind;
    [SerializeField] int speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Zone zoneToMove;
    BranchFinder branchFinder;
    bool inZone;
    bool angry;
    float angryTime = 2;
    float angryTimer;

    private void Awake()
    {
        branchFinder = new ClosestBranchFinder(distanceToFind,branchMask); // new InFrontBranchFinder(distanceToFind,branchMask);
    }
    void Update()
    {
        if (inZone) return;

        TryFindZoneFromABranch();

        if (zoneToMove != null)
            MoveToZone();
    }
    private void LateUpdate()
    {
        transform.up = rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ArrivedObjetiveZone(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitArrivedZone(collision);
    }

    void ArrivedObjetiveZone(Collider2D collision)
    {
        if (collision.TryGetComponent<Zone>(out Zone zone) && zone == zoneToMove)
        {
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            transform.SetPositionAndRotation(collision.transform.position, collision.transform.rotation);
            inZone = true;
        }
    }
    void ExitArrivedZone(Collider2D collision)
    {
        if (collision.TryGetComponent<Zone>(out Zone zone) && zone == zoneToMove)
        {
            rb.gravityScale = 1;
            inZone = false;
            zoneToMove = null;
        }
    }
    void TryFindZoneFromABranch() 
        => zoneToMove
            = branchFinder
            .TryFindBranch(transform)
            ?.TryGiveMeCloseZone(transform.position);


    void MoveToZone() 
        => rb.velocity = (zoneToMove.transform.position - transform.position).normalized * speed;
}

