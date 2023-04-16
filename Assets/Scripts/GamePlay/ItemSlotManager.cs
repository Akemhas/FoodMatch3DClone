using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotManager : Singleton<ItemSlotManager>
{
    public ItemSlot itemSlot => _itemSlots[0];
    [SerializeField] private ItemSlot[] _itemSlots;
    [SerializeField] private Transform seperatorCollider;

    [SerializeField][Range(0, 12)] private float _slotScale = 8;
    [SerializeField][Range(0, 1)] private float _slotsYPosition = .15f;
    [SerializeField][Range(0, .5f)] private float _padding = .25f;
    [SerializeField] private float zOffsetFromCamera = 11;

    private void OnValidate()
    {
        if (_itemSlots == null || _itemSlots.Length == 0) _itemSlots = GetComponentsInChildren<ItemSlot>();
        if (_itemSlots == null || _itemSlots.Length <= 0) return;
        Camera cam = Camera.main;
        int length = _itemSlots.Length;
        float spacing = (1 - _padding) / Mathf.Clamp(_itemSlots.Length - 1, 0, float.MaxValue);
        for (int i = 0; i < length; i++)
        {
            Vector3 pos = new(_padding * .5f + spacing * i, _slotsYPosition, zOffsetFromCamera);
            _itemSlots[i].transform.position = cam.ViewportToWorldPoint(pos);
            _itemSlots[i].model.localScale = Vector3.one * _slotScale / zOffsetFromCamera;
        }
        seperatorCollider.position = cam.ViewportToWorldPoint(new Vector3(0.5f, _slotsYPosition + 0.1f, zOffsetFromCamera));
    }
}
