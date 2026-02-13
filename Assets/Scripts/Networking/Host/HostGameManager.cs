using System;
using System.Threading.Tasks;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using Unity.Services.Relay;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    private Allocation _allocation;
    private const int MaxConnections = 20;
    private string _joinCode;
    private const string GameSceneName = "GameScene";
    public async Task StartHostAsync()
    {
        try
        { 
            _allocation = await RelayService.Instance.CreateAllocationAsync(MaxConnections);
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay allocation failed: {e.Message}");
            return;
        }
        
        try
        { 
            _joinCode = await RelayService.Instance.GetJoinCodeAsync(_allocation.AllocationId);
            Debug.Log($"Join Code: {_joinCode}");
        }
        catch (Exception e)
        {
            Debug.LogError($"An unexpected error occurred: {e.Message}");
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(
            _allocation.RelayServer.IpV4,
            (ushort)_allocation.RelayServer.Port,
            _allocation.AllocationIdBytes,
            _allocation.ConnectionData,
            _allocation.ConnectionData, 
            _allocation.Key,
            isSecure:true     
        );

        transport.SetRelayServerData(relayServerData);
        NetworkManager.Singleton.StartHost();

        await Task.Delay(500);
        if (NetworkManager.Singleton.IsHost) {
            NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
        }
    }
}
