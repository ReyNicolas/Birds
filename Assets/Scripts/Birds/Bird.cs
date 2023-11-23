using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer; 
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Zone zoneToMove;
    [SerializeField] BirdSO birdData;
    int speed;
    BranchFinder branchFinder;
    bool inZone;
    float angryTimer;
    Transform angryTransform;
    float followTimer;
    Transform followTransform;


    private void Awake()
    {
        branchFinder = BranchFinder.GenerateFinder(birdData.DistanceToFind,birdData.BranchMask,birdData.FindBranch); // new InFrontBranchFinder(distanceToFind,branchMask);
        speed = birdData.Speed;
    }
    void Update()
    {
        if (angryTransform != null)
        {
            EscapeFromObject();
            return;
        }
        if (followTransform != null)
        {
            FollowObject();
            return;
        }

        if (inZone) return;

        TryFindZoneFromABranch();

        if (zoneToMove != null)
            MoveToZone();
    }


    private void LateUpdate()
    {
        transform.up = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<FollowObject>(out FollowObject followObject))
        {
            Destroy(followObject.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ArrivedObjetiveZone(collision);

        AngryObjectNear(collision);
        ToFollowObjectNear(collision);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitArrivedZone(collision);
    }
     void AngryObjectNear(Collider2D collision)
    {
        if (collision.TryGetComponent<AngryObject>(out AngryObject angryObject))
        {
            angryTransform = angryObject.transform;
            angryTimer = birdData.AngryTime;
        }
    }
     void ToFollowObjectNear(Collider2D collision)
    {
        if (collision.TryGetComponent<FollowObject>(out FollowObject followObject))
        {
            followTransform = followObject.transform;
            followTimer = birdData.FollowTime;
        }
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public Color GetColor()
    {
        return spriteRenderer.color;
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
    void EscapeFromObject()
    {
        angryTimer -= Time.deltaTime;

        rb.velocity = (transform.position - angryTransform.position).normalized * speed;
        if (angryTimer <= 0)
        {
            angryTransform = null;
        }
    }
    void FollowObject()
    {
        followTimer -= Time.deltaTime;

        rb.velocity = (followTransform.position - transform.position).normalized * speed;
        if (followTimer <= 0)
        {
            followTransform = null;
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


