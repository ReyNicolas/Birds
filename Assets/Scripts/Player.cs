using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerSO playerData;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] List<GameObject> objectsPrefabs;
    [SerializeField] int force;
    [SerializeField] int speed;
    [SerializeField] Transform aimTransform;
    [SerializeField] Vector2 aimDirection;
    [SerializeField] Vector2 moveDirection;
    float waitTimer;

    private void Awake()
    {
        Initialize(playerData); // just to test GameManger should initialize       
    }

    public void Initialize(PlayerSO playerData)
    {
        this.playerData = playerData;
        spriteRenderer.color = playerData.PlayerColor;
    }

    private void Update()
    {
        waitTimer -= Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            waitTimer = 0.3f;
            ShootObject();
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Aim();
            return;
        }

        if (waitTimer<0)
            Move();
    }

    void Move()
    {
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");
        moveDirection = moveDirection.normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void Aim()
    {
        aimDirection.x = Input.GetAxis("Horizontal");
        aimDirection.y = Input.GetAxis("Vertical");
        aimDirection = aimDirection.normalized;
        aimTransform.localPosition = aimDirection;
        aimTransform.up = aimDirection;
    }

    void ShootObject() 
        => Instantiate(GetRandomObjectPrefab(), transform.position, Quaternion.identity)
                        .GetComponent<Rigidbody2D>().velocity = aimDirection * force;

    GameObject GetRandomObjectPrefab()
    {
        return objectsPrefabs[Random.Range(0, objectsPrefabs.Count)];
    }
}
