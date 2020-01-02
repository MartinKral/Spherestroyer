using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Tiny;
using UnityEngine;
using Random = Unity.Mathematics.Random;
using Unity.Jobs;
using Unity.Collections;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class SpawningJobSystem : JobComponentSystem
{
    private IcospherePrefab planePrefab;
    private Random randomGenerator;

    private BeginSimulationEntityCommandBufferSystem ecbs;

    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<IcospherePrefab>();
        ecbs = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        randomGenerator = new Random((uint)UnityEngine.Random.Range(1, 1000));
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        Entity planeEntity = GetSingletonEntity<IcospherePrefab>();
        planePrefab = EntityManager.GetComponentData<IcospherePrefab>(planeEntity);
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var commandBuffer = ecbs.CreateCommandBuffer().ToConcurrent();
        var planePrefab = this.planePrefab;
        float randomFloat = this.randomGenerator.NextFloat();
        var jobHandle = Entities
            .WithAny<OnClickTag>()
            .ForEach((Entity entity, int entityInQueryIndex) =>
        {
            Entity newPlane = commandBuffer.Instantiate(entityInQueryIndex, planePrefab.prefab);
            var position = new float3(0, randomFloat, 0);
            commandBuffer.SetComponent(entityInQueryIndex, newPlane, new Translation { Value = position });
        }).Schedule(inputDeps);

        // This jobs needs to be finished before hitting the buffer system
        ecbs.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }
}