using System;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public static event Action<Bird> OnNewBird;
    public static event Action<Bird> OnDestroyBird;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Transform bodyTransform;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] protected Zone zoneToMove;
    [SerializeField] BirdSO birdData;
    [SerializeField] Animator animator;
    [SerializeField] protected BirdState myState;
    int speed;
    protected BranchFinder branchFinder;
    protected float stateTimer;
    protected Transform stateTransform;

    protected virtual void Awake()
    {
        branchFinder = BranchFinder.GenerateFinder(birdData.DistanceToFind,birdData.BranchMask,birdData.FindBranch); // new InFrontBranchFinder(distanceToFind,branchMask);
        speed = birdData.Speed;
        myState = BirdState.InAir;
        OnNewBird?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnDestroyBird?.Invoke(this);
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
            case BirdState.MovingToZone:
                FollowObject();
                ArrivedObjetiveZone();
                break;
            case BirdState.InAir:
                TryFindZoneFromABranch();
                break;
        }

        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
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
    void ArrivedObjetiveZone()
    {        
        if((zoneToMove.myBird == null||zoneToMove.myBird == this) && Vector2.Distance(zoneToMove.transform.position, transform.position) < 0.1f)
        {
            zoneToMove.SetMyBird(this);
            rb.velocity = Vector3.zero;
            transform.SetPositionAndRotation(zoneToMove.transform.position, zoneToMove.transform.rotation);
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
    public virtual void GetToEscapeObject(Transform transformToEscape)
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
            myState = BirdState.MovingToZone;
            stateTransform = zoneToMove.transform;
            stateTimer = 1;
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
    MovingToZone,
    InAir,
    Escaping,
    Following
}