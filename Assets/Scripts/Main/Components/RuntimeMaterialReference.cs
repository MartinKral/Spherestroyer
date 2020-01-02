using System;
using Unity.Entities;

public struct RuntimeMaterialReference : IBufferElementData
{
    public Entity materialEntity;
}