public class API
{
    public API(string appId)
    {
        ExternalAPI.Init(appId);

        /// This is just to test calling C# from JS for later implementation
        ExternalAPI.ProvideCallback(ExternalAPI.Callback);
    }

    public bool IsLoggedIn()
    {
        return ExternalAPI.IsLoggedIn();
    }

    public void ShowHighscore(string tableId)
    {
        ExternalAPI.ShowHighscore(tableId);
    }

    public void SaveHighscore(string tableId, int score)
    {
        ExternalAPI.SaveHighscore(tableId, score);
    }
}