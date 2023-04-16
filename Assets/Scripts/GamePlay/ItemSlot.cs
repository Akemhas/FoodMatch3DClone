using DG.Tweening;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public bool hasItem;
    public Item item;
    public Transform model;

    public void SlotTheItem(Item item)
    {
        this.item = item;
        item.gameObject.layer = 0;
        hasItem = true;

        Rigidbody rb = item.rb;
        rb.isKinematic = true;
        int itemInstanceID = item.GetInstanceID();
        item.transform.DOScale(item.transform.localScale * 0.5f, .3f).SetId(itemInstanceID).SetLink(item.gameObject);
        item.transform.DORotate(new Vector3(0, 10, 0), .3f).SetDelay(0.3f).SetRelative(true).SetEase(Ease.Linear)
                             .SetLoops(-1, LoopType.Incremental).SetLink(item.gameObject).SetId(itemInstanceID);
        item.transform.DOMove(transform.position + Vector3.up * .275f, .3f).SetLink(item.gameObject);
    }

    public void FreeSlot()
    {
        item = null;
        hasItem = false;
    }
}
