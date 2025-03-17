using UnityEngine;

public class DeathDetect : MonoBehaviour
{
    // ���� PlayerState ����
    private PlayerState playerState;

    // ������Ч
    public AudioClip deathSound;
    public AudioSource audioSource;

    // ��һ�εĴ��״̬
    private bool wasAlive;

    private void Start()
    {
        // ��ȡ PlayerState ����
        playerState = PlayerState.Instance;

        // ��ʼ�� AudioSource
        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.clip = deathSound;

        // ��ʼ�����״̬
        wasAlive = !playerState.IsDead;
    }

    private void Update()
    {
        // �������Ƿ�Ӵ��״̬��Ϊ����״̬
        if (wasAlive && playerState.IsDead)
        {
            PlayDeathSound();
        }

        // ������һ�εĴ��״̬
        wasAlive = !playerState.IsDead;
    }

    // ����������Ч�����β��ţ�
    private void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            // ʹ�� PlayOneShot ������Ч��ȷ��ֻ����һ��
            audioSource.PlayOneShot(deathSound);
        }
    }
}
