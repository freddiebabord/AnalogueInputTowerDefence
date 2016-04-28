using UnityEngine;  
using UnityEditor;  
using UnityEditorInternal;
using System.IO;

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

        list.onSelectCallback = (ReorderableList l) =>
        {
            var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("Prefab").objectReferenceValue as GameObject;
            if (prefab)
                EditorGUIUtility.PingObject(prefab.gameObject);
        };

        list.onRemoveCallback = (ReorderableList l) =>
        {
            //if (EditorUtility.DisplayDialog("Warning!",
            //    "Are you sure you want to delete the wave?", "Yes", "No"))
            //{
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            //}
        };

        list.onAddCallback = (ReorderableList l) =>
        {
            var index = l.serializedProperty.arraySize;
            l.serializedProperty.arraySize++;
            l.index = index;
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Type").enumValueIndex = 0;
            element.FindPropertyRelative("Count").intValue = 20;
            element.FindPropertyRelative("Prefab").objectReferenceValue =
                    AssetDatabase.LoadAssetAtPath("Assets/Resources/Prefabs/AI/Basic/BlueAI.prefab",
                    typeof(GameObject)) as GameObject;
        };

        list.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
        {
            var menu = new GenericMenu();
            var guids = AssetDatabase.FindAssets("", new[] { "Assets/Resources/Prefabs/AI/Basic" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                menu.AddItem(new GUIContent("Basic/" + Path.GetFileNameWithoutExtension(path)),
                false, clickHandler,
                new WaveCreationParams() { Type = MobWave.WaveType.Basic, Path = path });
            }
            guids = AssetDatabase.FindAssets("", new[] { "Assets/Resources/Prefabs/AI/Medium" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                menu.AddItem(new GUIContent("Medium/" + Path.GetFileNameWithoutExtension(path)),
                false, clickHandler,
                new WaveCreationParams() { Type = MobWave.WaveType.Medium, Path = path });
            }
            guids = AssetDatabase.FindAssets("", new[] { "Assets/Resources/Prefabs/AI/Heavy" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                menu.AddItem(new GUIContent("Heavy/" + Path.GetFileNameWithoutExtension(path)),
                false, clickHandler,
                new WaveCreationParams() { Type = MobWave.WaveType.Heavy, Path = path });
            }
            guids = AssetDatabase.FindAssets("", new[] { "Assets/Resources/Prefabs/AI/Bosses" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                menu.AddItem(new GUIContent("Bosses/" + Path.GetFileNameWithoutExtension(path)),
                false, clickHandler,
                new WaveCreationParams() { Type = MobWave.WaveType.Boss, Path = path });
            }
            menu.ShowAsContext();
        };
	}

    private void clickHandler(object target)
    {
        var data = (WaveCreationParams)target;
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("Type").enumValueIndex = (int)data.Type;
        element.FindPropertyRelative("Count").intValue =
            data.Type == MobWave.WaveType.Boss ? 1 : 20;
        element.FindPropertyRelative("Prefab").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath(data.Path, typeof(GameObject)) as GameObject;
        serializedObject.ApplyModifiedProperties();
    }
	
	public override void OnInspectorGUI() {
		DrawDefaultInspector ();
		GUILayout.Space (10);
		serializedObject.Update();
		list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
		GUILayout.Space (10);
	}

    private struct WaveCreationParams
    {
        public MobWave.WaveType Type;
        public string Path;
    }
}
