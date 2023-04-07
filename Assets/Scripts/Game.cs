using Code;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
        
    public PlayerData PlayerData;
        
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
        
    private void Start()
    {
        PlayerData = new PlayerData();
    }
}