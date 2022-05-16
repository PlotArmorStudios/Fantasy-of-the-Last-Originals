using Photon.Pun;

public abstract class FiniteStateMachine : MonoBehaviourPunCallbacks
{
    protected StateMachine _stateMachine;
    protected FiniteStateMachine Instance;
    public bool Stun { get; set; }
    public bool Launch { get; set; }
    
    protected virtual void Start()
    {
        InitializeStates();
        AddStateTransitions();
        
        //Set default state
        //_stateMachine.SetState();
    }

    protected abstract void InitializeStates();

    protected abstract void AddStateTransitions();

    protected abstract void Update();
}