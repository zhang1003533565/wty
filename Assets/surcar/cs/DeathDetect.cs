using UnityEngine;

public class DeathDetect : MonoBehaviour
{
    // 引用 PlayerState 单例
    private PlayerState playerState;

    // 死亡音效
    public AudioClip deathSound;
    public AudioSource audioSource;

    // 上一次的存活状态
    private bool wasAlive;

    private void Start()
    {
        // 获取 PlayerState 单例
        playerState = PlayerState.Instance;

        // 初始化 AudioSource
        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.clip = deathSound;

        // 初始化存活状态
        wasAlive = !playerState.IsDead;
    }

    private void Update()
    {
        // 检测玩家是否从存活状态变为死亡状态
        if (wasAlive && playerState.IsDead)
        {
            PlayDeathSound();
        }

        // 更新上一次的存活状态
        wasAlive = !playerState.IsDead;
    }

    // 播放死亡音效（单次播放）
    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            // 使用 PlayOneShot 播放音效，确保只播放一次
            audioSource.PlayOneShot(deathSound);
        }
    }
}
