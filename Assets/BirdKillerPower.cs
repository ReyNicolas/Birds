using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdKillerPower : MonoBehaviour
{
    bool used;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bird bird) && !used)
        {
            used = true;
            KillBird(bird);
            Destroy(gameObject);
        }
    }

    private void KillBird(Bird bird)
    {
        Destroy(bird.gameObject);
    }
}
