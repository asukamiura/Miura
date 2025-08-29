public class ResultModel
{
    public enum ResultState { Select, Title }
    ResultState currentState = ResultState.Select;

    public ResultState CurrentState => currentState;

    public void ChangeStateDown()
    {
        if (currentState < ResultState.Title)
        {
            currentState++;
        }
    }

    public void ChangeStateUp()
    {
        if (currentState > ResultState.Select)
        {
            currentState--;
        }
    }
}
