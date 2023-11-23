using System;
using UnityEngine;

public class Zone: MonoBehaviour
{
    public Bird myBird = null;
    public event Action<Bird> OnBirdEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(myBird == null && collision.TryGetComponent<Bird>(out Bird bird))
        {
            myBird = bird;
            OnBirdEnter?.Invoke(myBird);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Bird>(out Bird bird) && myBird == bird)
        {
            myBird = null;
        }
    }
}