using System;
using UnityEngine;

public class Zone: MonoBehaviour
{
    public Bird myBird = null;
    public event Action<Bird> OnBirdEnter;
    [SerializeField] SpriteRenderer spriteRenderer;
    Color originalColor;

    private void Awake()
    {
        originalColor = spriteRenderer.color;
    }
    public void SetMyBird(Bird aBird)
    {
        myBird = aBird;
        spriteRenderer.color = myBird.GetColor();
        OnBirdEnter?.Invoke(myBird);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Bird>(out Bird bird) && myBird == bird)
        {
            myBird = null;
        }
        if(myBird == null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}