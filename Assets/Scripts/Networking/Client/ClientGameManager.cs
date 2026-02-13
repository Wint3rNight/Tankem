using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager
{
    private JoinAllocation _allocation;
    private const string MenuSceneName = "Menu";
    
    public async Task <bool>InitAsync()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            return AuthenticationService.Instance.IsSignedIn;
        }
        var options = new InitializationOptions();
        options.SetEnvironmentName("production");
        
        try
        {
            await UnityServices.InitializeAsync(options);
        }
        catch (Exception e)
        {
            Debug.LogError($"Unity Services Init Failed: {e.Message}");
            return false;
        }
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            try 
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"Authentication Failed: {e.Message}");
                return false;
            }
        }

        return AuthenticationService.Instance.IsSignedIn;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public async Task StartClientAsync(string joinCode)
    {
        try
        {
            _allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }
        
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        
        RelayServerData relayServerData = new RelayServerData(
            _allocation.RelayServer.IpV4,
            (ushort)_allocation.RelayServer.Port,
            _allocation.AllocationIdBytes,
            _allocation.ConnectionData,      
            _allocation.HostConnectionData, 
            _allocation.Key,                 
            isSecure: true                  
        );

        transport.SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartClient();
    }
}
