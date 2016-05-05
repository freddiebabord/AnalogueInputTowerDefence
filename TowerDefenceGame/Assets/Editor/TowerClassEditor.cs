using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TowerClass))]
public class TowerClassEditor : Editor {

	public override void OnInspectorGUI()
	{
		TowerClass myTarget = (TowerClass)target;

		myTarget.bullet = EditorGUILayout.ObjectField("Bullet", myTarget.bullet, typeof(GameObject)) as GameObject;
		myTarget.spawnPoint = EditorGUILayout.ObjectField ("Spawn Point", myTarget.spawnPoint, typeof(GameObject)) as GameObject;

        DrawDefaultInspector();

	}
}
