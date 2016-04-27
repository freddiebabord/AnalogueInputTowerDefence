/*
    Script: AnalogueButtonEditor.cs
    Author: Frederic Babord
    This script is an extention of the Unity UI system that handles custom axis based
    analogue inputs instead of the generic mouse click functionality. This shows the
    defualt button inspector with the analogue axis string box.
*/
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[CustomEditor(typeof(AnalogueButtons))]
public class AnaoguButtonEditor : UnityEditor.UI.ButtonEditor {

	public override void OnInspectorGUI() {
        AnalogueButtons component = (AnalogueButtons)target;

        component.clickAnalogueAxis = EditorGUILayout.TextField(
            new GUIContent("Analogue Axis", "The input AXIS that acts as a click check"), 
            component.clickAnalogueAxis
        );

		base.OnInspectorGUI();
	}
}
