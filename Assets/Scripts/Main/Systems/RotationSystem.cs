using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Tiny;
using UnityEngine;
using Unity.Jobs;

public class RotationSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RotateJob
        {
            deltaTime = (float)Time.DeltaTime
        };
        return job.Schedule(this, inputDeps);
    }

    [BurstCompile]
    public struct RotateJob : IJobForEach<RotateY, Rotation>
    {
        public float deltaTime;

        public void Execute(ref RotateY rotate, ref Rotation rotation)
        {
            rotation.Value = math.mul(
                     math.normalize(rotation.Value),
                     quaternion.AxisAngle(math.up(), rotate.RadiansPerSecond * deltaTime));
        }
    }
}