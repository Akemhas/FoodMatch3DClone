using System;

public class StageController : Singleton<StageController>
{
    private static GameStage _currentStage = GameStage.Start;
    public static GameStage CurrentStage
    {
        get => _currentStage;
        private set => _currentStage = value;
    }

    public StageData stageData;

    public static Action OnGameStart;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        ItemManager.Instance.SpawnIems(stageData);
        UIManager.Instance.stageTimer.StartTimer(stageData.stageDuration);
        CurrentStage = GameStage.Play;
        OnGameStart?.Invoke();
    }

    public void ReviveGame()
    {
        CurrentStage = GameStage.Play;
        ItemSlotManager.Instance.ClearSlots();
    }

    public void PauseGame()
    {
        CurrentStage = GameStage.Pause;
    }

    public void SuccessGame()
    {
        CurrentStage = GameStage.End;
    }

    private int failCount;
    public void FailGame()
    {
        CurrentStage = GameStage.End;
        failCount++;
        UIManager.Instance.failUI.Fail(failCount < 2);
    }
}

public enum GameStage
{
    Start,
    Play,
    Pause,
    End,
}
