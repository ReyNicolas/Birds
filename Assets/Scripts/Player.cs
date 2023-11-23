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
    [SerializeField]Vector2 aimDirection;
    [SerializeField]Vector2 moveDirection;

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
        if (Input.GetKey(KeyCode.Space))
        {
            aimDirection.x = Input.GetAxis("Horizontal");
            aimDirection.y = Input.GetAxis("Vertical");
            aimDirection = aimDirection.normalized;
            return;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Instantiate(GetRandomObjectPrefab(),transform.position,Quaternion.identity)
                .GetComponent<Rigidbody2D>().velocity = aimDirection * force;
        }

        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");
        moveDirection = moveDirection.normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }


    GameObject GetRandomObjectPrefab()
    {
        return objectsPrefabs[Random.Range(0, objectsPrefabs.Count)];
    }
}
