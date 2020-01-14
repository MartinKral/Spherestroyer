using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Audio;
using Unity.Tiny.Input;

[AlwaysUpdateSystem]
[AlwaysSynchronizeSystem]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public class GameInputSystem : JobComponentSystem
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

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!Input.GetMouseButtonDown(0)) return default;

        EntityCommandBuffer beginBuffer = beginInitECBS.CreateCommandBuffer();
        EntityCommandBuffer endBuffer = endInitECBS.CreateCommandBuffer();

        beginBuffer.AddComponent(inputEntityQuery, typeof(OnInputTag));
        endBuffer.RemoveComponent(inputEntityQuery, typeof(OnInputTag));

        return default;
    }
}