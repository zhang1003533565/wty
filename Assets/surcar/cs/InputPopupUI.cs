using UnityEngine;
using UnityEngine.UI;

public class InputPopupUI : MonoBehaviour
{
    public InputField quantityInputField; // ���������
    public Button confirmButton; // ȷ����ť
    public Button cancelButton; // ȡ����ť

    private System.Action<int> _onConfirm; // ȷ�ϻص�
    private System.Action _onCancel; // ȡ���ص�

    // ��ʼ������
    public void Initialize(System.Action<int> onConfirm, System.Action onCancel, int defaultQuantity = 1)
    {
        _onConfirm = onConfirm;
        _onCancel = onCancel;

        // ����Ĭ��ֵ
        quantityInputField.text = defaultQuantity.ToString();

        // �󶨰�ť�¼�
        confirmButton.onClick.AddListener(OnConfirmClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);

        // ��ʾ����
        gameObject.SetActive(true);
    }

    // ȷ�ϰ�ť����¼�
    private void OnConfirmClicked()
    {
        if (int.TryParse(quantityInputField.text, out int quantity) && quantity > 0)
        {
            _onConfirm?.Invoke(quantity);
        }
        else
        {
            Debug.LogWarning("Invalid quantity input.");
        }
        Close();
    }

    // ȡ����ť����¼�
    private void OnCancelClicked()
    {
        _onCancel?.Invoke();
        Close();
    }

    // �رյ���
    private void Close()
    {
        gameObject.SetActive(false);
    }

}