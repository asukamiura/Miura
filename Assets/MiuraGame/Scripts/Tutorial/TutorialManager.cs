using Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public PlayerCore playerCore;
    public ITutorialTask currentTask; // 現在のタスク
    public int attackNormalCount = 0;      // 通常攻撃をした回数
    public int justDodgeCount = 0;          // ジャスト回避回数          
    public int attackSpecial1Count = 0;    // 特殊攻撃1をした回数
    public int justGuardCount = 0;          // ジャストガード回数
    public int attackSpecial2Count = 0;    // 特殊攻撃2をした回数
    public int attackUltimateCount = 0;

    [Header("説明画面")]
    public GameObject attackNormalPanel;
    public GameObject justDodgePanel;
    public GameObject justGuardPanel;
    public GameObject justPointPanel;
    public GameObject attackUltimatePanel;

    [Header("タスクUI")]
    public GameObject attackNormalTaskUI;
    public GameObject justDodgeTaskUI;
    public GameObject justGuardTaskUI;
    public GameObject attackUltimateTaskUI;

    [Header("進捗カウントテキスト")]
    [SerializeField] TextMeshProUGUI attackNormalCountText;
    [SerializeField] TextMeshProUGUI justDodgeCountText;
    [SerializeField] TextMeshProUGUI attackSpecial1CountText;
    [SerializeField] TextMeshProUGUI justGuardCountText;
    [SerializeField] TextMeshProUGUI attackSpecial2CountText;
    [SerializeField] TextMeshProUGUI attackUltimateCountText;

    [Header("成功UI")]
    [SerializeField] GameObject completeUI;

    public InputReciver Input => InputReciver.Instance;
    List<ITutorialTask> tutorialTask; // タスクリスト
    bool taskExecuted = false;
    bool inTutorial = true;     // チュートリアル中かどうか
    bool isChangedScene = false;     // シーン遷移が実行されたかどうか

    const float CompleteUIDisplayLatency = 1;   // 完了UI表示時間
    const float FirstWaitTime = 3;  // 最初のタスク表示までの待機時間

    void Awake()
    {
        tutorialTask = new List<ITutorialTask>()
        {
            new AttackNormalTask(this),
            new JustDodgeTask(this),
            new JustGuardTask(this),
            new JustPointTask(this),
            new AttackUltimateTask(this),
        };
    }

    void Start()
    {
        attackNormalPanel.SetActive(false);
        justDodgePanel.SetActive(false);
        justGuardPanel.SetActive(false);
        justPointPanel.SetActive(false);
        attackUltimatePanel.SetActive(false);

        attackNormalTaskUI.SetActive(false);
        justDodgeTaskUI.SetActive(false);
        justGuardTaskUI.SetActive(false);
        attackUltimateTaskUI.SetActive(false);
        StartCoroutine(SetFirstTask(tutorialTask.First(), FirstWaitTime));
    }

    void Update()
    {
        if (inTutorial)
        {
            if (currentTask != null && !taskExecuted)
            {
                currentTask.Update();

                // 現在のタスクを完了したら次のタスクへ
                if (currentTask.CheckTask())
                {
                    taskExecuted = true;

                    tutorialTask.RemoveAt(0);

                    var nextTask = tutorialTask.FirstOrDefault();
                    StartCoroutine(SetNextTask(nextTask, currentTask.TransitionTime()));
                }
            }

            // 進捗カウントの更新
            attackNormalCountText.text = attackNormalCount.ToString();
            justDodgeCountText.text = justDodgeCount.ToString();
            attackSpecial1CountText.text = attackSpecial1Count.ToString();
            justGuardCountText.text = justGuardCount.ToString();
            attackSpecial2CountText.text = attackSpecial2Count.ToString();
            attackUltimateCountText.text = attackUltimateCount.ToString();
        }
        else if (!inTutorial && !isChangedScene)
        {
            isChangedScene = true;
            FadeManager.Instance.LoadScene("SelectScene");
        }
    }

    /// <summary>
    /// 最初のタスクを設定
    /// </summary>
    /// <param name="task">最初のタスク</param>
    /// <param name="waitTime">最初のタスクを表示するまでの待機時間</param>
    /// <returns></returns>
    IEnumerator SetFirstTask(ITutorialTask task, float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        currentTask = task;
        currentTask.Enter();
    }

    /// <summary>
    /// 次のタスクを設定
    /// </summary>
    /// <param name="task">次のタスク</param>
    /// <param name="waitTime">次のタスクを設定するまでの待機時間</param>
    /// <returns></returns>
    IEnumerator SetNextTask(ITutorialTask task, float waitTime)
    {
        if (currentTask.ShowSuccessUI)
        {
            yield return new WaitForSeconds(CompleteUIDisplayLatency);
            completeUI.SetActive(true);
            yield return new WaitForSeconds(CompleteUIDisplayLatency);
            completeUI.SetActive(false);
        }

        currentTask.Exit();

        yield return new WaitForSecondsRealtime(waitTime);

        // タスクがない場合、チュートリアル終了
        if (tutorialTask.Count <= 0)
        {
            inTutorial = false;
            yield break;
        }

        currentTask = task;
        currentTask.Enter();

        taskExecuted = false;
    }
}
