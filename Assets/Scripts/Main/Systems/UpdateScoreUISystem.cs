using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Tiny.Rendering;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class UpdateScoreUISystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<MaterialReferencesTag>();
    }

    protected override void OnUpdate()

    {
        var materialReferencesEntity = GetSingletonEntity<MaterialReferencesTag>();

        var nBuffer = EntityManager.GetBuffer<UIMaterialReference>(materialReferencesEntity);
        var materials = nBuffer.ToNativeArray(Allocator.TempJob);

        Entities
            .WithAll<ActivatedTag>()
            .ForEach((Entity entity, ref MeshRenderer meshRenderer, in ScorePart scorePart) =>
            {
                int digit = (int)((float)scorePart.TargetScore / scorePart.Divisor);
                meshRenderer.material = materials[digit % 10].materialEntity;
                EntityManager.RemoveComponent<ActivatedTag>(entity);
            }).WithStructuralChanges().Run();

        materials.Dispose();
    }
}
