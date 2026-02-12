using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private const string MenuSceneName = "Menu";
    
    public async Task <bool>InitAsync()
    {
        await UnityServices.InitializeAsync();
        
        AuthenticationState authenticationState = await AuthenticationWrapper.Authenticate();
        
        if (authenticationState == AuthenticationState.Authenticated)
        {
            return true;
        }
        return false;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
}
