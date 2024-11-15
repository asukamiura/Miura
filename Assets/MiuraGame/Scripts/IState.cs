public interface IState<TStateID>
{
    TStateID StateID { get; }
    public void Enter();
    public void Execute();
    public void Exit();
}
