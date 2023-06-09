using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly object Lock = new object();

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    _instance = FindObjectOfType<T>();
                }
            }
            return _instance;
        }
    }
}