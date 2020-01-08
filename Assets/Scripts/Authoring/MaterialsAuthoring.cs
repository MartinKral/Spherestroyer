using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class MaterialsAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Material[] materials;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new RuntimeMaterialReferencesTag());
        dstManager.AddBuffer<RuntimeMaterialReference>(entity);
    }
}

[UpdateInGroup(typeof(GameObjectAfterConversionGroup))]
internal class AddUIMaterialsReference : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((MaterialsAuthoring materialsAuth) =>
        {
            var primaryEntity = GetPrimaryEntity(materialsAuth);
            var buffer = DstEntityManager.GetBuffer<RuntimeMaterialReference>(primaryEntity);

            foreach (var material in materialsAuth.materials)
            {
                buffer.Add(new RuntimeMaterialReference() { materialEntity = GetPrimaryEntity(material) });
            }
        });
    }
}