using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerSO playerData;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject fruitPrefab;
    [SerializeField] int force;
    [SerializeField] int speed;
    [SerializeField] Transform aimTransform;
    [SerializeField] Vector2 aimDirection;
    [SerializeField] Vector2 moveDirection;
    [SerializeField] PlayerInput playerInput;
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
        if (playerInput.actions["Aim"].WasReleasedThisFrame())
        {
            waitTimer = 0.3f;
            ShootFruit();
            return;
        }
        if (playerInput.actions["Aim"].IsPressed())
        {
            Aim();
            return;
        }

        if (waitTimer<0)
            Move();
    }

    void Move()
    {
        moveDirection = playerInput.actions["Move"].ReadValue<Vector2>();
        moveDirection = moveDirection.normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    void Aim()
    {
        aimDirection = playerInput.actions["Move"].ReadValue<Vector2>();
        aimDirection = aimDirection.normalized;
        aimTransform.localPosition = aimDirection;
        aimTransform.up = aimDirection;
    }

    void ShootFruit()
        => Instantiate(fruitPrefab, transform.position, Quaternion.identity)
                        .GetComponent<PlayerFruit>()
                            .Initialize(playerData.PlayerColor, aimDirection * force);
  

}
