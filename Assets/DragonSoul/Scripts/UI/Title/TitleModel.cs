public class TitleModel
{
    public enum TitleState { Start, Option, Quit }
    TitleState currentState = TitleState.Start;

    public TitleState CurrentState => currentState;

    public void ChangeStateDown()
    {
        if (currentState < TitleState.Quit)
        {
            currentState++;
        }
    }

    public void ChangeStateUp()
    {
        if (currentState > TitleState.Start)
        {
            currentState--;
        }
    }

    public void SetInitialState()
    {
        currentState = TitleState.Start;
    }
}
