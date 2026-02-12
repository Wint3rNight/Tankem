using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthenticationState AuthenticationState{get;private set;}= AuthenticationState.NotAuthenticated;

    public static async Task<AuthenticationState> Authenticate(int maxRetries = 3)
    {
        if (AuthenticationState == AuthenticationState.Authenticated)
        {
            return AuthenticationState;
        }

        if (AuthenticationState == AuthenticationState.Authenticating)
        {
            Debug.LogWarning("Already authenticating. Please wait.");
            await Authenticating();
            return AuthenticationState;
        }
        
        await SignInAnonymouslyAsync(maxRetries);
        
        return AuthenticationState;
    }

    private static async Task<AuthenticationState> Authenticating()
    {
        while (AuthenticationState == AuthenticationState.Authenticating || AuthenticationState == AuthenticationState.NotAuthenticated)
        {
            await Task.Delay(500);
        }

        return AuthenticationState;
    }
    private static async Task SignInAnonymouslyAsync(int maxRetries)
    {
        AuthenticationState = AuthenticationState.Authenticating;
        
        int retries = 0;
        while(AuthenticationState== AuthenticationState.Authenticating && retries < maxRetries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthenticationState = AuthenticationState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException authenticationException)
            {
                Debug.LogError($"Authentication failed: {authenticationException.Message}");
                AuthenticationState = AuthenticationState.AuthenticationFailed;
            }
            catch (RequestFailedException requestFailedException)
            {
                Debug.LogError($"Request failed: {requestFailedException.Message}");
                AuthenticationState = AuthenticationState.AuthenticationFailed;
            }
            
            retries++;
            await Task.Delay(1000);
        }

        if (AuthenticationState != AuthenticationState.Authenticated)
        {
            Debug.LogWarning($"Player failed to authenticate after {maxRetries} attempts.");
            AuthenticationState = AuthenticationState.Timeout;
        } 
    }
}

public enum AuthenticationState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    AuthenticationFailed,
    Timeout
}
