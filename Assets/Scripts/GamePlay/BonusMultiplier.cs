using TMPro;
using UnityEngine;

public class BonusMultiplier : MonoBehaviour
{
    [SerializeField] private FillBar fillBar;
    [SerializeField] private TextMeshProUGUI countTMP;
    private int _currentMultiplier = 1;

    private void Start()
    {
        fillBar.SetFillAmount(0);
        UpdateCountText();
        fillBar.OnFillExpired += () =>
        {
            _currentMultiplier = 1;
            UpdateCountText();
        };
    }

    public void IncreaseMultiplier()
    {
        _currentMultiplier++;
        UpdateCountText();
        fillBar.FillTheBar(1, 10 / _currentMultiplier);
    }

    private void UpdateCountText() => countTMP.SetText($"X{_currentMultiplier}");
}
