using System.Runtime.InteropServices;

namespace Y8
{
    public static class ExternalAPI
    {
        [DllImport("__Internal")]
        internal static extern void Init(string appId);

        [DllImport("__Internal")]
        internal static extern bool IsLoggedIn();

        [DllImport("__Internal")]
        internal static extern void ShowHighscore(string tableId);

        [DllImport("__Internal")]
        internal static extern void SaveHighscore(string tableId, int score);
    }
}