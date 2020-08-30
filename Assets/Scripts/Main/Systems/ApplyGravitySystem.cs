using Unity.Entities;
using Unity.Jobs;

[UpdateBefore(typeof(ApplyDragSystem))]
public class ApplyGravitySystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities
            .WithAll<ApplyGravityTag>()
            .ForEach((ref Move move) =>
        {
            move.speedY -= 4f * deltaTime;
        }).Run();
    }
}
