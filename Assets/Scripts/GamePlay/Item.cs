using UnityEngine;

public class Item : MonoBehaviour
{
    public Rigidbody rb;

    void OnValidate()
    {
        if(rb == null) rb = GetComponentInChildren<Rigidbody>();
    }

    void Start()
    {
    }
}

public enum ItemType
{
    Tomato,
    Pizza,
    Hamburger
}
