using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Tiny.Rendering;
using UnityEngine;

[DisallowMultipleComponent]
public class MaterialsAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Material[] materials;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new RuntimeMaterialReferencesTag());
        dstManager.AddBuffer<RuntimeMaterialReference>(entity);

        foreach (var material in materials)
        {
            CreateMaterialEntity(dstManager, conversionSystem, material);
        }
    }

    private void CreateMaterialEntity(EntityManager dstManager, GameObjectConversionSystem conversionSystem, Material material)
    {
        Entity primaryEntity = conversionSystem.GetPrimaryEntity(material);

        Entity materialEntity = conversionSystem.CreateAdditionalEntity(material);
        var mat = dstManager.GetComponentData<LitMaterial>(primaryEntity);
        dstManager.AddComponentData<LitMaterial>(materialEntity, mat);
        dstManager.AddComponent<DynamicMaterial>(materialEntity);
    }
}

[UpdateInGroup(typeof(GameObjectAfterConversionGroup))]
internal class AddUIMaterialsReference : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((MaterialsAuthoring uNum) =>
        {
            var primaryEntity = GetPrimaryEntity(uNum);
            var buffer = DstEntityManager.GetBuffer<RuntimeMaterialReference>(primaryEntity);

            foreach (var material in uNum.materials)
            {
                buffer.Add(new RuntimeMaterialReference() { materialEntity = GetMaterialEntity(material) });
            }
        });
    }

    private Entity GetMaterialEntity(Material material)
    {
        var entities = GetEntities(material);
        if (entities.MoveNext() && entities.MoveNext())
        {
            return entities.Current;
        }
        else
        {
            throw new Exception($"{material} does not have second entity. Is it created?");
        }
    }
}

[UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
internal class DeclareNumberMaterials : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((MaterialsAuthoring uNum) =>
        {
            foreach (var material in uNum.materials)
            {
                DeclareReferencedAsset(material);
            }
        });
    }
}