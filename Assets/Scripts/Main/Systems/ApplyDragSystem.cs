using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class ApplyDragSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Move move, in MoveDrag drag) =>
        {
            move.speedX *= 1 - drag.Value * deltaTime;
            move.speedY *= 1 - drag.Value * deltaTime;
            move.speedZ *= 1 - drag.Value * deltaTime;
        }).Run();
    }
}
