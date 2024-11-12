public interface IState
{
    public PlayerStateID StateID { get; }
    public void Enter();
    public void Execute();
    public void Exit();
}
