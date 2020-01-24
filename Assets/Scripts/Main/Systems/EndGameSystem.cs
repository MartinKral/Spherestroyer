using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
//[UpdateAfter(typeof(SpikeDestructionSystem))]
public class EndGameSystem : JobComponentSystem
{
    private EntityQuery disabledTouchSymbolEQ;

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameData>();
        disabledTouchSymbolEQ = GetEntityQuery(
                    ComponentType.ReadOnly<TouchSymbolTag>(),
                    ComponentType.ReadOnly<Disabled>());
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var gameData = GetSingleton<GameData>();

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity, ref GameEnd gameEnd) =>
            {
                if (0 < gameEnd.TimeToEnd)
                {
                    gameEnd.TimeToEnd -= Time.DeltaTime;
                    return;
                }

                gameData.IsGameActive = false;
                SetSingleton(gameData);
                EntityManager.set

                Logger.Log($"It seems that removing a Disabled component on eq does not work under debug.");
                ecb.RemoveComponent(disabledTouchSymbolEQ, typeof(Disabled));
                ecb.AddComponent<UpdateHighscoreTag>(ecb.CreateEntity());
                ecb.DestroyEntity(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();

        return default;
    }
}