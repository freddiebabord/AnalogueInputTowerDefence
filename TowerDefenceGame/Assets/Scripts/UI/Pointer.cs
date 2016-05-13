using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Pointer : MonoBehaviour {

	public bool placeTower = false;
	public bool placeTile = false;

	GameObject go;

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

        if (!overUI)
        {
            Ray screenToGround = new Ray(transform.position, transform.forward);
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);
            foreach(RaycastHit hit in hits)
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.yellow);
                if (hit.collider.gameObject.GetComponentInChildren<TowerBase>() && AnalogueInput.GetRightTrigger() >= 1)
                {
                    TowerBase tc = hit.collider.gameObject.GetComponentInChildren<TowerBase>();
                    towerInfo.SetActive(true);
                    towerName.text = hit.collider.gameObject.name;
                    //towerLevel.text = "Level: " + tc.GetLevel();
                    //towerDamage.text = "Health: " + Mathf.RoundToInt(tc.GetHealth());

					go = hit.collider.gameObject;
                } 
                else if (hit.collider.gameObject.GetComponent<NodePath>())
                {
					if(placeTower)
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


		pointer.Reset ();

	}

    //public void Sell()
    //{
    //    if (go != null) 
    //    {

    //        if (go.tag == "Arrow") 
    //        {
    //            var bui = GameObject.FindObjectOfType<ArrowUI> ();
			
    //            float gold = bui.cost - (10 * (bui.cost / bui.cost));
    //            GameManager.Instance.AddGold (gold);
    //            GameObject tiley = SearchTile (go.gameObject.transform.position);
    //            tiley.GetComponent<NodePath> ().towerPlaced = false;
    //            Destroy (go);
    //        } 
    //        else if (go.tag == "Mage") 
    //        {
    //            var bui = GameObject.FindObjectOfType<Magic> ();
			
    //            float gold = bui.cost - (10 * (bui.cost / bui.cost));
    //            GameManager.Instance.AddGold (gold);
    //            GameObject tiley = SearchTile (go.gameObject.transform.position);
    //            tiley.GetComponent<NodePath> ().towerPlaced = false;
    //            Destroy (go);
    //        } 
    //        else if (go.tag == "Balista") 
    //        {
    //            var bui = GameObject.FindObjectOfType<ballistaUI> ();
			
    //            float gold = bui.cost - (10 * (bui.cost / bui.cost));
    //            GameManager.Instance.AddGold (gold);
    //            GameObject tiley = SearchTile (go.gameObject.transform.position);
    //            tiley.GetComponent<NodePath> ().towerPlaced = false;
    //            Destroy (go);
			
    //        } 
    //        else if (go.tag == "Freeze") 
    //        {
    //            var bui = GameObject.FindObjectOfType<Ice> ();
			
    //            float gold = bui.cost - (10 * (bui.cost / bui.cost));
    //            GameManager.Instance.AddGold (gold);
    //            GameObject tiley = SearchTile (go.gameObject.transform.position);
    //            tiley.GetComponent<NodePath> ().towerPlaced = false;
    //            Destroy (go);
			
    //        }
    //    }
    //}

	GameObject SearchTile(Vector3 pos)
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("Grass");
		
		foreach (GameObject t in tiles) 
		{
			if(t.transform.position == pos)
				return t;
			else
				continue;
		}
		
		return null;
	}
}
