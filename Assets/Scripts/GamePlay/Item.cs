using UnityEngine;

public class Item : MonoBehaviour
{
    public Rigidbody rb;

    // Too Lazy to fix nesting issue in here... :D At least that I can say even Unity did this once
    public ItemData itemData;

    void OnValidate()
    {
        if (rb == null) rb = GetComponentInChildren<Rigidbody>();
    }
}

public enum ItemType
{
    Tomato,
    Pizza,
    Hamburger
}
