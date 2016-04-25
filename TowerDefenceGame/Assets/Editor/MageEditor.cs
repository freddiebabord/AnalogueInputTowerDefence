using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Mage))]
public class MageEditor : Editor {
	
	public override void OnInspectorGUI()
	{
		Mage myTarget = (Mage)target;
		
		//myTarget.bullet = EditorGUILayout.ObjectField("Bullet", myTarget.bullet, typeof(GameObject)) as GameObject;
		myTarget.spawnPoint = EditorGUILayout.ObjectField ("Spawn Point", myTarget.spawnPoint, typeof(GameObject)) as GameObject;
		
	}
}
