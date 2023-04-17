using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 defaultScale;
    public Rigidbody rb;

    // Too Lazy to fix nesting issue in here... :D At least that I can say even Unity did this once
    public ItemData itemData;

    private void Start()
    {
        defaultScale = transform.localScale;
    }

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
