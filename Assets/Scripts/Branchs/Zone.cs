using System;
using UnityEngine;

public class Zone: MonoBehaviour
{
    public Bird myBird = null;
    public event Action<Bird> OnBirdEnter;


    public void SetMyBird(Bird aBird)
    {
        myBird = aBird;
        OnBirdEnter?.Invoke(myBird);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Bird>(out Bird bird) && myBird == bird)
        {
            myBird = null;
        }
    }
}