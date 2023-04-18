using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 defaultScale;
    public Rigidbody rb;

    // Fix this nesting issue later
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
    Apple,
    Avocado,
    Banana,
    Cherry,
    Lemon,
    Peach,
    Peanut,
    Pear,
    Strawberry,
    Watermelon
}
