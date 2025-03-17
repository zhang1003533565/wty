using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractable : MonoBehaviour, IInteractable
{
    public string playerName = "Player";

    private PlayerState playerState;

    // ��ƵԴ��������ڲ�����������
    public AudioSource audioSource;
    public AudioClip damageClip;

    // UI��Ч��Animator���
    public Animator damageEffectAnimator;

    // ����������������
    public string effectTriggerName = "ShowDamage";

    private void Start()
    {
        playerState = PlayerState.Instance;

        // ��ȡ����� AudioSource ���
        //audioSource = GetComponent<AudioSource>();
        /*if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }*/

        // ȷ�� damageEffectAnimator ��Ϊ��
        if (damageEffectAnimator != null)
        {
            damageEffectAnimator.enabled = true; // ȷ��Animator����
        }
    }

    // ʵ�� IInteractable �ӿڵķ���
    public bool IsInteractable()
    {
        // ����Ƿ�ɽ��������磺�Ƿ񻹻��ţ�
        return false;
    }

    public bool IsAttackable()
    {
        // ����Ƿ�ɱ����������磺�Ƿ񻹻��ţ�
        return true;
    }

    public string GetName()
    {
        // ������ҵ�����
        return playerName;
    }

    public void Interact()
    {
        // �����߼�
    }

    public void TakeDamage(string weaponName, int damage)
    {
        // �۳����Ѫ��
        playerState.ModifyHealth(-damage);

        // ������������
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(damageClip);
        }

        // ����UI��Ч����
        if (damageEffectAnimator != null)
        {
            damageEffectAnimator.SetTrigger(effectTriggerName);
        }
    }

    public bool isDeath()
    {
        // ��������Ƿ�����
        return playerState.IsDead;
    }
}
