using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class ScaleAnimationSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new ScaleAnimationSystemJob()
        {
            DeltaTime = Time.DeltaTime
        }.Schedule(this, inputDeps);
        return jobHandle;
    }

    [BurstCompile]
    private struct ScaleAnimationSystemJob : IJobForEach<NonUniformScale, ScaleAnimation>
    {
        public float DeltaTime;

        public void Execute(ref NonUniformScale scale, ref ScaleAnimation scaleAnimation)
        {
            if (scaleAnimation.MaxScale <= scale.Value.x) scaleAnimation.IsIncreasing = false;
            if (scale.Value.x <= scaleAnimation.MinScale) scaleAnimation.IsIncreasing = true;

            float scalePerFrame = (scaleAnimation.MaxScale - scaleAnimation.MinScale) * DeltaTime / scaleAnimation.Duration;
            if (scaleAnimation.IsIncreasing)
            {
                scale.Value += scalePerFrame;
            }
            else
            {
                scale.Value -= scalePerFrame;
            }
        }
    }
}