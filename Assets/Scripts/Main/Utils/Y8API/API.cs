using System;

public class API
{
    public bool IsLoggedIn { get; private set; }

    public API(string appId)
    {
        ExternalAPI.Init(appId);

        /// This is just to test calling C# from JS for later implementation
        ExternalAPI.WithCallback(ExternalAPI.Callback);
    }

    /* [MonoPInvokeCallback(typeof(Action<string>))]
     public static void Callback(string callbackType)
     {
         Logger.Log($"C# function callback from JS {callbackType}");
     }*/

    public void ShowHighscore(string tableId)
    {
        ExternalAPI.ShowHighscore(tableId);
    }

    public void SaveHighscore(string tableId, int score)
    {
        ExternalAPI.SaveHighscore(tableId, score);
    }
}