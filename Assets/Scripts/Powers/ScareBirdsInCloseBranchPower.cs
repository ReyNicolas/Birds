using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScareBirdsInCloseBranchPower : MonoBehaviour
{
    bool used;
    [SerializeField] float radius; //TODO: change this logic with a singleton *1
    [SerializeField] LayerMask branchMask; //TODO: change this logic with a singleton *1 or matchData
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerFruit playerFruit) && !used)
        {
            used = true;
            ScareBirdsInCloseBranch(playerFruit);
            Destroy(gameObject);
        }
    }

    private void ScareBirdsInCloseBranch(PlayerFruit playerFruit) //TODO: change this logic with a singleton *1
    {
        var branchTransform
             = Physics2D
                 .OverlapCircleAll(transform.position, radius, branchMask)
                     .OrderBy(Collider2d => Vector2.Distance(transform.position, Collider2d.transform.position))
                     .First()
                     .transform;

        branchTransform
            .GetComponent<Branch>()
            .GetBirdsInBranch()
            .ForEach(bird => bird.GetToEscapeObject(branchTransform));
    }



}
