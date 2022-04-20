using UnityEngine;

public class CurrentAnimatorState : MonoBehaviour
{
    public int AttackToTransitionTo { get; set; }
    public bool LoopToAttack { get; set; }
    public int AttackToLoopTo { get; set; }
}