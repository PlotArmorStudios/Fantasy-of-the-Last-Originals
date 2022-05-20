public class AirborneAttackCrossFade : AttackingCrossFade
{
    protected AutoTargetEnemy _autoTargeter;

    public AirborneAttackCrossFade(FiniteStateMachine stateMachine) : base(stateMachine)
    {
        _autoTargeter = stateMachine.GetComponent<AutoTargetEnemy>();
    }
}