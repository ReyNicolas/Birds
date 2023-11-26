using System;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public event Action OnDeadZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        OnDeadZone?.Invoke();
    }
}
