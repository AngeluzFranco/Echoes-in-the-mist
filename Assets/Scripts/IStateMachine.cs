public interface IStateMachine
{
    public IState CurrentState { get; set; }
    void ChangeState(IState newState);
}
