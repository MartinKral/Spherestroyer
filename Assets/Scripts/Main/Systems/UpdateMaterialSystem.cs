using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Rendering;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(ChangeMaterialIdSystem))]
public class UpdateMaterialSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<MaterialReferencesTag>();
    }

    protected override void OnUpdate()
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
    }
}
