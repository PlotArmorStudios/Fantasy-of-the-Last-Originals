public class AirborneAttackCrossFade : IState
{
    private PlayerStateMachineCrossFade _stateMachine;
    
    public AirborneAttackCrossFade(FiniteStateMachine stateMachine)
    {
        _stateMachine = (PlayerStateMachineCrossFade) stateMachine;
    }
    
    public void Tick()
    {
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }
}