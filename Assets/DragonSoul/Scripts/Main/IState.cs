public interface IState<TStateID>
{
    TStateID StateID { get; }
    public void Enter();
    public void StateUpdate();
    public void StateFixedUpdate();
    public void Exit();
}
