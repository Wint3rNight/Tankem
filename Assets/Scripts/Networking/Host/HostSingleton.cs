using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton _instance;
    
    private HostGameManager _gameManager;
    
    public static HostSingleton Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            _instance = FindFirstObjectByType<HostSingleton>();
            if (_instance == null)
            {
                Debug.LogError("HostSingleton Instance Not Found");
                return null;
            }
            return _instance;
        }
    }
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        _gameManager=new HostGameManager();
    }
    
}
