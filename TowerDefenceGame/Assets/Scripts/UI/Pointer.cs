using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Pointer : MonoBehaviour {

	public bool placeTower = false;

	RectTransform rt;
    List<AnalogueUIElement> selectedUI = new List<AnalogueUIElement>();
    GameObject latSelected;
    public GameObject currentTile;
    Color currentTileOriginalColour;
    public float pointerSpeed = 5.0f;
    public string horizontalAxis = "HorizontalLeft";
    public string verticalAxis = "VerticalLeft";
    CameraController cameraController;

    public float maxHorizontal = 18;
    public float maxVertical = 9;

    public bool invertXAxis = false, invertYAxis = false;

	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform> ();
        if (!Application.isEditor)
        {
            Screen.lockCursor = true;
            Screen.showCursor = false;
        }
        cameraController = GameObject.FindObjectOfType<CameraController>();
	}


	
	// Update is called once per frame
	void Update () {

        if (Input.GetAxis(horizontalAxis) >= 0.1f || Input.GetAxis(horizontalAxis) <= -0.1f)
        {
            if (rt.localPosition.x + Input.GetAxis(horizontalAxis) > -maxHorizontal-1 &&
                rt.localPosition.x + Input.GetAxis(horizontalAxis) < maxHorizontal+1)
                transform.Translate(Vector3.right * (invertXAxis ? Input.GetAxis(horizontalAxis) : -Input.GetAxis(horizontalAxis)) * pointerSpeed * Time.deltaTime);
            else
                cameraController.TranslatCameraHorisontal(Input.GetAxis(horizontalAxis));
        }

        if (Input.GetAxis(verticalAxis) >= 0.1f || Input.GetAxis(verticalAxis) <= -0.1f)
        {
            if (rt.localPosition.y + Input.GetAxis(verticalAxis) > -maxVertical-1 &&
                    rt.localPosition.y + Input.GetAxis(verticalAxis) < maxVertical+1)
                transform.Translate(Vector3.up * (invertYAxis ? Input.GetAxis(verticalAxis) : -Input.GetAxis(verticalAxis)) * pointerSpeed * Time.deltaTime);
            else
                cameraController.TranslatCameraVertical(Input.GetAxis(verticalAxis));
        }
        

		// TODO: FREDDIE - Rest pointer pos if greater than bounds

        
        // Simulates the OnSelect event
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Camera.main.WorldToScreenPoint(transform.position);
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);
        if (raycastResults.Count > 0)
        {
            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (raycastResults[i].gameObject.GetComponent<AnalogueUIElement>())
                {
                    Debug.Log(raycastResults[i].gameObject.GetComponent<AnalogueUIElement>().gameObject);
                    if (!raycastResults[i].gameObject.GetComponent<AnalogueUIElement>().isSelected)
                    {

                        selectedUI.Add(raycastResults[i].gameObject.GetComponent<AnalogueUIElement>());
                        raycastResults[i].gameObject.GetComponent<AnalogueUIElement>().OnSelect();
                    }
                }
            }
        }

		if (placeTower) 
		{
			Ray screenToGround = new Ray (transform.position, transform.forward);
			RaycastHit hit;
			if (Physics.Raycast (screenToGround, out hit, 150)) 
			{
				Debug.DrawLine (transform.position, hit.transform.position, Color.yellow);
				if (hit.collider.gameObject.GetComponent<NodePath> ()) 
				{
					if (currentTile != hit.collider.gameObject) 
					{
						if (hit.collider.gameObject.GetComponent<NodePath> ().pathType == NodePath.PathType.Grass) 
						{
							if (currentTile != null)
								currentTile.renderer.material.SetColor ("_DiffuseColour", currentTileOriginalColour);
							currentTile = hit.collider.gameObject;
							currentTileOriginalColour = currentTile.renderer.material.GetColor ("_DiffuseColour");
							currentTile.renderer.material.SetColor ("_DiffuseColour", new Color (0, 1, 0, 1));
						} 
						else 
						{
							if (currentTile != null)
								currentTile.renderer.material.SetColor ("_DiffuseColour", currentTileOriginalColour);
							currentTile = hit.collider.gameObject;
							currentTileOriginalColour = currentTile.renderer.material.GetColor ("_DiffuseColour");
							currentTile.renderer.material.SetColor ("_DiffuseColour", new Color (1, 0, 0, 1));
						}
					}
				}
			}
		}
        else if(currentTile)
        {
            currentTile.renderer.material.SetColor("_DiffuseColour", currentTileOriginalColour);
            currentTile = null;
        }


        // Simulation of click event
        if(Input.GetAxis("TriggerSelectRight") >= 1)
        {
            if (raycastResults.Count > 0)
            {
                for (int i = 0; i < raycastResults.Count; i++)
                {
                    if (raycastResults[i].gameObject.GetComponent<AnalogueUIElement>())
                    {
                        if (!raycastResults[i].gameObject.GetComponent<AnalogueUIElement>().clicked)
						{
                            raycastResults[i].gameObject.GetComponent<AnalogueUIElement>().OnClick();
						}
                    }
                }
            } 
        }

        foreach (AnalogueUIElement button in selectedUI)
        {
            if (!button.clicked)
                button.OnDeselect();
        }

        selectedUI.Clear();

        //// Hack to prevent mouse clicks
        //if (EventSystem.current.currentSelectedGameObject == null)
        //    EventSystem.current.SetSelectedGameObject(latSelected);
        //else
        //    latSelected = EventSystem.current.currentSelectedGameObject;

	}
}
