using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BranchFinder
{
    protected int distanceToFind;
    protected LayerMask branchMask;

    public BranchFinder(int distanceToFind, LayerMask branchMask)
    {
        this.distanceToFind = distanceToFind;
        this.branchMask = branchMask;
    }

    public static BranchFinder GenerateFinder(int distanceToFind, LayerMask branchMask, FindBranchType findBranch)
    {
        switch (findBranch)
        {
            case FindBranchType.InFront:
                return new InFrontBranchFinder(distanceToFind,branchMask);
            case FindBranchType.Closest:
                return new ClosestBranchFinder(distanceToFind, branchMask);
            default:
                return null;
        }
    }

    public abstract Branch TryFindBranch(Transform birdTransform);



}

public class ClosestBranchFinder : BranchFinder
{
    public ClosestBranchFinder(int distanceToFind, LayerMask branchMask) : base(distanceToFind, branchMask)
    {

    }
    public override Branch TryFindBranch(Transform birdTransform)
    {
        var myPosition = birdTransform.position;
        var colliders = Physics2D.OverlapCircleAll(myPosition, distanceToFind, branchMask);

        return
              (colliders.Length > 0)
              ? colliders
                  .Select(branchCollider => branchCollider.transform)
                  .OrderBy(branchTransform => Vector2.Distance(branchTransform.position, myPosition))
                  .First().GetComponent<Branch>()
              : null;
    }
}

public class InFrontBranchFinder: BranchFinder
{
    public InFrontBranchFinder(int distanceToFind, LayerMask branchMask) : base(distanceToFind, branchMask)
    {

    }

    public override Branch TryFindBranch(Transform birdTransform)
    {
        RaycastHit2D hit = Physics2D.Raycast(birdTransform.position, birdTransform.up, distanceToFind, branchMask);
        return (hit)
            ? Physics2D.OverlapCircle(hit.point, 0.1f, branchMask).GetComponent<Branch>()
            : null;
    }
}

public enum FindBranchType
{
    InFront,
    Closest
}