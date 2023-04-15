using UnityEngine;

public class InputController : MonoBehaviour
{
    public Item selectedItem;
    [SerializeField] private float highlightRiseAmount = 3;
    private Camera mainCam;
    private Vector3 startPos;
    private int selectedInstanceID;
    private bool hasItem;
    [SerializeField] private LayerMask rayMask;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TryGetItem();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (hasItem) ReleaseItem();
        }
    }

    private void ReleaseItem()
    {
        selectedItem.rb.isKinematic = false;
        // selectedItem.rb.
        hasItem = false;
        selectedItem = null;
        selectedInstanceID = -1;
    }

    private void SelectItem(Item item)
    {
        selectedItem = item;
        startPos = selectedItem.transform.position;
        item.rb.isKinematic = true;
        var pos = selectedItem.transform.position;
        selectedItem.transform.position = pos + (mainCam.transform.position - pos).normalized * highlightRiseAmount;
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