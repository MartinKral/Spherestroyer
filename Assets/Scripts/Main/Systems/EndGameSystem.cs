using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(SpikeDestructionSystem))]
public class EndGameSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem ecbs;

    private EntityQuery disabledTouchSymbolEQ;

    protected override void OnCreate()
    {
        ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        disabledTouchSymbolEQ = GetEntityQuery(
            ComponentType.ReadOnly(typeof(TouchSymbolTag)),
            ComponentType.ReadOnly(typeof(Disabled)));

        RequireSingletonForUpdate<GameData>();
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

                gameData.IsGameActive = false;
                SetSingleton(gameData);

                ecb.RemoveComponent(disabledTouchSymbolEQ, typeof(Disabled));

                Entity updateHighscoreEntity = ecb.CreateEntity();
                ecb.AddComponent<UpdateHighscoreTag>(updateHighscoreEntity);

                ecb.DestroyEntity(entity);
            }).Run();

        return default;
    }
}