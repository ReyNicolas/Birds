using UnityEngine;

public class Fruitsx3Power : Power
{
    bool used;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerFruit playerFruit) && !used )
        {
            used = true;
            Generate2Copies(playerFruit);
            Destroy(gameObject);
        }
    }

    private void Generate2Copies(PlayerFruit playerFruit)
    {
        Vector2 originalVelocity = playerFruit.GetComponent<Rigidbody2D>().velocity;
        Transform originalTransform = playerFruit.transform;

        Instantiate(playerFruit,originalTransform.position,originalTransform.rotation)
            .GetComponent<Rigidbody2D>()
                .velocity = originalVelocity.Rotate(30);
        Instantiate(playerFruit, originalTransform.position, originalTransform.rotation)
            .GetComponent<Rigidbody2D>()
                .velocity = originalVelocity.Rotate(-30);
    }
}

public static class VectorExtensions
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float radianes = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radianes);
        float sin = Mathf.Sin(radianes);
        float x = v.x * cos - v.y * sin;
        float y = v.x * sin + v.y * cos;
        return new Vector2(x, y);
    }
}
