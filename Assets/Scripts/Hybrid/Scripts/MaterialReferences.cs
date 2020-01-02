using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct MaterialReferences : ISharedComponentData, IEquatable<MaterialReferences>
{
    public Material[] Materials;

    public bool Equals(MaterialReferences other)
    {
        if (Materials.Length != other.Materials.Length) return false;

        for (int i = 0; i < Materials.Length; i++)
        {
            if (Materials[i] != other.Materials[i]) return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        return Materials[0].GetHashCode();
    }
}