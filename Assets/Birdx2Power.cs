using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birdx2Power : MonoBehaviour
{
    bool used;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bird bird) && !used)
        {
            used = true;
            GenerateClone(bird);
            Destroy(gameObject);
        }
    }

    private void GenerateClone(Bird bird)
    {
        Transform originalTransform = bird.transform;
        Instantiate(bird, transform.position, originalTransform.rotation);
    }
}
