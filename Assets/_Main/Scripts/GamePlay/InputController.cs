using UnityEngine;
using DG.Tweening;

public class InputController : MonoBehaviour
{
    [SerializeField] private float highlightRiseAmount = 3;

    private Item selectedItem;
    private Camera mainCam;
    private Vector3 startPos;
    private Quaternion startRot;
    private int selectedInstanceID;
    private bool hasItem;
    private int frameCounter;
    [SerializeField] private LayerMask rayMask;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if(StageController.CurrentStage == GameStage.End) return;
        if(StageController.CurrentStage == GameStage.Pause) return;
        if (Input.GetMouseButton(0))
        {
            frameCounter++;
            if (frameCounter > 2)
            {
                TryGetItem();
                frameCounter = 0;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (hasItem)
            {
                ItemSlotManager.Instance.SlotTheItem(selectedItem);
                hasItem = false;
                selectedItem = null;
            }
        }
    }

    private void ReleaseItem()
    {
        Rigidbody rb = selectedItem.rb;
        rb.isKinematic = false;
        DOTween.Kill(selectedItem.GetInstanceID());
        rb.DOMove(startPos, .1f).SetId(selectedItem.GetInstanceID());
        selectedItem.transform.DORotateQuaternion(startRot, .1f).SetId(selectedItem.GetInstanceID());
        hasItem = false;
        selectedItem = null;
        selectedInstanceID = -1;
    }

    private void SelectItem(Item item)
    {
        selectedItem = item;
        startPos = selectedItem.transform.position;
        startPos.y = Mathf.Clamp(startPos.y, 0, .3f);
        startRot = selectedItem.transform.rotation;
        item.rb.isKinematic = true;
        var pos = selectedItem.transform.position;
        var highlightedPos = pos + (mainCam.transform.position - pos).normalized * highlightRiseAmount;
        highlightedPos.y = Mathf.Clamp(highlightedPos.y, 0, highlightRiseAmount);
        selectedItem.transform.position = highlightedPos;
        hasItem = true;
    }

    private bool TryGetItem()
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 50, rayMask))
        {
            if (selectedInstanceID == hit.colliderInstanceID) return true;
            if (hit.collider.TryGetComponent(out Item item))
            {
                if (hasItem) ReleaseItem();
                selectedInstanceID = hit.colliderInstanceID;
                SelectItem(item);
                return true;
            }
        }
        else if (hasItem)
        {
            ReleaseItem();
        }
        return false;
    }

}