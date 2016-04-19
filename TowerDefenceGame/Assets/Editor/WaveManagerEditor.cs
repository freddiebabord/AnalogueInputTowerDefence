using UnityEngine;  
using UnityEditor;  
using UnityEditorInternal;

[CustomEditor(typeof(WaveManager))]
public class WaveManagerEditor : Editor {  

	private ReorderableList list;
	
	private void OnEnable() {
		list = new ReorderableList(serializedObject, 
		                           serializedObject.FindProperty("Waves"), 
		                           true, true, true, true);

		list.drawElementCallback =  
		(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;

			GUIContent typeContent = new GUIContent("", "The difficulty of the enemy. Note: Not linked to anything yet.");
			Rect typeRect = new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight);
			EditorGUI.PropertyField( typeRect, element.FindPropertyRelative("Type"), GUIContent.none);
			EditorGUI.LabelField(typeRect, typeContent);

			GUIContent countContent = new GUIContent("", "The amount of enemies to spawn this wave");
			Rect contRect = new Rect(rect.x + 65, rect.y, 35, EditorGUIUtility.singleLineHeight);
			EditorGUI.PropertyField(contRect, element.FindPropertyRelative("Count"), GUIContent.none);
			EditorGUI.LabelField(contRect, countContent);

			GUIContent prefabContent = new GUIContent("", "The gameobject of the enemy");
			Rect prefabRect = new Rect(rect.x + 105, rect.y, rect.width - 75 - 65, EditorGUIUtility.singleLineHeight);
			EditorGUI.PropertyField( prefabRect, element.FindPropertyRelative("Prefab"), GUIContent.none);
			EditorGUI.LabelField(prefabRect, prefabContent);

			GUIContent eowTimerConent = new GUIContent("", "The amount of time to wait before spawning the next wave.");
			Rect eowRect = new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight);
			EditorGUI.PropertyField( eowRect, element.FindPropertyRelative("waveEndTimer"), GUIContent.none);
			EditorGUI.LabelField(eowRect, eowTimerConent);
		};

		list.drawHeaderCallback = (Rect rect) => {  
			EditorGUI.LabelField(rect, "Wave Definitions");
		};
	}
	
	public override void OnInspectorGUI() {
		DrawDefaultInspector ();
		GUILayout.Space (10);
		serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
		GUILayout.Space (10);
	}
}
