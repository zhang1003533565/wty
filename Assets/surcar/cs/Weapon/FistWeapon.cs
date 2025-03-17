using System.Collections;
using UnityEngine;

public class FistWeapon : MonoBehaviour, IWeapon
{
    [Header("Attack Settings")]
    public float attackInterval = 0.8f;     // �����ɵ��Ĺ��ٲ���
    public float attackDelay = 0.5f;        // �����ӳ�ʱ��
    public int hitpoint = 15;
    public string weaponid = "fist";
    public float attackdistance = 1.5f;//��󹥻�����
    public float StaminaNeed = 20;//

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip attackSound;

    private float lastAttackTime;         // �ϴι���ʱ���¼

    private PlayerState playerState;

    void Start()
    {
        // ��ʼ��Ϊ����������״̬
        lastAttackTime = -attackInterval;

        playerState = PlayerState.Instance;
    }

    public string GetIdleAnimationName()
    {
        return "FistIdle"; // ����ȭͷ�Ŀ��ж�������
    }

    public string GetAttackAnimationName()
    {
        return "FistAttack"; // ����ȭͷ�Ĺ�����������
    }

    public void OnAttack()
    {
        // ����Э�̣��ӳ�ִ�й����߼�
        lastAttackTime = Time.time;  // ������ˢ�¼�ʱ
        StartCoroutine(DelayedAttack());
        playerState.ModifyStamina(-StaminaNeed);
    }

    private IEnumerator DelayedAttack()
    {
        if(audioSource!=null && attackSound!= null)
            audioSource.PlayOneShot(attackSound);

        // �ȴ�ָ�����ӳ�ʱ��
        yield return new WaitForSeconds(attackDelay);

        // ��ͷ�Ĺ����߼�
        Debug.Log("Fist attack: Damage applied!");
        PlayerInteract.Instance.TakeDamage(weaponid, hitpoint, attackdistance);
        
    }

    public bool CanAttack()
    {
        if ((playerState.Stamina < StaminaNeed))
        {
            return false;
        }
        // ��ʱ�䳬���ϴι���ʱ��+CDʱ������
        return Time.time >= lastAttackTime + attackInterval;
    }
}