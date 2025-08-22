public class PauseMenuModel
{
    public enum PauseMenuState { ReturnSelect, Option, Close }
    PauseMenuState currentState = PauseMenuState.ReturnSelect;

    public PauseMenuState CurrentState => currentState;

    public void ChangeStateDown()
    {
        if (currentState < PauseMenuState.Close)
        {
            currentState++;
        }
    }

    public void ChangeStateUp()
    {
        if (currentState > PauseMenuState.ReturnSelect)
        {
            currentState--;
        }
    }

    public void SetInitialState()
    {
        currentState = PauseMenuState.ReturnSelect;
    }
}
