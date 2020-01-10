using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(DestructionBufferSystem))]
public class SpikeDestructionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities
            .WithAll<DestroyedTag, SpikeTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                var buffer = EntityManager.GetBuffer<Child>(entity);
                var childArray = buffer.ToNativeArray(Allocator.Temp);

                for (int i = 0; i < childArray.Length; i++)
                {
                    ecb.DestroyEntity(childArray[i].Value);
                }
                childArray.Dispose();

                Entity gameEndedEntity = ecb.CreateEntity();
                ecb.AddComponent(gameEndedEntity, new GameEnd() { TimeToEnd = 2 });

                ecb.DestroyEntity(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
        return default;
    }
}