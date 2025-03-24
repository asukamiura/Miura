using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] CinemachineBrain cinemachineBrain;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] CinemachineVirtualCamera justDodgeCamera;

    CinemachinePOV pov;
    float horizontalSpeed = 2;
    float verticalSpeed = 1;
    float sensitivity = 50;
    bool isInput = true;
    bool isChangeFOV = false;
    bool isChangeDutch = false;
    float changeFOVSpeed;
    float targetFOV;
    float changeDutchSpeed;
    float targetDutch;

    const float RecenteringTime = 0.1f;

    InputReciver Input => InputReciver.Instance;

    //public static PlayerCameraController Instance { get; set; }

    void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    void Start()
    {
        pov = playerCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    void Update()
    {
        if (isInput)
        {
            // 視点を動かす処理
            pov.m_HorizontalAxis.Value += Input.Look.x * horizontalSpeed * sensitivity * Time.deltaTime;
            pov.m_VerticalAxis.Value -= Input.Look.y * verticalSpeed * sensitivity * Time.deltaTime;
        }

        if (isChangeFOV)
        {
            // 視野角を広げる処理
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, targetFOV, changeFOVSpeed * Time.deltaTime);

            // 目標視野角に到達したら処理を終了
            if (Mathf.Abs(targetFOV - playerCamera.m_Lens.FieldOfView) < 0.1f)
            {
                playerCamera.m_Lens.FieldOfView = targetFOV;
                isChangeFOV = false;
            }
        }

        if (isChangeDutch)
        {
            // カメラを傾ける処理
            playerCamera.m_Lens.Dutch = Mathf.Lerp(playerCamera.m_Lens.Dutch, targetDutch, changeDutchSpeed * Time.deltaTime);

            // 目標傾きに到達したら処理を終了
            if (Mathf.Abs(targetDutch - playerCamera.m_Lens.Dutch) < 0.1f)
            {
                playerCamera.m_Lens.Dutch = targetDutch;
                isChangeDutch = false;
            }
        }
    }

    /// <summary>
    /// 目標視野角、広げる速度を設定、視野角を広げ始める処理
    /// </summary>
    /// <param name="newTargetFOV">目標視野角</param>
    /// <param name="newSpreadSpeed">広げる速度</param>
    public void StartChangeFOV(float newTargetFOV, float newSpreadSpeed)
    {
        targetFOV = newTargetFOV;
        changeFOVSpeed = newSpreadSpeed;
        isChangeFOV = true;
    }

    /// <summary>
    /// カメラの目標傾き、傾ける速度を設定、傾け始める処理
    /// </summary>
    /// <param name="newTargetDutch">目標傾き</param>
    /// <param name="newChangeDutchSpeed">傾ける速度</param>
    public void StartChangeDutch(float newTargetDutch, float newChangeDutchSpeed)
    {
        targetDutch = newTargetDutch;
        changeDutchSpeed = newChangeDutchSpeed;
        isChangeDutch = true;
    }

    // カメラを揺らす処理
    public void ApplyImpulse()
    {
        impulseSource.GenerateImpulse();
    }

    public void ChangeDutch(float dutchVal)
    {
        playerCamera.m_Lens.Dutch = dutchVal;
    }

    // カメラの視点を中央に戻す処理開始
    public void RecenteringEnabled()
    {
        pov.m_VerticalRecentering.m_enabled = true;
        pov.m_HorizontalRecentering.m_enabled = true;

        pov.m_VerticalRecentering.m_RecenteringTime = RecenteringTime;
        pov.m_HorizontalRecentering.m_RecenteringTime = RecenteringTime;
    }

    // カメラの視点を中央に戻す処理を終了
    public void RecenteringDisabled()
    {
        pov.m_VerticalRecentering.m_enabled = false;
        pov.m_HorizontalRecentering.m_enabled = false;
    }

    public void ChangeJustDodgeCamera()
    {
        isInput = false;

        cinemachineBrain.m_DefaultBlend.m_Time = 0.5f;

        int priority = playerCamera.Priority;
        playerCamera.Priority = justDodgeCamera.Priority;
        justDodgeCamera.Priority = priority;
    }

    public void ChangePlayerCamera()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = 2f;

        int priority = justDodgeCamera.Priority;
        justDodgeCamera.Priority = playerCamera.Priority;
        playerCamera.Priority = priority;

        isInput = true;
    }
}
