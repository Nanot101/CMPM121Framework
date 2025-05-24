using UnityEngine;

public class CoroutineMonoHelper : MonoBehaviour
{
    public static CoroutineMonoHelper Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}