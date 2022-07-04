using Symnity.Core.Crypto;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LessonBasic3 : MonoBehaviour
{
    [SerializeField] private TMP_InputField privateKeyInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button registerEncryptedPrivateKeyButton;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TMP_InputField showPasswordInputField;
    [SerializeField] private Button showPrivateKeyButton;
    [SerializeField] private TextMeshProUGUI showPrivateKey;

private void Start()
{
    registerEncryptedPrivateKeyButton.onClick.AddListener(RegisterEncryptedPrivateKey);
    showPrivateKeyButton.onClick.AddListener(ShowPrivateKey);
}

    private void RegisterEncryptedPrivateKey()
    {
        warningText.text = "";
        if (!ValidatePassword())
        {
            warningText.text = "Wrong password format.";
            return;
        }

        var encryptedPrivateKey = Crypto.EncryptString(privateKeyInputField.text, passwordInputField.text, SystemInfo.deviceUniqueIdentifier);
        PlayerPrefs.SetString("ENCRYPTED_PRIVATE_KEY", encryptedPrivateKey);
    }

private void ShowPrivateKey()
{
    var encryptedPrivateKey = PlayerPrefs.GetString("ENCRYPTED_PRIVATE_KEY", "");
    showPrivateKey.text = Crypto.DecryptString(encryptedPrivateKey, showPasswordInputField.text, SystemInfo.deviceUniqueIdentifier);
}

    private bool ValidatePassword()
    {
        return passwordInputField.text != "";
    }
}
