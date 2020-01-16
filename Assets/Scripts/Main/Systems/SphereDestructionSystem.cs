using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(DestructionBufferSystem))]
public class SphereDestructionSystem : JobComponentSystem
{
    private EntityQuery shakeTarget;
    private EntityQuery uiUpdateTarget;

    protected override void OnCreate()
    {
        shakeTarget = GetEntityQuery(ComponentType.ReadOnly(typeof(Shake)));
        uiUpdateTarget = GetEntityQuery(ComponentType.ReadOnly(typeof(ScoreTag)));

        RequireSingletonForUpdate<GameData>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        var gameData = GetSingleton<GameData>();

        Entities
            .WithAll<DestroyedTag, SphereTag>()
            .WithoutBurst()
            .ForEach((Entity entity, in Translation translation) =>
            {
                gameData.score++;
                SetSingleton(gameData);

                ecb.AddComponent(shakeTarget, typeof(ActivatedTag));
                ecb.AddComponent(uiUpdateTarget, typeof(ActivatedTag));

                ecb.DestroyEntity(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();

        return default;
    }
}