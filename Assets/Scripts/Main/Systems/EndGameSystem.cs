using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(SpikeDestructionSystem))]
public class EndGameSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem ecbs;

    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameData>();
        ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var gameData = GetSingleton<GameData>();

        var ecb = ecbs.CreateCommandBuffer();

        Entities
            .WithoutBurst()
            .ForEach((Entity entity, ref GameEnd gameEnd) =>
            {
                if (0 < gameEnd.TimeToEnd)
                {
                    gameEnd.TimeToEnd -= Time.DeltaTime;
                    return;
                }

                gameData.IsGameFinished = true;
                SetSingleton(gameData);

                EntityQuery touchSymbolQuery = GetEntityQuery(
                    ComponentType.ReadOnly(typeof(TouchSymbolTag)),
                    ComponentType.ReadOnly(typeof(Disabled)));

                ecb.RemoveComponent(touchSymbolQuery, typeof(Disabled));

                ecb.DestroyEntity(entity);
            }).Run();

        return default;
    }
}