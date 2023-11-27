using System;
using System.Linq;
using UnityEngine;

public class BirdColorChangerPower : Power
{
    bool used;
    [SerializeField] float radius; //TODO: change this logic with a singleton *1
    [SerializeField] int maxToChange; 
    [SerializeField] LayerMask birdMask; //TODO: change this logic with a singleton *1 or matchData
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerFruit playerFruit) && !used)
        {
            used = true;
            ChangeRandomBirdToFruitColor(playerFruit);
            Destroy(gameObject);
        }
    }

    private void ChangeRandomBirdToFruitColor(PlayerFruit playerFruit) //TODO: change this logic with a singleton *1
    {
        Color playerColor = playerFruit.GetColor();
        Physics2D
            .OverlapCircleAll(transform.position, radius, birdMask)
                .Select(collider2d => collider2d.GetComponent<Bird>())
                .Where(bird => bird.GetColor() != playerColor)
                .Take(maxToChange)
                .ToList()
                .ForEach(bird=> bird.SetColor(playerColor)); //TODO Whats happen when change the color from brach complete with all birds same color
    }

    
}


public abstract class Power: MonoBehaviour
{
    public static event Action<Power> OnNewPower;
    public static event Action<Power> OnDestroyPower;

    protected virtual void Awake()
    {
        OnNewPower?.Invoke(this);
    }

    protected virtual void OnDestroy()
    {
        OnDestroyPower?.Invoke(this);
    }
}