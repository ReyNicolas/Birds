using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerSO playerData;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject fruitPrefab;
    [SerializeField] int force;
    [SerializeField] int speed;
    [SerializeField] int shootCooldown;
    [SerializeField] Transform aimTransform;
    [SerializeField] Vector2 aimDirection;
    [SerializeField] Vector2 moveDirection;
    [SerializeField] PlayerInput playerInput;
    float waitTimer;
    public ReactiveProperty<float> shootTimer = new ReactiveProperty<float>(0);

    private void Awake()
    {
        shootTimer.Value = shootCooldown;
    }

    public void Initialize(PlayerSO playerData)
    {
        this.playerData = playerData;
        spriteRenderer.color = playerData.PlayerColor;
    }

    private void Update()
    {
        waitTimer -= Time.deltaTime;
        shootTimer.Value -= Time.deltaTime;
        if (playerInput.actions["Aim"].WasReleasedThisFrame() && shootTimer.Value <0)
        {
            if (aimDirection == Vector2.zero) return;
            ShootFruit();
            SetCooldown();
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

    void SetCooldown()
    {
        waitTimer = 0.3f;
        shootTimer.Value = shootCooldown;
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
