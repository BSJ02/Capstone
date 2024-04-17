using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    private MonsterMove monsterMove;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapGenerator mapGenerator = (MapGenerator)target;

        if (GUILayout.Button("Bake Map"))
        {
            mapGenerator.CreateMap(mapGenerator.garo, mapGenerator.sero);
        }
    }

    void OnSceneGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;

        if (mapGenerator == null)
            return;

        Handles.BeginGUI();

        GUILayout.BeginArea(new Rect(10, 10, 100, 100));
        GUILayout.Label("Map Size");
        mapGenerator.garo = EditorGUILayout.IntField("Width", mapGenerator.garo);
        mapGenerator.sero = EditorGUILayout.IntField("Height", mapGenerator.sero);
        GUILayout.EndArea();

        Handles.EndGUI();
    }
}