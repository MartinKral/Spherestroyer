using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Rendering;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(ChangeMaterialIdSystem))]
public class UpdateMaterialSystem : JobComponentSystem
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<MaterialReferencesTag>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var materialReferencesEntity = GetSingletonEntity<MaterialReferencesTag>();

        var nBuffer = EntityManager.GetBuffer<GameMaterialReference>(materialReferencesEntity);
        var materials = nBuffer.ToNativeArray(Allocator.TempJob);

        Entities
            .WithAll<UpdateMaterialTag>()
            .ForEach((Entity entity, ref MeshRenderer meshRenderer, in MaterialId materialId) =>
        {
            meshRenderer.material = materials[materialId.currentMaterialId].materialEntity;
        }).Run();

        materials.Dispose();
        return default;
    }
}