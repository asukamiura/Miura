public class ConfirmDialogModel
{
    public enum ConfirmState { Yes=0, No }
    ConfirmState currentState = ConfirmState.No;

    public ConfirmState CurrentState => currentState;

    public void ChangeStateRight()
    {
        if (currentState < ConfirmState.No)
        {
            currentState++;
        }
    }

    public void ChangeStateLeft()
    {
        if (currentState > ConfirmState.Yes)
        {
            currentState--;
        }
    }

    public void SetInitialState()
    {
        currentState = ConfirmState.No;
    }
}
