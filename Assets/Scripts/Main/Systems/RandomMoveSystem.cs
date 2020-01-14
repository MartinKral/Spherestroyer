using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class RandomMoveSystem : JobComponentSystem
{
    private Random randomGenerator;

    private EndSimulationEntityCommandBufferSystem ecbs;

    protected override void OnCreate()
    {
        ecbs = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnStartRunning()
    {
        randomGenerator = new Random();
        randomGenerator.InitState();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        _ = randomGenerator.NextInt();

        var jobHandle = new RandomMoveSystemJob()
        {
            randomGenerator = randomGenerator,
            ecb = ecbs.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);
        return jobHandle;
    }

    [BurstCompile]
    private struct RandomMoveSystemJob : IJobForEachWithEntity<Move, RandomMove>
    {
        public Random randomGenerator;
        public EntityCommandBuffer.Concurrent ecb;

        public void Execute(Entity entity, int index, ref Move move, ref RandomMove randomMove)
        {
            move.speedX = randomGenerator.NextFloat(randomMove.MinMaxX.x, randomMove.MinMaxX.y);
            move.speedY = randomGenerator.NextFloat(randomMove.MinMaxY.x, randomMove.MinMaxY.y);
            move.speedZ = randomGenerator.NextFloat(randomMove.MinMaxZ.x, randomMove.MinMaxZ.y);

            ecb.RemoveComponent<RandomMove>(index, entity);
        }
    }
}