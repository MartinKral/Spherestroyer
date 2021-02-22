using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class HighScoreSystem : SystemBase
{
    public int CurrentHighscore => savedHighscore;
    private readonly SavedInt savedHighscore = new SavedInt("spherestroyer-leaderboard");


    protected override void OnCreate()
    {

        RequireSingletonForUpdate<GameState>();
    }

    protected override void OnStartRunning()
    {
        if (savedHighscore == 0)
        {
           // EntityManager.AddComponent(highScoreUi, typeof(Disabled));
            Logger.Log("Disabling highscore");
        }
    }

    protected override void OnUpdate()
    {
        var gameData = GetSingleton<GameState>();

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities
            .WithoutBurst()
            .WithAll<UpdateHighscoreTag>()
            .ForEach((Entity entity) =>
            {
                if (savedHighscore < gameData.score)
                {
                    ecb.AddComponent(ecb.CreateEntity(), new SoundRequest { Value = SoundType.Highscore });
                    savedHighscore.Value = gameData.score;
                }

                Y8.Api.SaveHighscore("Leaderboard", gameData.score);

                ecb.DestroyEntity(entity);
            }).Run();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
