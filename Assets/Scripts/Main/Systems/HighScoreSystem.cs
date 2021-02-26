using Unity.Entities;
using Unity.Jobs;

[UpdateAfter(typeof(GameSaverSystem))]
[AlwaysSynchronizeSystem]
public class HighScoreSystem : SystemBase
{
    public int CurrentHighscore => savedHighscore;

    private readonly string highscoreSaveKey = "highscore";
    private int savedHighscore = 0;
    private GameSaverSystem gameSaverSystem;

    private bool hasReadScore = false;

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameData>();
        gameSaverSystem = World.GetExistingSystem<GameSaverSystem>();
    }

    protected override void OnUpdate()
    {
        if (gameSaverSystem.IsReady && !hasReadScore)
        {
            hasReadScore = true;
            gameSaverSystem.Read(highscoreSaveKey, ref savedHighscore);
        }

        var gameData = GetSingleton<GameData>();

        Entities
            .WithAll<UpdateHighscoreTag>()
            .ForEach((Entity entity) =>
            {
                if (savedHighscore < gameData.score)
                {
                    EntityManager.AddComponentData(EntityManager.CreateEntity(), new SoundRequest { Value = SoundType.Highscore });
                    savedHighscore = gameData.score;

                    gameSaverSystem.Write(highscoreSaveKey, savedHighscore);
                    gameSaverSystem.Save();
                }

                Y8.Api.SaveHighscore("Leaderboard", gameData.score);

                EntityManager.DestroyEntity(entity);
            }).WithStructuralChanges().Run();
    }
}
