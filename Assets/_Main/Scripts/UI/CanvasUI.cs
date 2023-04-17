using UnityEngine;

public abstract class CanvasUI<T> : Singleton<T> where T : MonoBehaviour
{
    private Canvas canvas;
    private int _defaultSortOrder;

    protected virtual void Awake()
    {
        _defaultSortOrder = canvas.sortingOrder;
    }

    public bool IsCanvasEnable => canvas.enabled;

    public void SetHighlightStatus(bool status)
    {
        if (status) canvas.sortingOrder = 99;
        else canvas.sortingOrder = _defaultSortOrder;
    }

    public void SetCanvasEnable(bool value)
    {
        canvas.enabled = value;
    }
}
