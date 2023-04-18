using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public abstract class CurrencyUI<T> : CanvasUI<T> where T : MonoBehaviour
{
    public CurrencyType currencyType;
    public RectTransform currencyIcon;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private float travelAnimDuration = .34f;
    public Action OnCurrencyChange;

    private string _id;
    private string _currencyTag;
    private Tween _textScaleTween, _iconScaleTween;

    public virtual int CurrencyAmount
    {
        get => PlayerPrefs.GetInt(_id, 0);
        protected set => PlayerPrefs.SetInt(_id, value);
    }

    protected override void Awake()
    {
        _id = $"{currencyType}-amount";
        _currencyTag = currencyType.ToString();

        _textScaleTween = currencyText.transform.DOPunchScale(Vector3.one * .4f, .2f, 2, .5f).Pause().SetAutoKill(false).SetLink(currencyText.gameObject);
        _iconScaleTween = currencyIcon.transform.DOPunchScale(Vector3.one * .8f, .2f, 2, .5f).Pause().SetAutoKill(false).SetLink(currencyIcon.gameObject);

        base.Awake();
    }

    private void Start()
    {
        currencyText.SetText(CurrencyAmount.FormatMoney());
    }

    public bool HasEnoughCurrency(int required) => CurrencyAmount >= required;

    public void AddCurrency(int amount)
    {
        SetCurrency(CurrencyAmount + amount);
        _textScaleTween.Restart();
        _iconScaleTween.Restart();
        OnCurrencyChange?.Invoke();
    }

    public void AddCurrency(int amount, Vector3 from, int count, bool update)
    {
        StartCoroutine(AddCurrencyRoutine(amount, from, count, update));
    }

    public IEnumerator AddCurrencyRoutine(int amount, Vector3 from, int count, bool update)
    {
        if (amount == 0) yield break;
        int coinAddAmount = amount / count;
        int coinCount = count;
        float delay = 0;
        int addedCoinAmount = 0;
        int addedCointCount = 0;
        for (int i = 0; i < count; i++)
        {
            delay = coinCount * .012f;
            coinCount--;

            RectTransform coinIcon = RectTransformPooler.Instance.Get(_currencyTag, from, transform);
            coinIcon.transform.DOMove(currencyIcon.position, travelAnimDuration + delay).SetDelay(.05f + delay)
            .OnComplete(() =>
                            {
                                RectTransformPooler.Instance.Release(_currencyTag, coinIcon);
                                AddCurrency(coinAddAmount);
                                addedCoinAmount += coinAddAmount;
                                addedCointCount++;
                            }).SetUpdate(update).SetLink(coinIcon.gameObject);
        }

        yield return new WaitUntil(() => addedCointCount == count);
        AddCurrency(amount - addedCoinAmount);
    }


    public bool SpendCurrency(int amount)
    {
        if (CurrencyAmount < amount) return false;

        SetCurrency(CurrencyAmount - amount);
        OnCurrencyChange?.Invoke();
        return true;
    }

    public void SetCurrency(int amount)
    {
        CurrencyAmount = amount;
        currencyText.text = CurrencyAmount.FormatMoney();
        OnCurrencyChange?.Invoke();
    }
}

public enum CurrencyType
{
    Coin,
}
