using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameUI : MonoBehaviour {

    public enum Items
    {
        Magic = 0, 
        IceTower,
        ArrowTower,
        CannonTower
    }
    public GameObject m_ItemPanel;
    public Color m_holoColour = new Color(0, 0, 1, 1);
    public Color m_normalColor = new Color(1, 0, 0, 1);
    public Items m_currentItem = Items.Magic;
    private Items m_previousItem = Items.Magic;
    private bool m_shouldPlaceTower = false;
    private bool m_playingPlaceItemButtonAnimation = false;
    
    private Pointer pointer;
    private GameObject m_currentTile;
    private GameObject m_ObjectToPlace;
    GameManager m_gameManager;
    public Button UpgradeButton;
    

    void Start()
    {
        m_gameManager = GameManager.Instance;
        pointer = FindObjectOfType<Pointer>();
    }


    void Update()
    {
        m_currentTile = pointer.currentTile;
        if (!pointer.OverUI && m_shouldPlaceTower && m_currentTile)
        {
            NodePath nodeTile = m_currentTile.GetComponent<NodePath>();

            if (m_ObjectToPlace == null || m_currentItem != m_previousItem)
            {
                if (m_ObjectToPlace != null)
                    Destroy(m_ObjectToPlace);
                m_ObjectToPlace = Instantiate(Resources.Load("Prefabs/Towers/" + m_currentItem.ToString()), m_currentTile.transform.position, m_currentTile.transform.rotation) as GameObject;
                m_ObjectToPlace.GetComponent<TowerBase>().enabled = false;
                m_ObjectToPlace.GetComponent<TowerBase>().m_rangeIndicator.SetActive(true);
                m_ObjectToPlace.name.Replace("(Clone)", "_HOLOGRAPHIC_");
                m_previousItem = m_currentItem;
            }

            MoveItem(m_ObjectToPlace);

            if (nodeTile.pathType == NodePath.PathType.Grass && !nodeTile.towerPlaced && m_gameManager.gold - m_ObjectToPlace.GetComponent<TowerBase>().Cost >= 0)
            {
                SetItemColour(m_holoColour, m_ObjectToPlace.GetComponentsInChildren<Transform>());
                if(pointer.placeTower && AnalogueInput.GetRightTrigger() >= 1)
                {
                    PlaceItem();
                }
            }
            else
                SetItemColour(m_normalColor, m_ObjectToPlace.GetComponentsInChildren<Transform>());

            if(AnalogueInput.GetLeftTrigger() >= 1)
            {
                if (pointer.placeTower)
                {
                    pointer.placeTower = false;
                    m_shouldPlaceTower = false;
                    Destroy(m_ObjectToPlace);
                }
                else if (!pointer.towerInfo.activeInHierarchy)
                    FindObjectOfType<TowerPlacement>().HidePanel();

            }
            
        }

        if(pointer.selectedTower)
            UpgradeButton.interactable = pointer.selectedTower.GetComponent<TowerBase>().Upgradable;
    }

    public void SetItem(int item)
    {
        m_currentItem = (Items)item;
        m_shouldPlaceTower = true;
        pointer.placeTower = true;
    }

    private void SetItemColour(Color color, Transform[] transforms)
    {
        foreach (Transform transform in transforms)
        {
            if (transform.renderer != null)
            {
                if (transform.gameObject.name != "RangeIndicator")
                {
                    transform.renderer.material = Resources.Load("Prefabs/Materials/Holo") as Material;
                    transform.renderer.material.SetColor("_Colour", color);
                }
                else
                    transform.renderer.material.SetColor("_Color", color);
            }
        }
    }

    void MoveItem(GameObject item)
    {
        m_ObjectToPlace.transform.position = m_currentTile.transform.position;
        m_ObjectToPlace.transform.rotation = m_currentTile.transform.rotation;
    }

    void PlaceItem()
    {
        if (m_gameManager.gold - m_ObjectToPlace.GetComponent<TowerBase>().Cost >= 0)
        {
            GameObject placedTower = Instantiate(Resources.Load("Prefabs/Towers/" + m_currentItem.ToString()), m_currentTile.transform.position, m_currentTile.transform.rotation) as GameObject;
            placedTower.GetComponent<TowerBase>().Tile = m_currentTile.GetComponent<NodePath>();
            placedTower.GetComponent<TowerBase>().enabled = true;
            placedTower.GetComponent<TowerBase>().m_rangeIndicator.SetActive(false);
            placedTower.name.Replace("(Clone)", "");
            m_currentTile.GetComponent<NodePath>().towerPlaced = true;
            m_currentTile.GetComponent<NodePath>().placedTower = placedTower.GetComponent<TowerBase>();
            GameObject.FindObjectOfType<GameManager>().RemoveGold(m_ObjectToPlace.GetComponent<TowerBase>().Cost);
        }
    }

    public void SellTower()
    {
        GameManager.Instance.AddGold(pointer.selectedTower.GetComponent<TowerBase>().Cost * 0.9f);
        pointer.selectedTower.GetComponent<TowerBase>().Die();
        pointer.towerInfo.SetActive(false);
        pointer.selectedTower = null;
    }

    public void UpgradeTower()
    {
        pointer.selectedTower.GetComponent<TowerBase>().Upgrade();
        pointer.towerLevel.text = "Level: " + pointer.selectedTower.GetComponent<TowerBase>().Level;
        pointer.towerDamage.text = "Health: " + Mathf.RoundToInt(pointer.selectedTower.GetComponent<TowerBase>().Health);
    }
}
