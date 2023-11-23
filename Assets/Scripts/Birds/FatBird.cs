using UnityEngine;

public class FatBird: Bird
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.TryGetComponent<Zone>(out Zone zone))
            zone.SetMyBird(this);
    }
}
