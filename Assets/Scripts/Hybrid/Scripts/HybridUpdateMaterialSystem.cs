#if !UNITY_DOTSPLAYER

using Unity.Entities;
using Unity.Rendering;

[UpdateAfter(typeof(ChangeMaterialIdSystem))]
public class HybridUpdateMaterialSystem : ComponentSystem
{
    private MaterialReferences materialReferences;

    protected override void OnCreate()
    {
        base.OnCreate();

        RequireSingletonForUpdate<MaterialReferences>();
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        var materialEntity = GetSingletonEntity<MaterialReferences>();
        materialReferences = EntityManager.GetSharedComponentData<MaterialReferences>(materialEntity);
    }

    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, RenderMesh renderMesh, ref MaterialId materialId, ref UpdateMaterialTag tag) =>
        {
            renderMesh.material = materialReferences.Materials[materialId.currentMaterialId];
            PostUpdateCommands.SetSharedComponent(entity, renderMesh);
        });
    }
}

#endif