using UnityEngine;

public class PlayerReg : MonoBehaviour
{
    private void Start()
    {
        // ��ȡ���״̬ʵ��
        PlayerState playerState = PlayerState.Instance;

        if (playerState != null)
        {
            // ���ó�����Ϊ��ҵĳ�ʼλ��
            playerState.SetSpawnPoint(transform);
        }
    }
}
