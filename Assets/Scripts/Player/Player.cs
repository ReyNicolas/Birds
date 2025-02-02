using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerSO playerData;
    [Header("References")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerFruit fruitPrefab;
    [Header("Movement info")]
    [SerializeField] int speed;

    [Header("Dash info")]
    [SerializeField] int dashCooldown;
    [SerializeField] float dashDuration;
    [SerializeField] int dashForce;
    public ReactiveProperty<float> dashTimer = new ReactiveProperty<float>(0);
    bool isDashing;
    WaitForSeconds waitForSeconds;


    private void Awake()
    {
        dashTimer.Value = dashCooldown;
        waitForSeconds = new WaitForSeconds(dashDuration);
        fruitPrefab.gameObject.SetActive(false);
    }

    public void Initialize(PlayerSO playerData)
    {
        this.playerData = playerData;
        spriteRenderer.color = playerData.PlayerColor;
    }

    private void Update()
    {
        if (!isDashing) { 
            dashTimer.Value -= Time.deltaTime;
            Move();
            TryDash();
        }
        else
        {

        }
        
    }
    public void ShootFruit()
        => Instantiate(fruitPrefab, transform.position, Quaternion.identity)
                            .Initialize(playerData.PlayerColor, rigidbody2D.velocity);


    private void TryDash()
    {
        if(playerInput.actions["Dash"].WasReleasedThisFrame() && dashTimer.Value < 0)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        rigidbody2D.velocity = rigidbody2D.velocity.normalized * dashForce;
        fruitPrefab.gameObject.SetActive(true);

        yield return waitForSeconds;

        isDashing = false;
        fruitPrefab.gameObject.SetActive(false);
        dashTimer.Value = dashCooldown;
    }

    void Move()
    {
        rigidbody2D.velocity = Vector2.ClampMagnitude(playerInput.actions["Move"].ReadValue<Vector2>(), 1) * speed;
    } 
    
}