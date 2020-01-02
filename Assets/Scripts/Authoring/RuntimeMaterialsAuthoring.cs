using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Tiny.Rendering;
using UnityEngine;

[DisallowMultipleComponent]
public class RuntimeMaterialsAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Material material1;
    public Material material2;
    public Material material3;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new RuntimeMaterialReferencesTag());
        dstManager.AddBuffer<RuntimeMaterialReference>(entity);

        _ = CreateMaterialEntity(dstManager, conversionSystem, this.material1);
        _ = CreateMaterialEntity(dstManager, conversionSystem, this.material2);
        _ = CreateMaterialEntity(dstManager, conversionSystem, this.material3);
    }

    private Entity CreateMaterialEntity(EntityManager dstManager, GameObjectConversionSystem conversionSystem, Material material)
    {
        Entity primaryEntity = conversionSystem.GetPrimaryEntity(material);

        Entity materialEntity = conversionSystem.CreateAdditionalEntity(material);
        var mat = dstManager.GetComponentData<LitMaterial>(primaryEntity);
        dstManager.AddComponentData<LitMaterial>(materialEntity, mat);
        dstManager.AddComponent<DynamicMaterial>(materialEntity);

        return materialEntity;
    }
}

[UpdateInGroup(typeof(GameObjectAfterConversionGroup))]
internal class AddUIMaterialsReference : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((RuntimeMaterialsAuthoring uNum) =>
        {
            var primaryEntity = GetPrimaryEntity(uNum);
            var buffer = DstEntityManager.GetBuffer<RuntimeMaterialReference>(primaryEntity);

            var entities = GetEntities(uNum.material1);
            if (entities.MoveNext() && entities.MoveNext())
                buffer.Add(new RuntimeMaterialReference() { materialEntity = entities.Current });
        });
    }
}

[UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
internal class DeclareNumberMaterials : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((RuntimeMaterialsAuthoring uNum) =>
        {
            DeclareReferencedAsset(uNum.material1);
            DeclareReferencedAsset(uNum.material2);
            DeclareReferencedAsset(uNum.material3);
        });
    }
}