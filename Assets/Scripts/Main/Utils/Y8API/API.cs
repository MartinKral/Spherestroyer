using System;

public class API
{
    public bool IsInitialized { get; set; } = false;
    public bool IsLoggedIn { get; set; } = false;

    public API(string appId)
    {
        ExternalAPI.Init(appId, ExternalAPI.JsCallback);
    }

    public void ShowHighscore(string tableId)
    {
        if (!TryIsInitialized()) return;

        ExternalAPI.ShowHighscore(tableId, ExternalAPI.JsCallback);
    }

    public void SaveHighscore(string tableId, int score)
    {
        if (!TryIsInitialized()) return;
        if (!TryIsLoggedIn()) return;

        ExternalAPI.SaveHighscore(tableId, score, ExternalAPI.JsCallback);
    }

    private bool TryIsInitialized()
    {
        if (IsInitialized)
        {
            return true;
        }
        else
        {
            Logger.Log("[Y8] Is not initialized");
            return false;
        }
    }

    private bool TryIsLoggedIn()
    {
        if (IsLoggedIn)
        {
            return true;
        }
        else
        {
            Logger.Log("[Y8] Is not logged in");
            return false;
        }
    }
}