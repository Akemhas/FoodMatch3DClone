using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotManager : Singleton<ItemSlotManager>
{
    [SerializeField] private ItemSlot[] _itemSlots;

    private int slottedItemCount;
    private Dictionary<ItemType, int> itemAmountDict = new();

    // [SerializeField] private Transform seperatorCollider;
    // [SerializeField][Range(0, 12)] private float _slotScale = 8;
    // [SerializeField][Range(0, 1)] private float _slotsYPosition = .15f;
    // [SerializeField][Range(0, .5f)] private float _padding = .25f;
    // [SerializeField] private float zOffsetFromCamera = 11;

    // private void OnValidate()
    // {
    //     if (_itemSlots == null || _itemSlots.Length == 0) _itemSlots = GetComponentsInChildren<ItemSlot>();
    //     if (_itemSlots == null || _itemSlots.Length <= 0) return;
    //     Camera cam = Camera.main;
    //     int length = _itemSlots.Length;
    //     float spacing = (1 - _padding) / Mathf.Clamp(_itemSlots.Length - 1, 0, float.MaxValue);
    //     for (int i = 0; i < length; i++)
    //     {
    //         Vector3 pos = new(_padding * .5f + spacing * i, _slotsYPosition, zOffsetFromCamera);
    //         _itemSlots[i].transform.position = cam.ViewportToWorldPoint(pos);
    //         _itemSlots[i].model.localScale = Vector3.one * _slotScale / zOffsetFromCamera;
    //     }
    //     seperatorCollider.position = cam.ViewportToWorldPoint(new Vector3(0.5f, _slotsYPosition + 0.1f, zOffsetFromCamera));
    // }

    [ContextMenu("Clear Slots")]
    public void ClearSlots()
    {
        ItemType lastDeletedItemType = ItemType.Empty;
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (!_itemSlots[i].hasItem) continue;
            ItemType itemType = _itemSlots[i].item.itemData.itemType;
            if (lastDeletedItemType != itemType)
            {
                lastDeletedItemType = itemType;
                ItemManager.Instance.DestroyItems(itemType);
            }
            _itemSlots[i].FreeSlot();
            slottedItemCount--;
        }
    }

    public void ClearHint()
    {
        ItemType itemTypeToClear = ItemType.Empty;
        int maxItemAmount = -1;
        for (int i = _itemSlots.Length - 1; i >= 0; i--)
        {
            ItemSlot slot = _itemSlots[i];
            if (!slot.hasItem) continue;
            ItemType itemType = slot.item.itemData.itemType;
            int itemAmount = itemAmountDict[itemType];
            if (itemAmount > maxItemAmount)
            {
                maxItemAmount = itemAmount;
                itemTypeToClear = itemType;
            }
        }

        if (itemTypeToClear == ItemType.Empty)
        {
            itemTypeToClear = ItemManager.Instance.GetAnUnslottedItem();
            maxItemAmount = 3;
        }

        // Not Enough Space for Hint
        if (3 - maxItemAmount > _itemSlots.Length - slottedItemCount) return;

        var itemsToSlot = ItemManager.Instance.GetUnslottedItems(itemTypeToClear);
        for (int i = 0; i < itemsToSlot.Count; i++)
        {
            SlotTheItem(itemsToSlot[i]);
        }
    }

    public void SlotTheItem(Item item)
    {
        ItemType itemType = item.itemData.itemType;
        bool hadCopy = false;
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            // Firstly find if there is another copy of the item in the slots.
            if (_itemSlots[i].hasItem && _itemSlots[i].item.itemData.itemType == itemType)
            {
                // Found another copy of the item.
                RearrangeSlots(item, i);
                AddItemToDict(item);
                hadCopy = true;
                break;
            }
        }

        if (!hadCopy)
        {
            if (GetFreeSlot(out ItemSlot itemSlot))
            {
                itemSlot.SlotTheItem(item);
                AddItemToDict(item);
            }
        }

        if (itemAmountDict[itemType] == 3)
        {
            MergeItems(itemType);
        }

        if (slottedItemCount >= _itemSlots.Length)
        {
            StageController.Instance.FailGame();
        }
    }

    private void MergeItems(ItemType itemType)
    {
        int leftMostIndex = 0;
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (_itemSlots[i].item.itemData.itemType == itemType)
            {
                leftMostIndex = i;
                break;
            }
        }

        var item0 = _itemSlots[leftMostIndex].item;
        var item2 = _itemSlots[leftMostIndex + 2].item;
        _itemSlots[leftMostIndex + 1].SlotTheItem(item0);
        _itemSlots[leftMostIndex + 1].SlotTheItem(item2);

        ItemManager.Instance.DestroyItems(itemType);

        for (int i = leftMostIndex + 3; i < slottedItemCount; i++)
        {
            _itemSlots[i - 3].SlotTheItem(_itemSlots[i].item);
        }

        _itemSlots[slottedItemCount - 3].FreeSlot();
        _itemSlots[slottedItemCount - 2].FreeSlot();
        _itemSlots[slottedItemCount - 1].FreeSlot();

        slottedItemCount -= 3;
        itemAmountDict[itemType] = 0;

        UIManager.Instance.bonusMultiplier.IncreaseMultiplier();
    }

    private void RearrangeSlots(Item item, int index)
    {
        Item tempItem2 = item;
        for (int i = index; i < _itemSlots.Length; i++)
        {
            if (_itemSlots[i].item == null)
            {
                _itemSlots[i].SlotTheItem(tempItem2);
                break;
            }
            if (_itemSlots[i].item.itemData.itemType == item.itemData.itemType) continue;

            var tempItem = _itemSlots[i].item;
            _itemSlots[i].SlotTheItem(tempItem2);
            tempItem2 = tempItem;
        }
    }

    private void AddItemToDict(Item item)
    {
        if (itemAmountDict.ContainsKey(item.itemData.itemType)) itemAmountDict[item.itemData.itemType]++;
        else itemAmountDict.Add(item.itemData.itemType, 1);
        slottedItemCount++;
    }

    private bool GetFreeSlot(out ItemSlot itemSlot)
    {
        itemSlot = null;
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (!_itemSlots[i].hasItem)
            {
                itemSlot = _itemSlots[i];
                return true;
            }
        }
        return false;
    }

}
