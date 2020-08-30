using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

public class RotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Rotation rotation, in Rotate rotate) =>
        {
            quaternion qx = quaternion.RotateX(rotate.radiansPerSecond.x * deltaTime);
            quaternion qy = quaternion.RotateY(rotate.radiansPerSecond.y * deltaTime);
            quaternion qz = quaternion.RotateZ(rotate.radiansPerSecond.z * deltaTime);

            rotation.Value = math.mul(math.normalize(rotation.Value), math.mul(qx, math.mul(qy, qz)));
        }).Run();
    }
}
