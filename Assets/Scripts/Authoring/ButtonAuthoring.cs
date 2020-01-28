using System;
using System.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class ButtonAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public ButtonType Type;

    [Header("Debug")]
    public float minX;

    public float minY;
    public float maxX;
    public float maxY;

    public Vector3 worldMinPoint;
    public Vector3 worldMaxPoint;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Button
        {
            MinMaxRect = new MinMaxRect(minX, minY, maxX, maxY),
            Type = Type
        });
    }

    public void Regenerate()
    {
        StartCoroutine(RegenerateAsync());
    }

    private IEnumerator RegenerateAsync()
    {
        EditorApplication.ExecuteMenuItem("Window/General/Game");
        yield return null;
        yield return null;

        CalculateValues();
    }

    private void CalculateValues()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        if (!Mathf.Approximately(screenWidth / screenHeight, 1920.0f / 1080.0f))
        {
            Debug.LogError("Editor view is not set to match the internal Tiny aspect ratio (1920:1080)! " +
                "Buttons will not work properly");
        }

        CalculateMinMaxPoints();

        var minScreenPoint = Camera.main.WorldToScreenPoint(worldMinPoint);
        var maxScreenPoint = Camera.main.WorldToScreenPoint(worldMaxPoint);

        minX = minScreenPoint.x / screenWidth;
        minY = minScreenPoint.y / screenHeight;
        maxX = maxScreenPoint.x / screenWidth;
        maxY = maxScreenPoint.y / screenHeight;
    }

    private void CalculateMinMaxPoints()
    {
        // TransformPoint takes the scale into account, so works even for 2:1 textures or similar
        Vector3 offset = new Vector3(0.5f, 0.5f, 0);
        worldMinPoint = transform.TransformPoint(-offset);
        worldMaxPoint = transform.TransformPoint(offset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(worldMinPoint.x, worldMinPoint.y, worldMinPoint.z), new Vector3(worldMaxPoint.x, worldMinPoint.y, worldMinPoint.z));
        Gizmos.DrawLine(new Vector3(worldMaxPoint.x, worldMinPoint.y, worldMinPoint.z), new Vector3(worldMaxPoint.x, worldMaxPoint.y, worldMaxPoint.z));
        Gizmos.DrawLine(new Vector3(worldMaxPoint.x, worldMaxPoint.y, worldMaxPoint.z), new Vector3(worldMinPoint.x, worldMaxPoint.y, worldMaxPoint.z));
        Gizmos.DrawLine(new Vector3(worldMinPoint.x, worldMaxPoint.y, worldMaxPoint.z), new Vector3(worldMinPoint.x, worldMinPoint.y, worldMinPoint.z));

        Gizmos.DrawLine(new Vector3(worldMinPoint.x, worldMinPoint.y, worldMinPoint.z), new Vector3(worldMaxPoint.x, worldMaxPoint.y, worldMaxPoint.z));
        Gizmos.DrawLine(new Vector3(worldMaxPoint.x, worldMinPoint.y, worldMinPoint.z), new Vector3(worldMinPoint.x, worldMaxPoint.y, worldMaxPoint.z));
    }
}