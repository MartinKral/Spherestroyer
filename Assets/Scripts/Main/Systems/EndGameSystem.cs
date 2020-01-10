using Unity.Entities;
using Unity.Jobs;

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
            .WithoutBurst()
            .ForEach((Entity entity, ref GameEnd gameEnd) =>
            {
                if (0 < gameEnd.TimeToEnd)
                {
                    gameEnd.TimeToEnd -= Time.DeltaTime;
                }
                else
                {
                    ecb.DestroyEntity(entity);
                    gameData.IsGameFinished = true;
                    SetSingleton(gameData);
                }
            }).Run();

        return default;
    }
}