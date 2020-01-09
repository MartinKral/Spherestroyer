using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

public class RotationSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RotateJob
        {
            deltaTime = Time.DeltaTime
        };
        return job.Schedule(this, inputDeps);
    }

    [BurstCompile]
    public struct RotateJob : IJobForEach<Rotate, Rotation>
    {
        public float deltaTime;

        public void Execute(ref Rotate rotate, ref Rotation rotation)
        {
            quaternion qx = quaternion.RotateX(rotate.radiansPerSecond.x * deltaTime);
            quaternion qy = quaternion.RotateY(rotate.radiansPerSecond.y * deltaTime);
            quaternion qz = quaternion.RotateZ(rotate.radiansPerSecond.z * deltaTime);

            rotation.Value = math.mul(math.normalize(rotation.Value), math.mul(qx, math.mul(qy, qz)));
        }
    }
}