using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class EndGameSystem : JobComponentSystem
{
    private EntityQuery disabledTouchSymbolEQ;

    protected override void OnCreate()
    {
        var eqDesc = new EntityQueryDesc()
        {
            All = new ComponentType[] {
                ComponentType.ReadOnly<TouchSymbolTag>(),
                ComponentType.ReadWrite<Disabled>()
            },

            Options = EntityQueryOptions.IncludeDisabled
        };

        RequireSingletonForUpdate<GameData>();
        disabledTouchSymbolEQ = GetEntityQuery(eqDesc);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var gameData = GetSingleton<GameData>();

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities
            .WithoutBurst()
            .ForEach((Entity entity, ref GameEnd gameEnd) =>
            {
                if (0 < gameEnd.TimeToEnd)
                {
                    gameEnd.TimeToEnd -= Time.DeltaTime;
                    return;
                }

                gameData.IsGameActive = false;
                SetSingleton(gameData);

                Logger.Log($"Does not work under debug. Child reparenting is causing issues (?)");
                ecb.RemoveComponent(disabledTouchSymbolEQ, typeof(Disabled));
                ecb.AddComponent<UpdateHighscoreTag>(ecb.CreateEntity());
                ecb.DestroyEntity(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();

        return default;
    }
}