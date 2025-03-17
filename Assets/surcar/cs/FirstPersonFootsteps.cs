using UnityEngine;

public class FirstPersonFootsteps : MonoBehaviour
{
    public AudioSource footstepAudioSource; // 脚步声音的AudioSource
    public float footstepDelay = 0.5f; // 脚步声音的播放间隔
    public float minSpeedToPlay = 0.1f; // 触发脚步声音的最小速度

    private Vector3 previousPosition; // 上一帧的位置
    private float currentSpeed; // 当前速度
    private float nextFootstepTime;

    void Start()
    {
        previousPosition = transform.position; // 初始化上一帧的位置
        if (footstepAudioSource == null)
        {
            Debug.LogError("Footstep AudioSource is not assigned!");
        }
    }

    void Update()
    {
        // 计算当前速度
        currentSpeed = CalculateSpeed();

        // 检查速度是否大于最小速度
        if (currentSpeed > minSpeedToPlay)
        {
            // 根据速度调整脚步声音的播放频率
            float speedMultiplier = Mathf.Clamp(currentSpeed, 0.5f, 2f);
            float currentFootstepDelay = footstepDelay / speedMultiplier;

            // 播放脚步声音
            if (Time.time >= nextFootstepTime)
            {
                footstepAudioSource.Play();
                nextFootstepTime = Time.time + currentFootstepDelay;
            }
        }
    }

    // 计算速度的方法
    float CalculateSpeed()
    {
        // 获取当前位置
        Vector3 currentPosition = transform.position;

        // 计算位移
        float distanceMoved = Vector3.Distance(currentPosition, previousPosition);

        // 计算速度（位移 / 时间）
        float speed = distanceMoved / Time.deltaTime;

        // 更新上一帧的位置
        previousPosition = currentPosition;

        return speed;
    }
}
