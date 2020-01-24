using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Audio;

[AlwaysSynchronizeSystem]
public class TouchSymbolSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        Entities
            .WithAll<TouchSymbolTag, OnInputTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                ecb.AddComponent<StartGameTag>(ecb.CreateEntity());

                // OnInputTag needs to be removed manually, since this will get disabled immediately
                ecb.RemoveComponent<OnInputTag>(entity);
                ecb.AddComponent<Disabled>(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
        return default;
    }
}