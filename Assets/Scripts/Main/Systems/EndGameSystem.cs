using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class EndGameSystem : JobComponentSystem
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameData>();
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

                ReactivateTouchSymbol(ecb);

                ecb.AddComponent<UpdateHighscoreTag>(ecb.CreateEntity());
                ecb.DestroyEntity(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();

        return default;
    }

    private void ReactivateTouchSymbol(EntityCommandBuffer ecb)
    {
        EntityQuery disabledTouchSymbol = GetEntityQuery(
                 ComponentType.ReadOnly(typeof(Disabled)),
                 ComponentType.ReadOnly(typeof(TouchSymbolTag)));
        ecb.RemoveComponent(disabledTouchSymbol, typeof(Disabled));
    }
}