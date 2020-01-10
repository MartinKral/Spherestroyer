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
    private struct ScaleAnimationSystemJob : IJobForEach<Scale, ScaleAnimation>
    {
        public float DeltaTime;

        public void Execute(ref Scale scale, ref ScaleAnimation scaleAnimation)
        {
            if ((scaleAnimation.MaxScale <= scale.Value) && (scaleAnimation.IsIncreasing)) scaleAnimation.IsIncreasing = false;
            if ((scale.Value <= scaleAnimation.MinScale) && (!scaleAnimation.IsIncreasing)) scaleAnimation.IsIncreasing = true;

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