using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

/*
public class ChangeTypeSystem : JobComponentSystem
{
    private SpikeMaterial spikeMaterial;

    protected override void OnStartRunning()
    {
        base.OnStartRunning();
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new ChangeTypeSystemJob();

        // Assign values to the fields on your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     job.deltaTime = UnityEngine.Time.deltaTime;

        // Now that the job is set up, schedule it to be run.
        return job.Schedule(this, inputDependencies);
    }

    [BurstCompile]
    [RequireComponentTag(typeof(SpikeTag))]
    private struct ChangeTypeSystemJob : IJobForEach<MeshRenderer>
    {
        public SpikeMaterial spikeMaterial;

        public void Execute(ref MeshRenderer meshRenderer)
        {
            //meshRenderer.material = spikeMaterial;
        }
    }
}*/