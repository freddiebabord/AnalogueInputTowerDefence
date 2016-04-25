using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Absorbing))]
public class AbsorbingEditor : Editor {
	
	public override void OnInspectorGUI()
	{
		Absorbing myTarget = (Absorbing)target;
		
		//myTarget.rail = EditorGUILayout.ObjectField("Rail", myTarget.rail, typeof(RailManager)) as RailManager;
		myTarget.spawnPoint = EditorGUILayout.ObjectField ("Spawn Point", myTarget.spawnPoint, typeof(GameObject)) as GameObject;
		
	}
}