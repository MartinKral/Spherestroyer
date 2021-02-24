using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class EndGameSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameData>();
    }

    protected override void OnUpdate()
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

                gameData.currentGameState = GameState.PreGame;
                SetSingleton(gameData);

                ReactivateTouchSymbol(ecb);

                ecb.AddComponent<UpdateHighscoreTag>(ecb.CreateEntity());
                ecb.DestroyEntity(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();

        return;
    }

    private void ReactivateTouchSymbol(EntityCommandBuffer ecb)
    {
        EntityQuery disabledTouchSymbol = GetEntityQuery(
                 ComponentType.ReadOnly(typeof(Disabled)),
                 ComponentType.ReadOnly(typeof(HideableSymbolTag)));
        ecb.RemoveComponent(disabledTouchSymbol, typeof(Disabled));
    }
}
