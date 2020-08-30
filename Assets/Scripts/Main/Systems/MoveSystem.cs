using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class MoveSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<GameState>();
    }

    protected override void OnUpdate()
    {
        var gameDataEntity = GetSingletonEntity<GameState>();
        var gameData = EntityManager.GetComponentData<GameState>(gameDataEntity);

        if (!gameData.IsGameActive) return;

        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Translation translation, in Move move) =>
        {
            translation.Value.x += move.speedX * deltaTime;
            translation.Value.y += move.speedY * deltaTime;
            translation.Value.z += move.speedZ * deltaTime;
        }).Run();
    }
}
