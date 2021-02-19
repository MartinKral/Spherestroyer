using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameInputSystem : SystemBase
{
    private InputWrapperSystem inputSystem;

    private EntityQuery inputEntityQuery;

    private BeginSimulationEntityCommandBufferSystem beginInitECBS;
    private EndSimulationEntityCommandBufferSystem endInitECBS;

    protected override void OnCreate()
    {
        inputSystem = World.GetOrCreateSystem<InputWrapperSystem>();

        inputEntityQuery = GetEntityQuery(typeof(InputTag));

        beginInitECBS = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        endInitECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        if (!inputSystem.IsTouchOrButtonDown()) return;

        EntityCommandBuffer beginBuffer = beginInitECBS.CreateCommandBuffer();
        EntityCommandBuffer endBuffer = endInitECBS.CreateCommandBuffer();

        beginBuffer.AddComponent(inputEntityQuery, typeof(OnInputTag));
        endBuffer.RemoveComponent(inputEntityQuery, typeof(OnInputTag));
    }
}
