using UnityEngine;

public class safeZoneTester : MonoBehaviour
{
    private static safeZoneTester _instance; 
    public static safeZoneTester Instance {get{return _instance;}}
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
