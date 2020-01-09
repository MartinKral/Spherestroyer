using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
public class IncreaseScoreSystem : JobComponentSystem
{
    private EntityQuery entityQuery;

    protected override void OnCreate()
    {
        entityQuery = GetEntityQuery(ComponentType.ReadOnly(typeof(DestroyedIcosphereTag)));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var destroyedSpheres = entityQuery.ToEntityArray(Allocator.TempJob);
        var gameData = GetSingleton<GameData>();

        gameData.score += destroyedSpheres.Length;
        SetSingleton(gameData);

        destroyedSpheres.Dispose();
        return default;
    }
}