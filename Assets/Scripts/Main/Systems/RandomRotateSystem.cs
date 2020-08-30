using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class RandomRotateSystem : SystemBase
{
    private Random randomGenerator;

    protected override void OnStartRunning()
    {
        randomGenerator = new Random();
        randomGenerator.InitState();
    }

    protected override void OnUpdate()
    {
        _ = randomGenerator.NextInt();

        Entities.ForEach((ref Entity entity, ref Rotate rotate, in RandomRotate randomRotate) =>
        {
            float3 randomRadiansPerSecond = new float3(
               randomGenerator.NextFloat(randomRotate.MinMaxX.x, randomRotate.MinMaxX.y),
               randomGenerator.NextFloat(randomRotate.MinMaxY.x, randomRotate.MinMaxY.y),
               randomGenerator.NextFloat(randomRotate.MinMaxZ.x, randomRotate.MinMaxZ.y)
               );

            rotate.radiansPerSecond = randomRadiansPerSecond;

            EntityManager.RemoveComponent<RandomRotate>(entity);
        }).WithStructuralChanges().Run();
    }
}
