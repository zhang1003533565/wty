using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractable : MonoBehaviour, IInteractable
{
    public string playerName = "Player";

    private PlayerState playerState;

    // 音频源组件，用于播放受伤声音
    public AudioSource audioSource;
    public AudioClip damageClip;

    // UI特效的Animator组件
    public Animator damageEffectAnimator;

    // 动画触发参数名称
    public string effectTriggerName = "ShowDamage";

    private void Start()
    {
        playerState = PlayerState.Instance;

        // 获取或添加 AudioSource 组件
        //audioSource = GetComponent<AudioSource>();
        /*if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }*/

        // 确保 damageEffectAnimator 不为空
        if (damageEffectAnimator != null)
        {
            damageEffectAnimator.enabled = true; // 确保Animator启用
        }
    }

    // 实现 IInteractable 接口的方法
    public bool IsInteractable()
    {
        // 玩家是否可交互（例如：是否还活着）
        return false;
    }

    public bool IsAttackable()
    {
        // 玩家是否可被攻击（例如：是否还活着）
        return true;
    }

    public string GetName()
    {
        // 返回玩家的名字
        return playerName;
    }

    public void Interact()
    {
        // 交互逻辑
    }

    public void TakeDamage(string weaponName, int damage)
    {
        // 扣除玩家血量
        playerState.ModifyHealth(-damage);

        // 播放受伤声音
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(damageClip);
        }

        // 播放UI特效动画
        if (damageEffectAnimator != null)
        {
            damageEffectAnimator.SetTrigger(effectTriggerName);
        }
    }

    public bool isDeath()
    {
        // 返回玩家是否死亡
        return playerState.IsDead;
    }
}
