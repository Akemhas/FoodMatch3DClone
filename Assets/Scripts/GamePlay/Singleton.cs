using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
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

    protected virtual void Awake()
    {
        _instance = (T)this;
    }
}