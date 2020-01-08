using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
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
            .WithAll<GameEndedTag>()
            .WithoutBurst()
            .ForEach((Entity e) =>
            {
                ecb.DestroyEntity(e);
                gameData.IsGameFinished = true;
                SetSingleton(gameData);
            }).Run();

        return default;
    }
}