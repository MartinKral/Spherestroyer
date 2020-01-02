using Unity.Entities;
using Unity.Tiny.Input;

[AlwaysUpdateSystem]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameInputSystem : ComponentSystem
{
    private InputSystem Input;

    private EntityQuery inputEntityQuery;

    private BeginSimulationEntityCommandBufferSystem beginInitECB;
    private EndSimulationEntityCommandBufferSystem endInitECB;

    protected override void OnCreate()
    {
        Input = World.GetExistingSystem<InputSystem>();

        inputEntityQuery = GetEntityQuery(typeof(InputTag));

        beginInitECB = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        endInitECB = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        EntityCommandBuffer beginBuffer = beginInitECB.CreateCommandBuffer();
        EntityCommandBuffer endBuffer = endInitECB.CreateCommandBuffer();

        beginBuffer.AddComponent(inputEntityQuery, typeof(OnClickTag));
        endBuffer.RemoveComponent(inputEntityQuery, typeof(OnClickTag));
    }
}