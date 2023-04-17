using UnityEngine;

public abstract class QueuePrompt : MonoBehaviour
{
    [HideInInspector] public QueuePromptType queuePromptType;
    [HideInInspector] public float delayDuration = 1;
    protected internal bool IsOpen;

    public virtual void OpenRequest(int priority)
    {
        PromptQueueManager.Instance.QueueInsert(this, priority);
    }
    public virtual void OpenRequest()
    {
        PromptQueueManager.Instance.QueueInsert(this, 0);
    }
    public virtual void Open()
    {
        IsOpen = true;
    }
    public virtual void Close()
    {
        IsOpen = false;
    }
}

public enum QueuePromptType
{
    Instant,
    Delayed,
    Continuous
}
