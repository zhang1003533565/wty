using UnityEngine;

public class PlayerReg : MonoBehaviour
{
    private void Start()
    {
        // 获取玩家状态实例
        PlayerState playerState = PlayerState.Instance;

        if (playerState != null)
        {
            // 设置出生点为玩家的初始位置
            playerState.SetSpawnPoint(transform);
        }
    }
}
