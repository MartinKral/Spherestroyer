using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateBefore(typeof(ApplyDragSystem))]
public class ApplyGravitySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new ApplyGravitySystemJob()
        {
            deltaTime = Time.DeltaTime
        }.Schedule(this, inputDeps);
        return jobHandle;
    }

    [BurstCompile]
    [RequireComponentTag(typeof(ApplyGravityTag))]
    private struct ApplyGravitySystemJob : IJobForEach<Move>
    {
        public float deltaTime;

        public void Execute(ref Move move)
        {
            move.speedY -= 4f * deltaTime;
        }
    }
}