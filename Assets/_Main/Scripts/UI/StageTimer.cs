using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class StageTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerTMP;

    private int _currentTimer;
    private WaitForSeconds oneSecWait;
    public Action OnTimerEnd;

    private void Awake()
    {
        oneSecWait = new WaitForSeconds(1);
    }

    public void StartTimer(int duration)
    {
        _currentTimer = duration;
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        SetTimerText(_currentTimer);
        while (_currentTimer > 0)
        {
            yield return oneSecWait;
            if (StageController.CurrentStage == GameStage.Pause || StageController.CurrentStage == GameStage.End)
            {
                yield return null;
                continue;
            }
            _currentTimer--;
            SetTimerText(_currentTimer);
        }

        OnTimerEnd?.Invoke();
    }

    private void SetTimerText(int timerInSecond)
    {
        timerTMP.SetText($"{timerInSecond / 60:00.##}:{timerInSecond % 60:00.##}");
    }
}
