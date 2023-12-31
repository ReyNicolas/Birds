using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxChangeHeighLoop : MonoBehaviour
{
    [SerializeField] float maxRate;
    [SerializeField] float ratePerSecond;
    float ymin, ymax;

    void Start()
    {
        ymin = 1 - maxRate;
        ymax = 1 + maxRate;
    }

    void Update()
    {
        transform.localScale += Vector3.up * ratePerSecond * Time.deltaTime;

        if (transform.localScale.y >= ymax || transform.localScale.y <= ymin)
            ratePerSecond *= -1;
    }
}
