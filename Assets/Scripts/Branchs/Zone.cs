using UnityEngine;

public class Zone: MonoBehaviour
{
    public Bird myBird = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if(myBird == null && collision.TryGetComponent<Bird>(out Bird bird))
        {
            myBird = bird;
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