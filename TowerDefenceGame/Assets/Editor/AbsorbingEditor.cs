using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Absorbing))]
public class AbsorbingEditor : Editor {
	
	public override void OnInspectorGUI()
	{
		Absorbing myTarget = (Absorbing)target;
		
		//myTarget.anim = EditorGUILayout.ObjectField("Anim", myTarget.anim, typeof(Animation)) as Animation;
		myTarget.spawnPoint = EditorGUILayout.ObjectField ("Spawn Point", myTarget.spawnPoint, typeof(GameObject)) as GameObject;
		
	}
}