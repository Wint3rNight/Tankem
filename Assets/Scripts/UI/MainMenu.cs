using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeInputField;
    public async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHostAsync();
    }

    public async void StartClient()
    {
        // Trim spaces AND force Uppercase
        string cleanCode = joinCodeInputField.text.Trim().ToUpper();

        Debug.Log($"Attempting to join with code: '{cleanCode}'"); // Log exactly what we are sending

        if (string.IsNullOrEmpty(cleanCode))
        {
            Debug.LogWarning("Join Code is empty!");
            return;
        }

        await ClientSingleton.Instance.GameManager.StartClientAsync(cleanCode);
    }
}
