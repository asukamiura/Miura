using Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public PlayerCore playerCore;
    public ITutorialTask currentTask; // 現在のタスク
    public int attackNormalCount = 0;      // 通常攻撃をした回数
    public int attackSpecial1Count = 0;    // 特殊攻撃1をした回数
    public int attackSpecial2Count = 0;    // 特殊攻撃2をした回数
    public int attackUltimateCount = 0;

    [Header("説明画面")]
    public GameObject attackNormalPanel;
    public GameObject justDodgePanel;
    public GameObject justGaurdPanel;
    public GameObject justPointPanel;
    public GameObject attackUltimatePanel;

    [Header("タスクUI")]
    public GameObject attackNormalTaskUI;
    public GameObject justDodgeTaskUI;
    public GameObject justGaurdTaskUI;
    public GameObject attackUltimateTaskUI;

    [Header("進捗カウントテキスト")]
    [SerializeField] private TextMeshProUGUI attackNormalCountText;
    [SerializeField] private TextMeshProUGUI justDodgeCountText;
    [SerializeField] private TextMeshProUGUI attackSpecial1CountText;
    [SerializeField] private TextMeshProUGUI justGaurdCountText;
    [SerializeField] private TextMeshProUGUI attackSpecial2CountText;
    [SerializeField] private TextMeshProUGUI attackUltimateCountText;

    [Header("成功UI")]
    [SerializeField] private GameObject completeUI;

    public InputReciver Input => InputReciver.Instance;
    private List<ITutorialTask> tutorialTask; // タスクリスト
    private bool taskExecuted = false;
    private float completeUIDisplayLatency = 1;
    private bool inTutorial = true;
    private float transitionTime = 2;

    private void Awake()
    {
        tutorialTask = new List<ITutorialTask>()
        {
            new AttackNormalTask(this),
            new JustDodgeTask(this),
            new JustGaurdTask(this),
            new JustPointTask(this),
            new AttackUltimateTask(this),
        };
    }

    private void Start()
    {
        attackNormalPanel.SetActive(false);
        justDodgePanel.SetActive(false);
        justGaurdPanel.SetActive(false);
        justPointPanel.SetActive(false);
        attackUltimatePanel.SetActive(false);

        attackNormalTaskUI.SetActive(false);
        justDodgeTaskUI.SetActive(false);
        justGaurdTaskUI.SetActive(false);
        attackUltimateTaskUI.SetActive(false);

        SetFirstTask(tutorialTask.First());
    }

    private void Update()
    {
        if (inTutorial)
        {
            currentTask.Update();

            if (currentTask != null && !taskExecuted)
            {
                if (currentTask.CheckTask())
                {
                    taskExecuted = true;

                    tutorialTask.RemoveAt(0);

                    var nextTask = tutorialTask.FirstOrDefault();
                    StartCoroutine(SetNextTask(nextTask, transitionTime));
                }
            }

            // 進捗カウントの更新
            attackNormalCountText.text = attackNormalCount.ToString();
            justDodgeCountText.text = playerCore.justDodgeCount.ToString();
            attackSpecial1CountText.text = attackSpecial1Count.ToString();
            justGaurdCountText.text = playerCore.justGaurdCount.ToString();
            attackSpecial2CountText.text = attackSpecial2Count.ToString();
            attackUltimateCountText.text = attackUltimateCount.ToString();
        }
        else
        {
            // セレクトシーンに遷移
            SceneManager.LoadScene("SelectScene");
        }
    }

    /// <summary>
    /// 最初のタスクを設定
    /// </summary>
    /// <param name="task">最初のタスク</param>
    private void SetFirstTask(ITutorialTask task)
    {
        currentTask = task;
        currentTask.Enter();
    }

    /// <summary>
    /// 次のタスクを設定
    /// </summary>
    /// <param name="task">次のタスク</param>
    /// <param name="waitTime">次のタスクを設定するまでの待機時間</param>
    /// <returns></returns>
    private IEnumerator SetNextTask(ITutorialTask task, float waitTime)
    {
        if (currentTask.ShowSuccessUI)
        {
            yield return new WaitForSeconds(completeUIDisplayLatency);
            completeUI.SetActive(true);
            yield return new WaitForSeconds(completeUIDisplayLatency);
            completeUI.SetActive(false);
        }

        currentTask.Exit();

        // タスクがない場合、チュートリアル終了
        if (tutorialTask.Count <= 0)
        {
            inTutorial = false;
            yield break;
        }

        yield return new WaitForSecondsRealtime(waitTime);

        currentTask = task;
        currentTask.Enter();

        taskExecuted = false;
    }
}
