public class HitStopConfig
{
    public readonly float stopDuration;
    public readonly CameraShakeType cameraShakeType;

    public HitStopConfig(float stopDuration, CameraShakeType cameraShakeType)
    {
        this.stopDuration = stopDuration;
        this.cameraShakeType = cameraShakeType;
    }
}
