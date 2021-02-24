using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class StartGameSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameData>();
    }

    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        Entities
            .WithoutBurst()
            .WithAll<StartGameTag>()
            .ForEach((Entity entity) =>
            {
                ResetGameData();
                ResetSpawner();

                ActivateEntities(ecb);
                ActivateSounds(ecb);
                DestroyAllSpheres(ecb);

                ecb.DestroyEntity(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }

    private void ResetSpawner()
    {
        var sphereSpawner = GetSingleton<SphereSpawner>();
        sphereSpawner.TimesUpgraded = 0;
        sphereSpawner.SecondsUntilSpawn = 0;
        SetSingleton(sphereSpawner);
    }

    private void ResetGameData()
    {
        var gameData = GetSingleton<GameData>();
        gameData.currentGameState = GameState.Game;
        gameData.score = 0;
        SetSingleton(gameData);
    }

    private void ActivateEntities(EntityCommandBuffer ecb)
    {
        EntityQuery disabledEntities = GetEntityQuery(
                    ComponentType.ReadOnly(typeof(Disabled)),
                    ComponentType.ReadOnly(typeof(EnableOnStartTag)));
        ecb.RemoveComponent(disabledEntities, typeof(Disabled));
    }

    private void ActivateSounds(EntityCommandBuffer ecb)
    {
        ecb.AddComponent<StartMusicTag>(ecb.CreateEntity());
    }

    private void DestroyAllSpheres(EntityCommandBuffer ecb)
    {
        EntityQuery spheresQuery = GetEntityQuery(ComponentType.ReadOnly(typeof(SphereTag)));
        var sphereEntities = spheresQuery.ToEntityArray(Allocator.TempJob);
        for (int i = 0; i < sphereEntities.Length; i++)
        {
            ecb.DestroyEntity(sphereEntities[i]);
        }
        sphereEntities.Dispose();
    }
}
