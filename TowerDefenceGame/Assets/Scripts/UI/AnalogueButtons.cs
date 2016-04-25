using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
[AddComponentMenu("UI/Analogue Button", 30)]

public class AnalogueButtons : Button, ISelectHandler, IDeselectHandler {
    
	public bool isHovering = false;
    [HideInInspector]
    public bool isSelected = false;

	private float clickTimeout = 0.5f;
	[HideInInspector]
    public bool clicked = false;
    
	// Update is called once per frame
	void Update () {
        //if (isSelected) {
        //    if(Input.GetAxis("TriggerSelect") >= 1)
        //        this.OnClick();
        //}
	}

    public void OnSelect()
    {
        this.isSelected = true;
        this.DoStateTransition(SelectionState.Highlighted, false);
    }

	public void OnSelect (BaseEventData eventData) 
	{
        this.isSelected = true;
        this.DoStateTransition(SelectionState.Highlighted, false);
	}

    public void OnDeselect()
    {
        this.isSelected = false;
        this.DoStateTransition(SelectionState.Normal, false);
    }

	public void OnDeselect (BaseEventData data) 
	{
        this.isSelected = false;
        this.DoStateTransition(SelectionState.Normal, false);
	}


	public void OnClick()
	{
		if (!clicked) {

            this.onClick.Invoke();
            this.DoStateTransition(SelectionState.Pressed, false);
			StartCoroutine(clickTimeOut());
		}
        
	}

	IEnumerator clickTimeOut()
	{
        this.clicked = true;
		yield return new WaitForSeconds (clickTimeout);
        OnDeselect();
        this.clicked = false;
	}

}
