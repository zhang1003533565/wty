using UnityEngine;
using UnityEngine.UI;

public class InputPopupUI : MonoBehaviour
{
    public InputField quantityInputField; // 数量输入框
    public Button confirmButton; // 确定按钮
    public Button cancelButton; // 取消按钮

    private System.Action<int> _onConfirm; // 确认回调
    private System.Action _onCancel; // 取消回调

    // 初始化弹框
    public void Initialize(System.Action<int> onConfirm, System.Action onCancel, int defaultQuantity = 1)
    {
        _onConfirm = onConfirm;
        _onCancel = onCancel;

        // 设置默认值
        quantityInputField.text = defaultQuantity.ToString();

        // 绑定按钮事件
        confirmButton.onClick.AddListener(OnConfirmClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);

        // 显示弹框
        gameObject.SetActive(true);
    }

    // 确认按钮点击事件
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

    // 取消按钮点击事件
    private void OnCancelClicked()
    {
        _onCancel?.Invoke();
        Close();
    }

    // 关闭弹框
    private void Close()
    {
        gameObject.SetActive(false);
    }

}