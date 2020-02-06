using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class StartGameSystem : JobComponentSystem
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameData>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
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
                UpdateScoreUI(ecb);

                ecb.DestroyEntity(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
        return default;
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
        gameData.IsGameActive = true;
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

    private void UpdateScoreUI(EntityCommandBuffer ecb)
    {
        EntityQuery uiQuery = GetEntityQuery(ComponentType.ReadOnly(typeof(ScoreTag)));
        ecb.AddComponent(uiQuery, typeof(ActivatedTag));
    }
}