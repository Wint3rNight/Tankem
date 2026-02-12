using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

public static class AuthenticationWrapper
{
    public static AuthenticationState AuthenticationState{get;private set;}= AuthenticationState.NotAuthenticated;

    public static async Task<AuthenticationState> Authenticate(int maxTries = 3)
    {
        if (AuthenticationState == AuthenticationState.Authenticated)
        {
            return AuthenticationState;
        }
        
        AuthenticationState = AuthenticationState.Authenticating;
        
        int tries = 0;
        while(AuthenticationState== AuthenticationState.Authenticating && tries < maxTries)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
            if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
            {
                AuthenticationState = AuthenticationState.Authenticated;
                break;
            }
            tries++;
            await Task.Delay(1000);
        }
        return AuthenticationState;
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
