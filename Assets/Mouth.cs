using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    [SerializeField] Bird myBird;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerFruit playerFruit) && playerFruit.GetColor() == myBird.GetColor())
            Destroy(playerFruit.gameObject);        
    }
}
