using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class ShakeSystem : JobComponentSystem
{
    private Random randomGenerator;

    protected override void OnStartRunning()
    {
        randomGenerator = new Random((uint)(Time.DeltaTime * 1000));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        Entities
            .WithAll<ActivatedTag>()
            .WithoutBurst()
            .ForEach((Entity entity, ref Translation translation, ref Shake screenShake) =>
            {
                if (screenShake.Duration == screenShake.DurationLeft) screenShake.DefaultTranslation = translation.Value;

                if (0 < screenShake.DurationLeft)
                {
                    screenShake.DurationLeft -= Time.DeltaTime;

                    float3 randomShake = randomGenerator.NextFloat3(-screenShake.Intensity, screenShake.Intensity);
                    translation.Value = screenShake.DefaultTranslation + randomShake;
                }
                else
                {
                    translation.Value = screenShake.DefaultTranslation;
                    screenShake.DurationLeft = screenShake.Duration;
                    ecb.RemoveComponent<ActivatedTag>(entity);
                }
            }).Run();

        ecb.Playback(EntityManager);
        ecb.Dispose();
        return default;
    }
}