using Unity.Entities;
using Unity.Jobs;

public class ChangeMaterialIdSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<OnInputTag>()
            .ForEach((ref Entity entity, ref MaterialId materialId) =>
            {
                materialId.currentMaterialId = ++materialId.currentMaterialId % 3;
                EntityManager.AddComponent<UpdateMaterialTag>(entity);
            }).WithStructuralChanges().Run();
    }
}
