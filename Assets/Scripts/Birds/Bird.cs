using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Transform bodyTransform;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Zone zoneToMove;
    [SerializeField] BirdSO birdData;
    [SerializeField] Animator animator;
    [SerializeField]BirdState myState;
    bool fliped;
    int speed;
    BranchFinder branchFinder;
    float stateTimer;
    Transform stateTransform;

    private void Awake()
    {
        branchFinder = BranchFinder.GenerateFinder(birdData.DistanceToFind,birdData.BranchMask,birdData.FindBranch); // new InFrontBranchFinder(distanceToFind,branchMask);
        speed = birdData.Speed;
        myState = BirdState.InAir;
    }
    void Update()
    {
        switch (myState)
        {
            case BirdState.Angry:
                EscapeFromObject();
                break;
            case BirdState.Following:
                FollowObject();
                break;
            case BirdState.InAir:
                TryFindZoneFromABranch();
                break;
            case BirdState.MovingToZone:
                MoveToStateTransform();
                break;
        }

        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
    }
    private void LateUpdate()
    {
        transform.up = rb.velocity;
        if (rb.velocity.x > 0 && !fliped)
        {
            bodyTransform.transform.localScale =  (-Vector3.right + Vector3.up);
            fliped = true;
        }else if (rb.velocity.x < 0 && fliped)
        {
            bodyTransform.transform.localScale = Vector3.one;
            fliped = false;
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<FollowObject>(out FollowObject followObject))
        {
            Destroy(followObject.gameObject);
        }

    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        ArrivedObjetiveZone(collision);

        AngryObjectNear(collision);
        ToFollowObjectNear(collision);
    }
    void ArrivedObjetiveZone(Collider2D collision)
    {
        if (collision.TryGetComponent<Zone>(out Zone zone) && zone == zoneToMove)
        {
            if(zone.myBird !=null)
            {
                myState = BirdState.InAir;
                return;
            }

            zone.SetMyBird(this);
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            transform.SetPositionAndRotation(collision.transform.position, collision.transform.rotation);
            myState = BirdState.InZone;
        }
    }
     void AngryObjectNear(Collider2D collision)
    {
        if (collision.TryGetComponent<AngryObject>(out AngryObject angryObject))
        {
            stateTransform = angryObject.transform;
            stateTimer = birdData.AngryTime;
            myState = BirdState.Angry;
        }
    }
     void ToFollowObjectNear(Collider2D collision)
    {
        if (collision.TryGetComponent<FollowObject>(out FollowObject followObject))
        {
            stateTransform = followObject.transform;
            stateTimer = birdData.FollowTime;
            myState = BirdState.Following;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitArrivedZone(collision);
    }
    void ExitArrivedZone(Collider2D collision)
    {
        if (collision.TryGetComponent<Zone>(out Zone zone) && zone == zoneToMove)
        {
            rb.gravityScale = 0.1f;
            zoneToMove = null;
            if (myState == BirdState.InZone) 
                myState = BirdState.InAir;
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

    void EscapeFromObject()
    {
        if (!CheckIfStateTransformExist())
            return;
        EscapeFromStateTransform();
        TryEndStateWithTimer();
    }

    void FollowObject()
    {
        if (!CheckIfStateTransformExist())
            return;
        MoveToStateTransform();
        TryEndStateWithTimer();
    }

    bool CheckIfStateTransformExist()
    {
        if (stateTransform != null)
            return true;

        myState = BirdState.InAir;
        return false;
    }
    void TryFindZoneFromABranch()
    {
        zoneToMove
            = branchFinder
            .TryFindBranch(transform)
            ?.TryGiveMeCloseZone(transform.position);
        if (zoneToMove != null)
        {
            myState = BirdState.Following;
            stateTransform = zoneToMove.transform;
        }
    }         
    void TryEndStateWithTimer()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            stateTransform = null;
            myState = BirdState.InAir;
        }
    }
    void EscapeFromStateTransform()
        => rb.velocity = (transform.position - stateTransform.position).normalized * speed;
    void MoveToStateTransform() 
        => rb.velocity = (stateTransform.position - transform.position).normalized * speed;
}

public enum BirdState
{
    InZone,
    InAir,
    MovingToZone,
    Angry,
    Following
}