using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Tiny.Audio;

[AlwaysSynchronizeSystem]
public class InputSoundSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        Entities
            .WithAll<OnInputTag>()
            .ForEach((Entity entity, in SoundManager soundManager) =>
            {
                ecb.AddComponent<AudioSourceStart>(soundManager.InputAS);
                ecb.RemoveComponent<OnInputTag>(entity);
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
        return default;
    }
}