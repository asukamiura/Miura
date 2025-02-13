using Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] CinemachineBrain cinemachineBrain;
    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] CinemachineImpulseSource impulseSource;

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

    void Start()
    {
        pov = playerCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    void Update()
    {
        if (isInput)
        {
            pov.m_HorizontalAxis.Value += Input.Look.x * horizontalSpeed * sensitivity * Time.deltaTime;
            pov.m_VerticalAxis.Value -= Input.Look.y * verticalSpeed * sensitivity * Time.deltaTime;
        }

        if (isChangeFOV)
        {
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, targetFOV, changeFOVSpeed * Time.deltaTime);

            if (Mathf.Abs(targetFOV - playerCamera.m_Lens.FieldOfView) < 0.1f)
            {
                playerCamera.m_Lens.FieldOfView = targetFOV;
                isChangeFOV = false;
            }
        }

        if (isChangeDutch)
        {
            playerCamera.m_Lens.Dutch = Mathf.Lerp(playerCamera.m_Lens.Dutch, targetDutch, changeDutchSpeed * Time.deltaTime);

            if (Mathf.Abs(targetDutch - playerCamera.m_Lens.Dutch) < 0.1f)
            {
                playerCamera.m_Lens.Dutch = targetDutch;
                isChangeDutch = false;
            }
        }

    }

    /// <summary>
    /// 目標視野角、広げる速度を設定、視野角を広げ始める
    /// </summary>
    /// <param name="newTargetFOV">目標視野角</param>
    /// <param name="newSpreadSpeed">広げる速度</param>
    public void StartChangeFOV(float newTargetFOV, float newSpreadSpeed)
    {
        targetFOV = newTargetFOV;
        changeFOVSpeed = newSpreadSpeed;
        isChangeFOV = true;
    }

    public void StartChangeDutch(float newTargetDutch, float newChangeDutchSpeed)
    {
        targetDutch = newTargetDutch;
        changeDutchSpeed = newChangeDutchSpeed;
        isChangeDutch = true;
    }

    /// <summary>
    /// カメラを揺らす
    /// </summary>
    public void ApplyImpulse()
    {
        impulseSource.GenerateImpulse();
    }

    public void ChangeDutch(float dutchVal)
    {
        playerCamera.m_Lens.Dutch = dutchVal;
    }


    public void RecenteringEnabled()
    {
        pov.m_VerticalRecentering.m_enabled = true;
        pov.m_HorizontalRecentering.m_enabled = true;

        pov.m_VerticalRecentering.m_RecenteringTime = RecenteringTime;
        pov.m_HorizontalRecentering.m_RecenteringTime = RecenteringTime;
    }

    public void RecenteringDisabled()
    {
        pov.m_VerticalRecentering.m_enabled = false;
        pov.m_HorizontalRecentering.m_enabled = false;
    }
}
