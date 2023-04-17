using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleController : MonoBehaviour
{
    public bool isOn = true;

    public Action<bool> OnToggleSwitch;

    [SerializeField] private Button toggleButton;
    [SerializeField] private float toggleDuration = .15f;
    [SerializeField] private Image toggleBgImage;

    [SerializeField] private Sprite toggleOnBGImage;
    [SerializeField] private Sprite toggleOffBGImage;

    [SerializeField] private RectTransform toggleArea;
    [SerializeField] private RectTransform handle;

    private float _handleSize, _onPosX, _offPosX;
    private bool _switching = false;

    void OnEnable()
    {
        toggleButton.onClick.AddListener(Toggle);
    }

    void OnDisable()
    {
        toggleButton.onClick.RemoveListener(Toggle);
    }

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        _handleSize = handle.rect.width;
        float toggleSizeX = toggleArea.rect.width;

        _onPosX = toggleSizeX * 0.5f - _handleSize * 0.5f - _handleSize * 0.1f;

        _offPosX = _onPosX * -1;

        if (isOn)
        {
            toggleBgImage.sprite = toggleOnBGImage;
            handle.localPosition = new Vector3(_onPosX, 0f, 0f);
        }
        else
        {
            toggleBgImage.sprite = toggleOffBGImage;
            handle.localPosition = new Vector3(_offPosX, 0f, 0f);
        }
    }

    public void Toggle()
    {
        SetToggle(!isOn);
    }

    public void SetToggle(bool toggleStatus)
    {
        if (_switching) return;

        _switching = true;

        isOn = toggleStatus;

        if (toggleStatus)
        {
            toggleBgImage.sprite = toggleOnBGImage;
            handle.transform.DOLocalMoveX(_onPosX, toggleDuration).SetLink(handle.gameObject).OnComplete(() => _switching = false).SetUpdate(true);
        }
        else
        {
            toggleBgImage.sprite = toggleOffBGImage;
            handle.transform.DOLocalMoveX(_offPosX, toggleDuration).SetLink(handle.gameObject).OnComplete(() => _switching = false).SetUpdate(true);
        }

        OnToggleSwitch?.Invoke(toggleStatus);
    }
}