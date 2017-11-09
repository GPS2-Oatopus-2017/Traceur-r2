using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointNodeScript))]
public class WaypointNodeEditor : Editor
{
	SerializedProperty data;
	static int currentDir = 0;
	bool nodeFoldout = true;
    
    public override void OnInspectorGUI()
	{
		serializedObject.Update();

        data = serializedObject.FindProperty ("data");

		nodeFoldout = EditorGUILayout.Foldout(nodeFoldout, "Directional Nodes");

		if(nodeFoldout)
		{
			EditorGUI.indentLevel++;

			EditorGUILayout.PropertyField(data.FindPropertyRelative("directionalNodes").GetArrayElementAtIndex(currentDir), new GUIContent(((Direction)currentDir).ToString() + " Node"));

			Color defaultColor = GUI.backgroundColor;

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			GUI.enabled = false;
			GUILayout.Button("◤\nNW\n", GUILayout.MaxWidth(50), GUILayout.MaxHeight(50));
			GUI.enabled = true;

			if(data.FindPropertyRelative("directionalNodes").GetArrayElementAtIndex((int)Direction.North).objectReferenceValue == null) GUI.backgroundColor = Color.red;
			else GUI.backgroundColor = Color.green;
			if(currentDir != (int)Direction.North) GUI.backgroundColor = (GUI.backgroundColor + Color.black) / 2;
			if(GUILayout.Button("▲\nNorth\n(Z+)", GUILayout.MaxWidth(50), GUILayout.MaxHeight(50))) currentDir = (int)Direction.North;
			GUI.backgroundColor = defaultColor;

			GUI.enabled = false;
			GUILayout.Button("◥\nNE\n", GUILayout.MaxWidth(50), GUILayout.MaxHeight(50));
			GUI.enabled = true;

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			if(data.FindPropertyRelative("directionalNodes").GetArrayElementAtIndex((int)Direction.West).objectReferenceValue == null) GUI.backgroundColor = Color.red;
			else GUI.backgroundColor = Color.green;
			if(currentDir != (int)Direction.West) GUI.backgroundColor = (GUI.backgroundColor + Color.black) / 2;
			if(GUILayout.Button("◀\nWest\n(X-)", GUILayout.MaxWidth(50), GUILayout.MaxHeight(50))) currentDir = (int)Direction.West;
			GUI.backgroundColor = defaultColor;

			GUI.backgroundColor = Color.yellow;
			GUI.enabled = false;
			GUILayout.Button("⦿\nNode\n", GUILayout.MaxWidth(50), GUILayout.MaxHeight(50));
			GUI.enabled = true;
			GUI.backgroundColor = defaultColor;

			if(data.FindPropertyRelative("directionalNodes").GetArrayElementAtIndex((int)Direction.East).objectReferenceValue == null) GUI.backgroundColor = Color.red;
			else GUI.backgroundColor = Color.green;
			if(currentDir != (int)Direction.East) GUI.backgroundColor = (GUI.backgroundColor + Color.black) / 2;
			if(GUILayout.Button("▶\nEast\n(X+)", GUILayout.MaxWidth(50), GUILayout.MaxHeight(50))) currentDir = (int)Direction.East;
			GUI.backgroundColor = defaultColor;

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			GUI.enabled = false;
			GUILayout.Button("◣\nSW\n", GUILayout.MaxWidth(50), GUILayout.MaxHeight(50));
			GUI.enabled = true;

			if(data.FindPropertyRelative("directionalNodes").GetArrayElementAtIndex((int)Direction.South).objectReferenceValue == null) GUI.backgroundColor = Color.red;
			else GUI.backgroundColor = Color.green;
			if(currentDir != (int)Direction.South) GUI.backgroundColor = (GUI.backgroundColor + Color.black) / 2;
			if(GUILayout.Button("▼\nSouth\n(Z-)", GUILayout.MaxWidth(50), GUILayout.MaxHeight(50))) currentDir = (int)Direction.South;
			GUI.backgroundColor = defaultColor;

			GUI.enabled = false;
			GUILayout.Button("◢\nSE\n", GUILayout.MaxWidth(50), GUILayout.MaxHeight(50));
			GUI.enabled = true;

			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();

			EditorGUI.indentLevel--;
		}

        EditorGUILayout.HelpBox("To reset this node, click on \"Reset Node\" instead of \"Reset\".", MessageType.Info);

		serializedObject.ApplyModifiedProperties();
	}
}
