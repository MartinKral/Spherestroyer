using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class ApplyDragSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new ApplyDragSystemJob()
        {
            deltaTime = Time.DeltaTime
        }.Schedule(this, inputDeps);
        return jobHandle;
    }

    [BurstCompile]
    private struct ApplyDragSystemJob : IJobForEach<MoveDrag, Move>
    {
        public float deltaTime;

        public void Execute(ref MoveDrag drag, ref Move move)
        {
            move.speedX *= 1 - drag.Value * deltaTime;
            move.speedY *= 1 - drag.Value * deltaTime;
            move.speedZ *= 1 - drag.Value * deltaTime;
        }
    }
}