using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private CinemachinePOV pov;
    private float horizontalSpeed = 2;
    private float verticalSpeed = 1;
    private float sensitivity = 50;
    private bool isInput = true;
    private Vector3 defaultCameraPos;
    private bool isSpread = false;
    private bool isNarrow = false;
    private float spreadSpeed = 1;
    private float narrowSpeed = 1;
    private float targetFOV = 50;

    private InputReciver Input => InputReciver.Instance;

    private void Start()
    {
        pov = playerCamera.GetCinemachineComponent<CinemachinePOV>();

        defaultCameraPos = playerCamera.transform.position;
    }

    private void Update()
    {
        if (isInput)
        {
            pov.m_HorizontalAxis.Value += Input.Look.x * horizontalSpeed * sensitivity * Time.deltaTime;
            pov.m_VerticalAxis.Value += Input.Look.y * verticalSpeed * sensitivity * Time.deltaTime;
        }

        if (isSpread)
        {
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, targetFOV, spreadSpeed * Time.deltaTime);
        }

        if (isNarrow)
        {
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, targetFOV, narrowSpeed * Time.deltaTime);
        }

    }

    private IEnumerator ActiveInput(float delay)
    {
        yield return new WaitForSeconds(delay);

        isInput = true;
    }

    /// <summary>
    /// 視野角を広げる
    /// </summary>
    /// <param name="targetFOV">目標視野角</param>
    /// <param name="spreadSpeed">広げる速度</param>
    private void SpreadFOV(float targetFOV, float spreadSpeed)
    {      
        playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, targetFOV, spreadSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 視野角を狭める
    /// </summary>
    /// <param name="targetFOV">目標視野角</param>
    /// <param name="narrowSpeed">狭める速度</param>
    private void NarrowFOV(float targetFOV, float narrowSpeed)
    {        
        playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, targetFOV, narrowSpeed * Time.deltaTime);
    }

    public void StartSpreadFOV(float newTargetFOV, float newSpreadSpeed)
    {
        isNarrow = false;
        isSpread = true;
        targetFOV = newTargetFOV;
        spreadSpeed = newSpreadSpeed;
    }

    public void StartNarrowFOV(float newTargetFOV, float newNarrowSpeed)
    {
        isSpread = false;
        isNarrow = true;
        targetFOV = newTargetFOV;
        narrowSpeed = newNarrowSpeed;
    }

    public void ApplyImpulse()
    {
        impulseSource.GenerateImpulse();
    }
}
