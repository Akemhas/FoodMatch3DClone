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
        OnGameStart?.Invoke();
    }

}

public enum GameStage
{
    Start,
    Play,
    Pause,
    End,
}
