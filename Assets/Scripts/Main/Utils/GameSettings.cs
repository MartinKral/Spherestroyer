public class GameSettings
{
    public static GameSettings Instance { get; } = new GameSettings();
    public GameData Data { get; private set; }

    public static void Init(GameData gameData)
    {
        if (Instance.Data.Equals(default(GameData)))
        {
            Instance.Data = gameData;
        }
        else
        {
            Logger.Log("Game data already initialized");
        }
    }
}