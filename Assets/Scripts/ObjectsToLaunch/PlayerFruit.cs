using System;
using System.Linq;
using UnityEngine;

public class PlayerFruit: MonoBehaviour
{
    [SerializeField] LayerMask birdMask;
    [SerializeField] int radius;
    [SerializeField] Color color;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rb;
    float timer;

    public void Initialize(Color playerColor, Vector2 vectorForce)
    {
        spriteRenderer.color = playerColor;
        color = playerColor;
        rb.velocity = vectorForce;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            ActInCloseBirds();
            timer = 0.1f;
        }

    }

    void ActInCloseBirds()
    {
        Physics2D
            .OverlapCircleAll(transform.position, radius, birdMask)
            .ToList()
            .ForEach(
                collider =>
                {
                    if (collider.TryGetComponent(out Bird bird))
                    {
                        if (bird.GetColor() == color)
                        {
                            bird.GetToFollowObject(transform);
                            return;
                        }
                        bird.GetToEscapeObject(transform);
                    }

                });
    }
}