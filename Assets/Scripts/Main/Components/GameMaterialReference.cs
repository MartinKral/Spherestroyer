using System;
using Unity.Entities;

public struct GameMaterialReference : IBufferElementData
{
    public Entity materialEntity;
}