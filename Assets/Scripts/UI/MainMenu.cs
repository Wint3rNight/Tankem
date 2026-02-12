using UnityEngine;

public class UI : MonoBehaviour
{
    public async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHostAsync();
    }
}
