using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Transform bodyTransform;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] protected Zone zoneToMove;
    [SerializeField] BirdSO birdData;
    [SerializeField] Animator animator;
    [SerializeField] protected BirdState myState;
    int speed;
    protected BranchFinder branchFinder;
    float stateTimer;
    protected Transform stateTransform;

    protected virtual void Awake()
    {
        branchFinder = BranchFinder.GenerateFinder(birdData.DistanceToFind,birdData.BranchMask,birdData.FindBranch); // new InFrontBranchFinder(distanceToFind,branchMask);
        speed = birdData.Speed;
        myState = BirdState.InAir;
    }
    void Update()
    {
        switch (myState)
        {
            case BirdState.Escaping:
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
        transform.right = rb.velocity;
        bodyTransform.localScale 
            = (transform.up.y >= 0)
                ? Vector2.up + Vector2.right 
                : -Vector2.up +Vector2.right;

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

    public void GetToFollowObject(Transform transformToFollow)
    {
        stateTransform = transformToFollow;
        stateTimer = birdData.FollowTime;
        myState = BirdState.Following;
    }
    public void GetToEscapeObject(Transform transformToEscape)
    {
        stateTransform = transformToEscape;
        stateTimer = birdData.FollowTime;
        myState = BirdState.Escaping;
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
    protected virtual void TryFindZoneFromABranch()
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
    protected virtual void MoveToStateTransform() 
        => rb.velocity = (stateTransform.position - transform.position).normalized * speed;

    
}

public enum BirdState
{
    InZone,
    InAir,
    MovingToZone,
    Escaping,
    Following
}