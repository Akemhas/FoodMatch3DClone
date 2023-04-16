using UnityEngine;

[CreateAssetMenu(menuName = "FoodMatch3DClone/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public Item itemPrefab;
}