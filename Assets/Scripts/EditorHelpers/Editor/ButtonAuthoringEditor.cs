using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(ButtonAuthoring))]
public class ButtonAuthoringEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Always Regenerate after changing button position / scale.", MessageType.Info);
        EditorGUILayout.HelpBox(
            "You can check if everything is set up correctly" +
            " in scene view (red debug lines over textures)", MessageType.Info);
        DrawDefaultInspector();

        ButtonAuthoring buttonAuthoring = (ButtonAuthoring)target;
        if (GUILayout.Button("Regenerate values"))
        {
            buttonAuthoring.Regenerate();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(buttonAuthoring);
            EditorSceneManager.MarkSceneDirty(buttonAuthoring.gameObject.scene);
        }
    }
}