using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Pointer : MonoBehaviour {

	public bool placeTower = false;
	public bool placeTile = false;

	RectTransform rt;
    List<AnalogueButtons> selectedButtons = new List<AnalogueButtons>();
    public GameObject currentTile;
    Color currentTileOriginalColour;
    public string horizontalAxis = "HorizontalLeft";
    public string verticalAxis = "VerticalLeft";
    CameraController cameraController;

    public float maxHorizontal = 18;
    public float maxVertical = 9;

    public GameObject towerInfo;
    public Text towerName;
    public Text towerLevel;
    public Text towerDamage;

    public bool invertXAxis = false, invertYAxis = false;

    private bool overUI = false;
    public bool OverUI { get { return overUI; } }


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
        
        overUI = false;

		if (AnalogueInput.GetLeftHorizontal() >= 0.1f || AnalogueInput.GetLeftHorizontal() <= -0.1f)
        {
			if (rt.localPosition.x + AnalogueInput.GetLeftHorizontal() > -maxHorizontal-1 &&
			    rt.localPosition.x + AnalogueInput.GetLeftHorizontal() < maxHorizontal+1)
				transform.Translate(Vector3.right * (ConfigSettings.Instance.invertXAxis ? AnalogueInput.GetLeftHorizontal() * ConfigSettings.Instance.sensitivity * Time.unscaledDeltaTime : -AnalogueInput.GetLeftHorizontal()) * ConfigSettings.Instance.sensitivity * Time.unscaledDeltaTime);
            else
				cameraController.TranslatCameraHorisontal(AnalogueInput.GetLeftHorizontal());
        }

		if (AnalogueInput.GetLeftVertical() >= 0.1f || AnalogueInput.GetLeftVertical() <= -0.1f)
        {
			if (rt.localPosition.y + AnalogueInput.GetLeftVertical() > -maxVertical-1 &&
			    rt.localPosition.y + AnalogueInput.GetLeftVertical() < maxVertical+1)
				transform.Translate(Vector3.up * (ConfigSettings.Instance.invertYAxis ? AnalogueInput.GetLeftVertical() * ConfigSettings.Instance.sensitivity * Time.unscaledDeltaTime : -AnalogueInput.GetLeftVertical()) * ConfigSettings.Instance.sensitivity * Time.unscaledDeltaTime);
            else
				cameraController.TranslatCameraVertical(AnalogueInput.GetLeftVertical());
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
                    overUI = true;
                    if (!raycastResults[i].gameObject.GetComponent<AnalogueButtons>().isSelected)
                    {
                        selectedButtons.Add(raycastResults[i].gameObject.GetComponent<AnalogueButtons>());
                        raycastResults[i].gameObject.GetComponent<AnalogueButtons>().OnSelect();
						EventSystem.current.SetSelectedGameObject (raycastResults [i].gameObject);
                    }
                }
            }
            if (selectedButtons.Count > 0)
                overUI = false;
        }

		if (placeTower || placeTile) 
		{
            if (!overUI)
            {
                Ray screenToGround = new Ray(transform.position, transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(screenToGround, out hit, 150))
                {
                    Debug.DrawLine(transform.position, hit.transform.position, Color.yellow);
                    if (hit.collider.gameObject.GetComponent<TowerClass>())
                    {
                        TowerClass tc = hit.collider.gameObject.GetComponent<TowerClass>();
                        towerInfo.SetActive(true);
                        towerName.text = tc.name;
                        towerLevel.text = "Level: " + tc.GetLevel();
                        towerDamage.text = "Health: " + Mathf.RoundToInt(tc.GetHealth());
                    } 
                    else if (hit.collider.gameObject.GetComponent<NodePath>())
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
            }
		}
        else if(currentTile)
        {
            currentTile.renderer.material.SetColor("_DiffuseColour", currentTileOriginalColour);
            currentTile = null;
            towerInfo.SetActive(false);
        }


        // Simulation of click event
		if(AnalogueInput.GetRightTrigger() >= 1)
        {
			if (AnalogueInput.GetLeftTrigger() >= 1)
            {
                transform.localPosition = new Vector3(0, 0, 0);
            }
            if (raycastResults.Count > 0)
            {
                for (int i = 0; i < raycastResults.Count; i++)
                {
                    if (raycastResults[i].gameObject.GetComponent<AnalogueButtons>())
                    {
                        if (!raycastResults[i].gameObject.GetComponent<AnalogueButtons>().clicked)
						{
                            raycastResults[i].gameObject.GetComponent<AnalogueButtons>().OnClick();
						}
                    }
                }
            } 
        }


        foreach (AnalogueButtons button in selectedButtons)
        {
            if (!button.clicked)
                button.OnDeselect();
        }
		pointer.Reset ();
		pointer
        selectedButtons.Clear();

	}
}
