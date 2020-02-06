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
        if (!TryInitialized()) return;
        ExternalAPI.ShowHighscore(tableId, ExternalAPI.JsCallback);
    }

    public void SaveHighscore(string tableId, int score)
    {
        if (!TryInitialized()) return;
        if (!TryLoggedIn()) return;
        ExternalAPI.SaveHighscore(tableId, score, ExternalAPI.JsCallback);
    }

    private bool TryInitialized()
    {
        if (!IsInitialized)
        {
            Logger.Log("[Y8] Not initialized");
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool TryLoggedIn()
    {
        if (!IsLoggedIn)
        {
            Logger.Log("[Y8] Not logged in");
            return false;
        }
        else
        {
            return true;
        }
    }
}