using System;
using UnityEngine;

public class StageController : Singleton<StageController>
{
    private static GameStage _currentStage = GameStage.Start;
    public static GameStage CurrentStage
    {
        get => _currentStage;
        private set => _currentStage = value;
    }

    [SerializeField] private StageData[] stageDatas;

    private int CurrentStageIndex
    {
        get => PlayerPrefs.GetInt("CurrentStageIndex", 0);
        set => PlayerPrefs.SetInt("CurrentStageIndex", value % stageDatas.Length);
    }

    private StageData stageData;
    private int failCount;
    public static Action OnGameStart;
    public static Action OnGameResume;
    public static Action OnGameEnd;

    private void Start()
    {
        stageData = stageDatas[CurrentStageIndex];
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

    internal void Resume()
    {
        OnGameResume?.Invoke();
        CurrentStage = GameStage.Play;
    }

    public void SuccessGame()
    {
        CurrentStage = GameStage.End;
        CurrentStageIndex++;
        OnGameEnd?.Invoke();
        UIManager.Instance.successUI.Success();
    }

    public void FailGame()
    {
        CurrentStage = GameStage.End;
        failCount++;
        OnGameEnd?.Invoke();
        UIManager.Instance.failUI.Fail(failCount < 2 && !UIManager.Instance.stageTimer.HasTimeEnded);
    }

}

public enum GameStage
{
    Start,
    Play,
    Pause,
    End,
}
