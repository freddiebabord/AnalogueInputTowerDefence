using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Pointer : MonoBehaviour {

	public bool placeTower = false;
	public bool placeTile = false;

	public GameObject selectedTower;

	RectTransform rt;
    AnalogueButtons selectedButton;
    public GameObject currentTile;
    Color currentTileOriginalColour;
    CameraController cameraController;

    public float maxHorizontal = 18;
    public float maxVertical = 9;

    public GameObject towerInfo;
    public Text towerName;
    public Text towerLevel;
    public Text towerDamage;
    public GameObject selectedTowerEffect;
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
				transform.Translate(Vector3.right * (ConfigSettings.Instance.invertXAxis ? AnalogueInput.GetLeftHorizontal() * ConfigSettings.Instance.sensitivity * Time.deltaTime : -AnalogueInput.GetLeftHorizontal()) * ConfigSettings.Instance.sensitivity * Time.deltaTime);
            else
				cameraController.TranslatCameraHorisontal(AnalogueInput.GetLeftHorizontal());
        }

		if (AnalogueInput.GetLeftVertical() >= 0.1f || AnalogueInput.GetLeftVertical() <= -0.1f)
        {
			if (rt.localPosition.y + AnalogueInput.GetLeftVertical() > -maxVertical-1 &&
			    rt.localPosition.y + AnalogueInput.GetLeftVertical() < maxVertical+1)
				transform.Translate(Vector3.up * (ConfigSettings.Instance.invertYAxis ? AnalogueInput.GetLeftVertical() * ConfigSettings.Instance.sensitivity * Time.deltaTime : -AnalogueInput.GetLeftVertical()) * ConfigSettings.Instance.sensitivity * Time.deltaTime);
            else
				cameraController.TranslatCameraVertical(AnalogueInput.GetLeftVertical());
        }
        
        
        // Simulates the OnSelect event
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Camera.main.WorldToScreenPoint(transform.position);
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0) {
			for (int i = 0; i < raycastResults.Count; i++) {
				if (raycastResults [i].gameObject.GetComponent<AnalogueButtons> ()) {
					overUI = true;
					if (selectedButton != raycastResults [i].gameObject.GetComponent<AnalogueButtons> ()) {
						if (selectedButton != null)
							selectedButton.OnDeselect ();

						selectedButton = raycastResults [i].gameObject.GetComponent<AnalogueButtons> ();
						selectedButton.OnSelect ();
						EventSystem.current.SetSelectedGameObject (selectedButton.gameObject);
					}
				}
			}
		} 
        else 
        {
			overUI = false;
			if(selectedButton){
				selectedButton.OnDeselect();
				selectedButton = null;
			}
		}

        if (AnalogueInput.GetLeftTrigger() >= 1)
        {
            if(towerInfo.activeInHierarchy)
                DiableTowerSelctionUIAndEffect();
                
        }

        if (!overUI) 
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);
            foreach(RaycastHit hit in hits)
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.yellow);
                if (hit.collider.gameObject.GetComponent<NodePath>())
                {
                    if (placeTower)
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
                    else if (AnalogueInput.GetRightTrigger() >= 1)
                    {
                        TowerBase tc = hit.collider.gameObject.GetComponent<NodePath>().placedTower;
                        if (tc != null)
                        {
                            selectedTower = tc.gameObject;
                            EnableTowerSelectionUIAndEffect();
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
                    if(raycastResults[i].gameObject.GetComponent<NodePath>())
                    {
                        if(raycastResults[i].gameObject.GetComponent<NodePath>().placedTower != null)
                        {
                            selectedTower = raycastResults[i].gameObject.GetComponent<NodePath>().placedTower.gameObject;
                            towerInfo.SetActive(true);
                            towerName.text = selectedTower.name;
                            towerLevel.text = "Level: " + selectedTower.GetComponent<TowerBase>().Level;
                            towerDamage.text = "Health: " + Mathf.RoundToInt(selectedTower.GetComponent<TowerBase>().Health);
                        }
                    }
                }
            } 
        }


		pointer.Reset ();

	}

    public void EnableTowerSelectionUIAndEffect()
    {
        TowerBase tc = selectedTower.GetComponent<TowerBase>();
        towerInfo.SetActive(true);
        selectedTowerEffect.SetActive(true);
        selectedTowerEffect.transform.position = selectedTower.transform.position;
        towerName.text = selectedTower.gameObject.name;
        towerLevel.text = "Level: " + tc.Level;
        towerDamage.text = "Health: " + Mathf.RoundToInt(tc.Health);
    }

    public void DiableTowerSelctionUIAndEffect()
    {
        towerInfo.SetActive(false);
        selectedTowerEffect.SetActive(false);
    }
}

