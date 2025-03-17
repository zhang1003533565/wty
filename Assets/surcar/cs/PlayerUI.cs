using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // 引用 PlayerState 单例
    private PlayerState playerState;

    // UI 元素
    public Slider healthSlider;
    public Slider hungerSlider;
    public Slider thirstSlider;
    public Slider staminaSlider;

    // 游戏结束 UI
    public GameObject gameOverUI;
    public Button respawnButton;

    private void Start()
    {
        // 获取 PlayerState 单例
        playerState = PlayerState.Instance;

        // 初始化 UI
        UpdateUI();

        // 绑定重生按钮事件
        respawnButton.onClick.AddListener(OnRespawnButtonClicked);

        // 初始隐藏游戏结束 UI
        gameOverUI.SetActive(false);
    }

    private void Update()
    {
        // 更新 UI
        UpdateUI();

        // 检测玩家是否死亡
        if (playerState.IsDead)
        {
            ShowGameOverUI();
        }
    }

    // 更新 UI 进度条
    private void UpdateUI()
    {
        healthSlider.value = playerState.Health / 100f;
        hungerSlider.value = playerState.Hunger / 100f;
        thirstSlider.value = playerState.Thirst / 100f;
        staminaSlider.value = playerState.Stamina / 100f;
    }

    // 显示游戏结束 UI
    private void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    // 重生按钮点击事件
    private void OnRespawnButtonClicked()
    {
        playerState.Respawn();
        gameOverUI.SetActive(false);
    }
}
