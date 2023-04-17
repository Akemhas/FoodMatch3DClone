using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptQueueManager : Singleton<PromptQueueManager>
{
    public bool promptsDisabled;

    private Dictionary<int, Queue<QueuePrompt>> _queuePromptPriorityDict = new();
    private List<int> _priorityKeys = new();
    private Coroutine _queueRoutine;
    private WaitUntil _waitForPromptsEnabled;
    private Dictionary<float, WaitForSeconds> _waitForSecondsDict = new();

    private void Awake()
    {
        _waitForPromptsEnabled = new WaitUntil(() => !promptsDisabled);
    }

    public void QueueInsert(QueuePrompt queuePrompt, int priority = 0)
    {
        if (!_queuePromptPriorityDict.ContainsKey(priority))
        {
            _queuePromptPriorityDict.Add(priority, new Queue<QueuePrompt>());
            _priorityKeys.Add(priority);
            _priorityKeys.Sort((x, y) => y.CompareTo(x));
        }

        _queuePromptPriorityDict[priority].Enqueue(queuePrompt);
        if (_queueRoutine == null) _queueRoutine = StartCoroutine(QueueRoutine());
    }

    private IEnumerator QueueRoutine()
    {
        while (TryGetPromptInQueue(out QueuePrompt queuePrompt))
        {
            yield return _waitForPromptsEnabled;

            queuePrompt.Open();


            yield return new WaitUntil(() => !queuePrompt.IsOpen);


            if (TryPeekPromptInQueue(out _))
            {
                switch (queuePrompt.queuePromptType)
                {
                    case QueuePromptType.Instant:
                        break;

                    case QueuePromptType.Delayed:
                        yield return WaitForSeconds(queuePrompt.delayDuration);
                        break;
                }
            }
        }

        _queueRoutine = null;
    }

    private bool TryGetPromptInQueue(out QueuePrompt queuePrompt)
    {
        queuePrompt = null;
        int count = _priorityKeys.Count;
        for (int i = 0; i < count; i++)
        {
            if (_queuePromptPriorityDict[_priorityKeys[i]].Count > 0)
            {
                queuePrompt = _queuePromptPriorityDict[_priorityKeys[i]].Dequeue();
                return true;
            }
        }

        return false;
    }

    private bool TryPeekPromptInQueue(out QueuePrompt queuePrompt)
    {
        queuePrompt = null;
        int count = _priorityKeys.Count;
        for (int i = 0; i < count; i++)
        {
            if (_queuePromptPriorityDict[_priorityKeys[i]].Count > 0)
            {
                return _queuePromptPriorityDict[_priorityKeys[i]].TryPeek(out queuePrompt);
            }
        }

        return false;
    }

    private WaitForSeconds WaitForSeconds(float duration)
    {
        if (_waitForSecondsDict.ContainsKey(duration)) return _waitForSecondsDict[duration];
        else _waitForSecondsDict.Add(duration, new WaitForSeconds(duration));
        return _waitForSecondsDict[duration];
    }
}