using UnityEngine;

public class FirstPersonFootsteps : MonoBehaviour
{
    public AudioSource footstepAudioSource; // �Ų�������AudioSource
    public float footstepDelay = 0.5f; // �Ų������Ĳ��ż��
    public float minSpeedToPlay = 0.1f; // �����Ų���������С�ٶ�

    private Vector3 previousPosition; // ��һ֡��λ��
    private float currentSpeed; // ��ǰ�ٶ�
    private float nextFootstepTime;

    void Start()
    {
        previousPosition = transform.position; // ��ʼ����һ֡��λ��
        if (footstepAudioSource == null)
        {
            Debug.LogError("Footstep AudioSource is not assigned!");
        }
    }

    void Update()
    {
        // ���㵱ǰ�ٶ�
        currentSpeed = CalculateSpeed();

        // ����ٶ��Ƿ������С�ٶ�
        if (currentSpeed > minSpeedToPlay)
        {
            // �����ٶȵ����Ų������Ĳ���Ƶ��
            float speedMultiplier = Mathf.Clamp(currentSpeed, 0.5f, 2f);
            float currentFootstepDelay = footstepDelay / speedMultiplier;

            // ���ŽŲ�����
            if (Time.time >= nextFootstepTime)
            {
                footstepAudioSource.Play();
                nextFootstepTime = Time.time + currentFootstepDelay;
            }
        }
    }

    // �����ٶȵķ���
    float CalculateSpeed()
    {
        // ��ȡ��ǰλ��
        Vector3 currentPosition = transform.position;

        // ����λ��
        float distanceMoved = Vector3.Distance(currentPosition, previousPosition);

        // �����ٶȣ�λ�� / ʱ�䣩
        float speed = distanceMoved / Time.deltaTime;

        // ������һ֡��λ��
        previousPosition = currentPosition;

        return speed;
    }
}
