using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineFreeLook playerCamera;
    [SerializeField] private CinemachineVirtualCamera playerCamera2;
    [SerializeField] private CinemachineVirtualCamera justGuardCamera;

    private Transform defaultPlayerCameraTransform;
    private float xAxisSpeed = 300;
    private float yAxisSpeed = 1.5f;
    private bool isInput = true;

    private const float effectTime = 0.5f;
    private const int enabledPriority = 20;
    private const int disabledPriority = 10;
    private const float justGuardBlendTime = 0.1f;
    private const float returnBlendTime = 0.5f;



    private InputReciver Input => InputReciver.Instance;

    private void Awake()
    {
        defaultPlayerCameraTransform = playerCamera.transform;        
    }

    private void Start()
    {
        playerCamera.m_XAxis.m_InputAxisName = "";
        playerCamera.m_YAxis.m_InputAxisName = "";
        SetCameraSensitivity(xAxisSpeed, yAxisSpeed);        
    }

    private void Update()
    {
        if (isInput)
        {
            playerCamera.m_XAxis.Value += Input.Look.x * playerCamera.m_XAxis.m_MaxSpeed * Time.deltaTime;
            playerCamera.m_YAxis.Value += Input.Look.y * playerCamera.m_YAxis.m_MaxSpeed * Time.deltaTime;
        }
    }

    private void SetCameraSensitivity(float xAxisSpeed, float yAxisSpeed)
    {
        playerCamera.m_XAxis.m_MaxSpeed = xAxisSpeed;
        playerCamera.m_YAxis.m_MaxSpeed = yAxisSpeed;
    }

    public void ChangeCameraPriority()
    {
        isInput = false;

        cinemachineBrain.m_DefaultBlend.m_Time = justGuardBlendTime;

        playerCamera.Priority = disabledPriority;

        justGuardCamera.Priority = enabledPriority;

        Transform justGuardCameraTransform = justGuardCamera.transform;
        // FreeLookカメラの軸を設定
        playerCamera.m_XAxis.Value = GetXAxis(justGuardCameraTransform.rotation.eulerAngles.y);
        playerCamera.m_XAxis.Value = GetYAxis(justGuardCameraTransform.position);
    }

    public void ResetCameraPriority()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = returnBlendTime;

        playerCamera.Priority = enabledPriority;

        justGuardCamera.Priority = disabledPriority;

        StartCoroutine(ActiveInput(effectTime));    
    }

    private float GetXAxis(float yRotation)
    {
        return Mathf.Repeat(yRotation, 360);
    }

    private float GetYAxis(Vector3 position)
    {
        float height = position.y - playerCamera.Follow.position.y;
        float totalHeight = playerCamera.m_Orbits[2].m_Height - playerCamera.m_Orbits[0].m_Height;
        return Mathf.Clamp01((height - playerCamera.m_Orbits[0].m_Height) / totalHeight);
    }

    private IEnumerator ActiveInput(float delay)
    {
        yield return new WaitForSeconds(delay);

        isInput = true;
    }
}
