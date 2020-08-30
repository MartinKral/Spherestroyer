using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Audio;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(SphereCollisionSystem))]
public class SpikeDestructionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<DestroyedTag, SpikeTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                var buffer = EntityManager.GetBuffer<LinkedEntityGroup>(entity);
                var linkedEntityGroup = buffer.ToNativeArray(Allocator.Temp);

                for (int i = 0; i < linkedEntityGroup.Length; i++)
                {
                    EntityManager.AddComponent<Disabled>(linkedEntityGroup[i].Value);
                }
                linkedEntityGroup.Dispose();

                EntityManager.AddComponentData(EntityManager.CreateEntity(), new GameEnd() { TimeToEnd = 1 });

                EntityManager.AddComponent<StopMusicTag>(EntityManager.CreateEntity());
                EntityManager.AddComponentData(EntityManager.CreateEntity(), new SoundRequest { Value = SoundType.End });

                EntityManager.RemoveComponent<DestroyedTag>(entity);
            }).WithStructuralChanges().Run();
    }
}
