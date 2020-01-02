using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Tiny.Rendering;
using UnityEngine;

[DisallowMultipleComponent]
public class SpikeMaterialAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Material material;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        Entity materialEntity = conversionSystem.GetPrimaryEntity(material);
        Debug.Log($"Material entity {materialEntity}, material {material}");

        Entity additionalEntity = conversionSystem.CreateAdditionalEntity(material);
        var mat = dstManager.GetComponentData<LitMaterial>(materialEntity);
        dstManager.AddComponentData<LitMaterial>(additionalEntity, mat);
        dstManager.AddComponent<DynamicMaterial>(additionalEntity);

        // dstManager.AddComponent<SpikeMaterial>(entity);

        dstManager.AddComponentData(entity, new SpikeMaterial { material = additionalEntity });
    }
}

[UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
internal class DeclareNumberMaterials : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((SpikeMaterialAuthoring uNum) =>
        {
            DeclareReferencedAsset(uNum.material);
        });
    }
}