using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[CustomEditor(typeof(AnalogueButtons))]
public class AnaoguButtonEditor : UnityEditor.UI.ButtonEditor {

	public override void OnInspectorGUI() {
        //AnalogueButtons component = (AnalogueButtons)target;

		base.OnInspectorGUI();
	}
}
