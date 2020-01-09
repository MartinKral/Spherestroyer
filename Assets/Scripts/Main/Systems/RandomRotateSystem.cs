using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class RandomRotateSystem : JobComponentSystem
{
    private Random randomGenerator;

    private EndSimulationEntityCommandBufferSystem ecbs;

    protected override void OnCreate()
    {
        ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnStartRunning()
    {
        randomGenerator = new Random((uint)(Time.DeltaTime * 1000));
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        _ = randomGenerator.NextInt();

        var jobHandle = new RandomRotateSystemJob()
        {
            randomGenerator = randomGenerator,
            ecb = ecbs.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);
        return jobHandle;
    }

    [BurstCompile]
    private struct RandomRotateSystemJob : IJobForEachWithEntity<Rotate, RandomRotate>
    {
        public Random randomGenerator;
        public EntityCommandBuffer.Concurrent ecb;

        public void Execute(Entity entity, int index, ref Rotate rotate, ref RandomRotate randomRotate)
        {
            float3 randomRadiansPerSecond = new float3(
                randomGenerator.NextFloat(randomRotate.MinMaxX.x, randomRotate.MinMaxX.y),
                randomGenerator.NextFloat(randomRotate.MinMaxY.x, randomRotate.MinMaxY.y),
                randomGenerator.NextFloat(randomRotate.MinMaxZ.x, randomRotate.MinMaxZ.y)
                );

            rotate.radiansPerSecond = randomRadiansPerSecond;

            ecb.RemoveComponent<RandomRotate>(index, entity);
        }
    }
}