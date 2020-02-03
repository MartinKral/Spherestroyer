public class API
{
    public bool IsInitialized { get; set; }
    public bool IsLoggedIn { get; set; }

    public API(string appId)
    {
        ExternalAPI.Init(appId, ExternalAPI.JsCallback);
    }

    public void ShowHighscore(string tableId)
    {
        if (!IsInitialized) return;
        ExternalAPI.ShowHighscore(tableId, ExternalAPI.JsCallback);
    }

    public void SaveHighscore(string tableId, int score)
    {
        if (!IsInitialized) return;
        if (!IsLoggedIn) return;
        ExternalAPI.SaveHighscore(tableId, score, ExternalAPI.JsCallback);
    }
}