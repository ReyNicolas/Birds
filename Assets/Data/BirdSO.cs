using UnityEngine;
using UniRx;

[CreateAssetMenu(fileName = "BirdData", menuName = "Bird Data")]
public class BirdSO : ScriptableObject
{
    public int AngryTime;
    public int FollowTime;
    public int DistanceToFind;
    public int Speed;
    public LayerMask BranchMask;
    public FindBranchType FindBranch;
}

