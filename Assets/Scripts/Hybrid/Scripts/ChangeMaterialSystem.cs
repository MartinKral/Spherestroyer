#if !UNITY_DOTSPLAYER

using Unity.Entities;
using Unity.Rendering;

public class ChangeMaterialSystem : ComponentSystem
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<MaterialReferences>();
    }

    protected override void OnUpdate()
    {
        var materialEntity = GetSingletonEntity<MaterialReferences>();
        var materialReferences = EntityManager.GetSharedComponentData<MaterialReferences>(materialEntity);

        Entities.ForEach((Entity entity, RenderMesh renderMesh, ref ChangableMaterialTag tag) =>
        {
            renderMesh.material = materialReferences.Materials[0];
            PostUpdateCommands.SetSharedComponent(entity, renderMesh);
        });
    }
}

#endif