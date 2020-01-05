using Unity.Collections;
using Unity.Entities;
using Unity.Tiny.Rendering;

[UpdateAfter(typeof(ChangeMaterialIdSystem))]
public class UpdateMaterialSystem : ComponentSystem
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<RuntimeMaterialReferencesTag>();
    }

    protected override void OnUpdate()
    {
        var materialReferencesEntity = GetSingletonEntity<RuntimeMaterialReferencesTag>();

        var nBuffer = EntityManager.GetBuffer<RuntimeMaterialReference>(materialReferencesEntity);
        var materials = nBuffer.ToNativeArray(Allocator.Temp);

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        Entities.ForEach((Entity entity, ref MeshRenderer meshRenderer, ref MaterialId materialId, ref UpdateMaterialTag updateMaterialTag) =>
        {
            meshRenderer.material = materials[materialId.currentMaterialId].materialEntity;
            //LitMaterial newMaterial = EntityManager.GetComponentData<LitMaterial>(materials[materialId.currentMaterialId].materialEntity);
            // ecb.SetComponent(meshRenderer.material, newMaterial);
        });

        ecb.Playback(EntityManager);
        ecb.Dispose();

        materials.Dispose();
    }
}