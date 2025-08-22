public class GameOverModel
{
    public enum GameOverState { ReturnSelect, Retry }
    GameOverState currentState;

    public GameOverState CurrentState => currentState;

    public void ChangeStateRight()
    {
        if (currentState < GameOverState.Retry)
        {
            currentState++;
        }
    }

    public void ChangeStateLeft()
    {
        if (currentState > GameOverState.ReturnSelect)
        {
            currentState--;
        }
    }

    public void SetInitialState()
    {
        currentState = GameOverState.Retry;
    }
}
