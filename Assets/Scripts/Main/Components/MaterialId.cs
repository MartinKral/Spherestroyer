using Unity.Entities;

[GenerateAuthoringComponent]
public struct MaterialId : IComponentData
{
    public int currentMaterialId;
}