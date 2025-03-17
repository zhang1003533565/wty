using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text messageText;
    public Button okButton;
    public GameObject MegBoxObj;

    public static MessageBox instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        okButton.onClick.AddListener(Hide);
    }

    public void Show(string message)
    {
        messageText.text = message;
        MegBoxObj.SetActive(true);
    }

    public void Hide()
    {
        MegBoxObj.SetActive(false);
    }
}