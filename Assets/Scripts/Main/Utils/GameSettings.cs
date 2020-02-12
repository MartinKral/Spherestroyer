public class GameSettings
{
    public static GameSettings Instance { get; private set; }
    public GameData Data { get; }

    private GameSettings(GameData gameData)
    {
        Data = gameData;
    }

    public static void Init(GameData gameData)
    {
        if (Instance == null)
        {
            Instance = new GameSettings(gameData);
        }
        else
        {
            Logger.Log("GameSettings already initialized");
        }
    }
}