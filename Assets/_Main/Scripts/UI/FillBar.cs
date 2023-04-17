using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class FillBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float fillDuration = .15f;

    public Action OnFillExpired;

    private Sequence fillSequence;

    // Fill amount is between 0 and 1
    public void FillTheBar(float fillAmount, float fillExpireDuration)
    {
        fillSequence?.Kill();
        fillSequence = DOTween.Sequence();
        fillSequence.Append(fillImage.DOFillAmount(fillAmount, fillDuration).SetEase(Ease.Linear));
        if (fillExpireDuration == -1) return;
        fillSequence.Append(fillImage.DOFillAmount(0, fillExpireDuration).SetEase(Ease.Linear)).OnComplete(()=>OnFillExpired?.Invoke());
    }

    public void SetFillAmount(float fillAmount)
    {
        fillImage.fillAmount = fillAmount;
    }
}
