using System;

public class StageSelectModel
{
    public enum SelectState { Tutorial = 0, Stage1, Stage2, Stage3 }
    SelectState currentState;

    public static SelectState inStageNum = 0;

    public StageSelectModel()
    {
        currentState = inStageNum;
    }

    public SelectState CurrentState => currentState;

    public void ChangeStateRight()
    {
        if (currentState < SelectState.Stage3)
        {
            currentState++;
        }
    }

    public void ChangeStateLeft()
    {
        if (currentState > SelectState.Tutorial)
        {
            currentState--;
        }
    }

    public string GetSceneName()
    {
        return currentState switch
        {
            SelectState.Tutorial => "TutorialScene",
            SelectState.Stage1 => "Stage1Scene",
            SelectState.Stage2 => "Stage2Scene",
            SelectState.Stage3 => "Stage3Scene",
            _ => ""
        };
    }

    public int GetStageCount()
    {
        return Enum.GetValues(typeof(SelectState)).Length;
    }
}
