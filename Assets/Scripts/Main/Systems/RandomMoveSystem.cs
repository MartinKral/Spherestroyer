using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class RandomMoveSystem : SystemBase
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

        Entities.ForEach((ref Entity entity, ref Move move, in RandomMove randomMove) =>
        {
            move.speedX = randomGenerator.NextFloat(randomMove.MinMaxX.x, randomMove.MinMaxX.y);
            move.speedY = randomGenerator.NextFloat(randomMove.MinMaxY.x, randomMove.MinMaxY.y);
            move.speedZ = randomGenerator.NextFloat(randomMove.MinMaxZ.x, randomMove.MinMaxZ.y);

            EntityManager.RemoveComponent<RandomMove>(entity);
        }).WithStructuralChanges().Run();
    }
}
