using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Pointer : MonoBehaviour {

	RectTransform rt;
    List<AnalogueButtons> selectedButtons = new List<AnalogueButtons>();
    GameObject latSelected;

	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform> ();
        if (!Application.isEditor)
        {
            Screen.lockCursor = true;
            Screen.showCursor = false;
        }
	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetAxis("TriggerSelect") >= 1 && Input.GetAxis("TriggerSelect2") >= 1)
        //{
        //    rt.position = new Vector3(0, 0, 0);
        //}

        rt.Translate (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);

        // Simulates the OnSelect event
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Camera.main.WorldToScreenPoint(transform.position);
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);
        if (raycastResults.Count > 0)
        {
            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (raycastResults[i].gameObject.GetComponent<AnalogueButtons>())
                {
                    if (!raycastResults[i].gameObject.GetComponent<AnalogueButtons>().isSelected)
                    {
                        selectedButtons.Add(raycastResults[i].gameObject.GetComponent<AnalogueButtons>());
                        raycastResults[i].gameObject.GetComponent<AnalogueButtons>().OnSelect();
                    }
                }
            }
        }

        // Simulation of click event
        if(Input.GetAxis("TriggerSelect") >= 1)
        {
            if (raycastResults.Count > 0)
            {
                for (int i = 0; i < raycastResults.Count; i++)
                {
                    if (raycastResults[i].gameObject.GetComponent<AnalogueButtons>())
                        raycastResults[i].gameObject.GetComponent<AnalogueButtons>().OnClick();
                }
            } 
        }

        foreach (AnalogueButtons button in selectedButtons)
        {
            if (!button.clicked)
                button.OnDeselect();
        }

        selectedButtons.Clear();

        // Hack to prevent mouse clicks
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(latSelected);
        else
            latSelected = EventSystem.current.currentSelectedGameObject;

	}
}
