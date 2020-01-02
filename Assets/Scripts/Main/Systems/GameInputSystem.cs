using Unity.Entities;
using Unity.Tiny.Input;

[AlwaysUpdateSystem]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameInputSystem : ComponentSystem
{
    private InputSystem Input;

    private EntityQuery inputEntityQuery;

    private BeginSimulationEntityCommandBufferSystem beginInitECBS;
    private EndSimulationEntityCommandBufferSystem endInitECBS;

    protected override void OnCreate()
    {
        Input = World.GetExistingSystem<InputSystem>();

        inputEntityQuery = GetEntityQuery(typeof(InputTag));

        beginInitECBS = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        endInitECBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        EntityCommandBuffer beginBuffer = beginInitECBS.CreateCommandBuffer();
        EntityCommandBuffer endBuffer = endInitECBS.CreateCommandBuffer();

        beginBuffer.AddComponent(inputEntityQuery, typeof(OnClickTag));
        endBuffer.RemoveComponent(inputEntityQuery, typeof(OnClickTag));
    }
}