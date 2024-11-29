public interface IState<TStateID>
{
    TStateID StateID { get; }
    public void Enter();
    public void Update();
    public void FixedUpdate();
    public void Exit();
}
