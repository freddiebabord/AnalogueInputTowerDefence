using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Pointer : MonoBehaviour {

	RectTransform rt;
    List<AnalogueButtons> selectedButtons = new List<AnalogueButtons>();
    GameObject latSelected;
    GameObject currentTile;
    Color currentTileOriginalColour;
    public float pointerSpeed = 5.0f;
    public string horizontalAxis = "HorizontalLeft";
    public string verticalAxis = "VerticalLeft";

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

        Vector3 inputVector = new Vector3(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis), 0.0f);
       // if (rt.position.x + inputVector.x < transform.parent.GetComponent<RectTransform>().sizeDelta.x / 2 && rt.position.x + inputVector.x > transform.parent.GetComponent<RectTransform>().sizeDelta.x / 2)
        {
            //if (rt.position.y + inputVector.y < transform.parent.GetComponent<RectTransform>().sizeDelta.y / 2 && rt.position.y + inputVector.y > transform.parent.GetComponent<RectTransform>().sizeDelta.y / 2)
                rt.Translate(inputVector * pointerSpeed * Time.deltaTime);
        }
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

        Ray screenToGround = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(screenToGround, out hit, 150))
        {
            Debug.DrawLine(transform.position, hit.transform.position, Color.yellow);
            if(hit.collider.gameObject.GetComponent<NodePath>())
            {
                if (currentTile != hit.collider.gameObject)
                {
                    if (hit.collider.gameObject.GetComponent<NodePath>().pathType == NodePath.PathType.Grass)
                    {
                        if (currentTile != null)
                            currentTile.renderer.material.SetColor("_DiffuseColour", currentTileOriginalColour);
                        currentTile = hit.collider.gameObject;
                        currentTileOriginalColour = currentTile.renderer.material.GetColor("_DiffuseColour");
                        currentTile.renderer.material.SetColor("_DiffuseColour", new Color(0, 1, 0, 1));
                    }
                    else
                    {
                        if (currentTile != null)
                            currentTile.renderer.material.SetColor("_DiffuseColour", currentTileOriginalColour);
                        currentTile = hit.collider.gameObject;
                        currentTileOriginalColour = currentTile.renderer.material.GetColor("_DiffuseColour");
                        currentTile.renderer.material.SetColor("_DiffuseColour", new Color(1, 0, 0, 1));
                    }
                }
            }
        }



        // Simulation of click event
        if(Input.GetAxis("TriggerSelectRight") >= 1)
        {
            if (raycastResults.Count > 0)
            {
                for (int i = 0; i < raycastResults.Count; i++)
                {
                    if (raycastResults[i].gameObject.GetComponent<AnalogueButtons>())
                    {
                        if (!raycastResults[i].gameObject.GetComponent<AnalogueButtons>().clicked)
                            raycastResults[i].gameObject.GetComponent<AnalogueButtons>().OnClick();
                    }
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
