public class AirborneAttackCrossFade : AttackingCrossFade
{
    protected AutoTargetEnemy _autoTargeter;

    public AirborneAttackCrossFade(FiniteStateMachine stateMachine) : base(stateMachine)
    {
        _autoTargeter = stateMachine.GetComponent<AutoTargetEnemy>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _autoTargeter.GetComponent<RootMotionController>().enabled = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        _autoTargeter.GetComponent<RootMotionController>().enabled = true;
    }
}