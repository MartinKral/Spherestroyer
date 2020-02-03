using Unity.Entities;
using Unity.Transforms;
using Unity.Jobs;

[AlwaysUpdateSystem]
[AlwaysSynchronizeSystem]
public class InputSoundSystem : JobComponentSystem
{
    private InputWrapperSystem inputSystem;

    protected override void OnCreate()
    {
        inputSystem = World.GetOrCreateSystem<InputWrapperSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        if (!inputSystem.IsTouchOrButtonDown()) return default;

        EntityManager.AddComponentData(EntityManager.CreateEntity(), new SoundRequest { Value = SoundType.Input });

        return default;
    }
}