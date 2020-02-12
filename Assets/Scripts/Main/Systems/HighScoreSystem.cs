using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class HighScoreSystem : JobComponentSystem
{
    private readonly SavedInt savedHighscore = new SavedInt("spherestroyer-leaderboard");

    private EntityQuery highScoreUi;
    private EntityQuery disabledHighscoreUi;

    public int CurrentHighscore => savedHighscore;

    protected override void OnCreate()
    {
        highScoreUi = GetEntityQuery(ComponentType.ReadOnly<HighscoreTag>());
        disabledHighscoreUi = GetEntityQuery(
            ComponentType.ReadOnly<HighscoreTag>(),
            ComponentType.ReadOnly<Disabled>());

        RequireSingletonForUpdate<GameState>();
    }

    protected override void OnStartRunning()
    {
        if (savedHighscore == 0)
        {
            EntityManager.AddComponent(highScoreUi, typeof(Disabled));
            Logger.Log("Disabling highscore");
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
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
                    ecb.RemoveComponent(disabledHighscoreUi, typeof(Disabled));
                    ecb.AddComponent(highScoreUi, typeof(ActivatedTag));
                    ecb.AddComponent(ecb.CreateEntity(), new SoundRequest { Value = SoundType.Highscore });
                    savedHighscore.Value = gameData.score;
                }

                Y8.Api.SaveHighscore("Leaderboard", gameData.score);

                ecb.DestroyEntity(entity);
            }).Run();
        ecb.Playback(EntityManager);
        ecb.Dispose();
        return default;
    }
}