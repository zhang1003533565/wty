using System.Collections;
using UnityEngine;

public class FistWeapon : MonoBehaviour, IWeapon
{
    [Header("Attack Settings")]
    public float attackInterval = 0.8f;     // 公开可调的攻速参数
    public float attackDelay = 0.5f;        // 攻击延迟时间
    public int hitpoint = 15;
    public string weaponid = "fist";
    public float attackdistance = 1.5f;//最大攻击距离
    public float StaminaNeed = 20;//

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip attackSound;

    private float lastAttackTime;         // 上次攻击时间记录

    private PlayerState playerState;

    void Start()
    {
        // 初始化为可立即攻击状态
        lastAttackTime = -attackInterval;

        playerState = PlayerState.Instance;
    }

    public string GetIdleAnimationName()
    {
        return "FistIdle"; // 返回拳头的空闲动画名称
    }

    public string GetAttackAnimationName()
    {
        return "FistAttack"; // 返回拳头的攻击动画名称
    }

    public void OnAttack()
    {
        // 启动协程，延迟执行攻击逻辑
        lastAttackTime = Time.time;  // 攻击后刷新计时
        StartCoroutine(DelayedAttack());
        playerState.ModifyStamina(-StaminaNeed);
    }

    private IEnumerator DelayedAttack()
    {
        if(audioSource!=null && attackSound!= null)
            audioSource.PlayOneShot(attackSound);

        // 等待指定的延迟时间
        yield return new WaitForSeconds(attackDelay);

        // 斧头的攻击逻辑
        Debug.Log("Fist attack: Damage applied!");
        PlayerInteract.Instance.TakeDamage(weaponid, hitpoint, attackdistance);
        
    }

    public bool CanAttack()
    {
        if ((playerState.Stamina < StaminaNeed))
        {
            return false;
        }
        // 当时间超过上次攻击时间+CD时允许攻击
        return Time.time >= lastAttackTime + attackInterval;
    }
}