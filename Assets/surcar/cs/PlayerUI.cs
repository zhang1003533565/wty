using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // ���� PlayerState ����
    private PlayerState playerState;

    // UI Ԫ��
    public Slider healthSlider;
    public Slider hungerSlider;
    public Slider thirstSlider;
    public Slider staminaSlider;

    // ��Ϸ���� UI
    public GameObject gameOverUI;
    public Button respawnButton;

    private void Start()
    {
        // ��ȡ PlayerState ����
        playerState = PlayerState.Instance;

        // ��ʼ�� UI
        UpdateUI();

        // ��������ť�¼�
        respawnButton.onClick.AddListener(OnRespawnButtonClicked);

        // ��ʼ������Ϸ���� UI
        gameOverUI.SetActive(false);
    }

    private void Update()
    {
        // ���� UI
        UpdateUI();

        // �������Ƿ�����
        if (playerState.IsDead)
        {
            ShowGameOverUI();
        }
    }

    // ���� UI ������
    private void UpdateUI()
    {
        healthSlider.value = playerState.Health / 100f;
        hungerSlider.value = playerState.Hunger / 100f;
        thirstSlider.value = playerState.Thirst / 100f;
        staminaSlider.value = playerState.Stamina / 100f;
    }

    // ��ʾ��Ϸ���� UI
    private void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    // ������ť����¼�
    private void OnRespawnButtonClicked()
    {
        playerState.Respawn();
        gameOverUI.SetActive(false);
    }
}
