using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

[AlwaysSynchronizeSystem]
[UpdateBefore(typeof(StartGameSystem))]
public class HighScoreSystem : JobComponentSystem
{
    private readonly SavedInt savedHighscore = new SavedInt("spherestroyer-highscore");

    private EntityQuery highScoreUi;
    private EntityQuery disabledHighscoreUi;

    public int CurrentHighscore => savedHighscore;

    protected override void OnCreate()
    {
        highScoreUi = GetEntityQuery(ComponentType.ReadOnly(typeof(HighscoreTag)));
        disabledHighscoreUi = GetEntityQuery(
            ComponentType.ReadOnly(typeof(HighscoreTag)),
            ComponentType.ReadOnly(typeof(Disabled)));

        RequireSingletonForUpdate<GameData>();
    }

    protected override void OnStartRunning()
    {
        if (savedHighscore == 0)
        {
            EntityManager.AddComponent(highScoreUi, typeof(Disabled));
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var gameData = GetSingleton<GameData>();

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
                    savedHighscore.Value = gameData.score;
                }
                ecb.DestroyEntity(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
        return default;
    }
}