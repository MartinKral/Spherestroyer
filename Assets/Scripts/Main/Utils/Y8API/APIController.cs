namespace Y8
{
    public class APIController
    {
        public APIController(string appId)
        {
            ExternalAPI.Init(appId);
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
}