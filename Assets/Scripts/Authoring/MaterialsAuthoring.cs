using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class MaterialsAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Material[] gameMaterials;
    public Material[] uiMaterials;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new MaterialReferencesTag());
        dstManager.AddBuffer<GameMaterialReference>(entity);
        dstManager.AddBuffer<UIMaterialReference>(entity);
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

            var gameMaterialsBuffer = DstEntityManager.GetBuffer<GameMaterialReference>(primaryEntity);
            foreach (var material in materialsAuth.gameMaterials)
            {
                gameMaterialsBuffer.Add(new GameMaterialReference() { materialEntity = GetPrimaryEntity(material) });
            }

            var uiMaterialsBuffer = DstEntityManager.GetBuffer<UIMaterialReference>(primaryEntity);
            foreach (var material in materialsAuth.uiMaterials)
            {
                uiMaterialsBuffer.Add(new UIMaterialReference() { materialEntity = GetPrimaryEntity(material) });
            }
        });
    }
}

[UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
internal class DeclareNumberMaterials : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((MaterialsAuthoring uNum) =>
        {
            foreach (Material mat in uNum.uiMaterials)
            {
                DeclareReferencedAsset(mat);
            }

            /*foreach (Material mat in uNum.gameMaterials)
            {
                DeclareReferencedAsset(mat);
            }*/
        });
    }
}