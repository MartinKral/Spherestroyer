using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny;
using Unity.Transforms;

public class ParticleShardDestroySystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<ParticleShardTag>()
            .ForEach((ref Entity entity, in Translation translation) =>
            {
                if (-10 < translation.Value.y) return;
                EntityManager.DestroyEntity(entity);
            }).WithStructuralChanges().Run();
    }
}
