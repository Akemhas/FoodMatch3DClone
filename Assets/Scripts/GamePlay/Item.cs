using UnityEngine;

public class Item : MonoBehaviour
{
    public Rigidbody rb;

    void Start()
    {
        Debug.Log(ItemSlotManager.Instance.itemSlot);
    }
}

public enum ItemType
{
    Apple,
    Pizza,
    Hamburger
}
