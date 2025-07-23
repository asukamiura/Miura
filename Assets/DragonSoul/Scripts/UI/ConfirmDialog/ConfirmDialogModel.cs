public class ConfirmDialogModel
{
    public enum ConfirmState { Yes=0, No }
    ConfirmState currentState;

    public ConfirmDialogModel(ConfirmState intialState = ConfirmState.No)
    {
        currentState = intialState;
    }

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

    public void SetState()
    {
        currentState = ConfirmState.No;
    }
}
